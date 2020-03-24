using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    private const float NOTE_PADDING = 4.0f;

    //holds all 4 note prefabs so they may be spawned
    public List<GameObject> noteObjects;
    public List<GameObject> gestureObjects;
    public List<float> noteXoffsets;
    //units of movement per second
    public float speedMod;
    //where the notes start from
    public float startDistance;
    //how long until the note hits the reciever - this gets computed from startDistance and speedMod
    private float noteTravelTime;
    //what y position the notes should be spawned at
    public float yOffset;
    private Transform noteReciever;
    // Start is called before the first frame update
    public double bpm; // this will need to be a reference to the metronomes bpm
    public Metronome metronome;

    //creating the queues for notes
    public List<GameObject>[] notes = new List<GameObject>[4];
    public List<GameObject>[] gestures = new List<GameObject>[4];
    //values for transparency 
    //(notes phase in from background as if coming from fog)
    public float transparentStart;
    public float transparentEnd;
    public GameObject holdNoteObject;
    public List<GameObject> holds;

    void Start()
    {
        for (int i=0;i<4;i++)
        {
            gestures[i] = new List<GameObject>();
            notes[i] = new List<GameObject>();
        }

        bpm = metronome.tempo; //getting tempo from metronome
        noteReciever = transform.Find("noteReceiver").transform;
        noteTravelTime = startDistance / speedMod;
        print("The first notes should play in " + noteTravelTime + " seconds");
    }    

    private float beatToDistance(double beat)
    {
        double time, distance;
        //convert beat to time
        time = (60 / bpm) * beat;
        //convert time to distance
        distance = (time * speedMod);
        return (float)distance;
    }

    //This function takes a note number from 0-3 and spawns it
    public void spawnNote(int noteNum, double beat, double length=0)
    {
        GameObject curNote =
            Instantiate(noteObjects[noteNum], new Vector3(noteXoffsets[noteNum], yOffset, noteReciever.position.z + startDistance + beatToDistance((float) (beat))), Quaternion.Euler(0, 0, 90));
        
        notes[noteNum].Add(curNote.gameObject);

        curNote.GetComponent<NoteMovement>().speedMod = speedMod;
        curNote.transform.parent = transform;
        curNote.GetComponent<NoteMovement>().metronome = metronome;
        curNote.GetComponent<NoteMovement>().beat = beat;
        curNote.GetComponent<NoteMovement>().length = length;

        if (length > 0)
        {
            GameObject endHold =
                Instantiate(noteObjects[noteNum], new Vector3(noteXoffsets[noteNum], yOffset, noteReciever.position.z + startDistance + beatToDistance((float) (beat))), Quaternion.Euler(0, 0, 90));
    
            endHold.transform.parent = transform;
            //print(length);
            //since it moves relative to first: beat = oldbeat+length
            endHold.GetComponent<NoteMovement>().beat = length+beat;
            curNote.GetComponent<NoteMovement>().speedMod = speedMod;
            curNote.GetComponent<NoteMovement>().metronome = metronome;
            curNote.GetComponent<HoldNoteLine>().secondNote = endHold;
            endHold.GetComponent<HoldNoteLine>().secondNote = curNote;
            endHold.GetComponent<LineRenderer>().startWidth=.2f;
            endHold.GetComponent<LineRenderer>().endWidth=.2f;
            
            holds.Add(endHold);
        }

    }

    public void spawnGesture(int gestureNum, double beat)
    {
        GameObject curGesture = 
            Instantiate(gestureObjects[gestureNum], new Vector3((noteXoffsets[1]+noteXoffsets[2])/2, yOffset, noteReciever.position.z + startDistance + beatToDistance((float) (beat))), Quaternion.Euler(90, 0, 0));
        
        print(curGesture);


        gestures[gestureNum].Add(curGesture.gameObject);

        curGesture.GetComponent<NoteMovement>().speedMod = speedMod;
        curGesture.transform.parent = transform;
        curGesture.GetComponent<NoteMovement>().metronome = metronome;
        curGesture.GetComponent<NoteMovement>().beat = beat;
    }

    public void Update()
    {
        //set location of all notes according to beatsElapsed
        for (int x=0;x<4;x++)
        {
            for (int y=0;y<notes[x].Count;y++)
            {
                double curBeat = notes[x][y].GetComponent<NoteMovement>().beat;
                float beatDistance = (float)(curBeat-metronome.beatsElapsed) * speedMod * NOTE_PADDING;
                notes[x][y].transform.position = new Vector3 
                    (
                        notes[x][y].transform.position.x,
                        notes[x][y].transform.position.y,
                        (float)( noteReciever.transform.position.z + beatDistance )
                    );
                
                //new meshes break these. TODO: fix these
                var curNoteColor = notes[x][y].gameObject.GetComponent<MeshRenderer>().material.color;
            
                // if (notes[x][y].GetComponent<NoteMovement>().length>0)
                // {
                //     notes[x][y].GetComponent<LineRenderer>().startWidth=.2f;
                //     notes[x][y].GetComponent<LineRenderer>().endWidth=.2f;
                //     //then move end correctly
                //     GameObject end = notes[x][y].GetComponent<HoldNoteLine>().secondNote;

                //     curBeat = end.GetComponent<NoteMovement>().beat;
                //     beatDistance = (float)(curBeat-metronome.beatsElapsed) * speedMod * NOTE_PADDING;

                //     end.transform.position = new Vector3 
                //     (
                //         end.transform.position.x,
                //         end.transform.position.y,
                //         (float)( noteReciever.transform.position.z + beatDistance )
                //     );
                // }
            }
        }

        //move holds independently
        for (int x=holds.Count-1; x>=0; x--)
        {
            double curBeat = holds[x].GetComponent<NoteMovement>().beat;
            float beatDistance = (float)(curBeat-metronome.beatsElapsed) * speedMod * NOTE_PADDING;
            holds[x].transform.position = new Vector3 
                (
                    holds[x].transform.position.x,
                    holds[x].transform.position.y,
                    (float)( noteReciever.transform.position.z + beatDistance )
                );
            holds[x].GetComponent<LineRenderer>().startWidth=.2f;
            holds[x].GetComponent<LineRenderer>().endWidth=.2f;

            if (holds[x].transform.position.z < noteReciever.transform.position.z)
            {
                holds[x].SetActive(false);
                holds.RemoveAt(x);
            }
        }

        //set location of all gestures according to beatsElapsed
        for (int x=0;x<4;x++)
        {
            for (int y=0;y<gestures[x].Count;y++)
            {
                double curBeat = gestures[x][y].GetComponent<NoteMovement>().beat;
                float beatDistance = (float)(curBeat-metronome.beatsElapsed) * speedMod * NOTE_PADDING;
                gestures[x][y].transform.position = new Vector3 
                    (
                        gestures[x][y].transform.position.x,
                        gestures[x][y].transform.position.y,
                        (float)( noteReciever.transform.position.z + beatDistance )
                    );
                
                // var curNoteColor = gestures[x][y].gameObject.GetComponent<MeshRenderer>().material.color;
                //set transparency of note
                //new meshes break these too :^( must fix
                // if (beatDistance < transparentEnd)
                // {
                //     curNoteColor.a = 1f;
                // }
                // else if (beatDistance > transparentStart)
                // {
                //     curNoteColor.a = 0f;
                // }
                // else
                // {
                //     curNoteColor.a = 1f-((beatDistance-transparentEnd)/(transparentStart-transparentEnd));
                // }
                // gestures[x][y].gameObject.GetComponent<MeshRenderer>().material.color = curNoteColor;
            }
        }

        // DEBUG - Spawn gesture notes via key press
        // if (Input.GetKeyDown("1"))
        // {
        //     spawnGesture(0, 85);
        // }

        // if (Input.GetKeyDown("2"))
        // {
        //     spawnNote(1, 85,4);
        // }

        // if (Input.GetKeyDown("3"))
        // {
        //     spawnNote(2, 85,4);
        // }

        // if (Input.GetKeyDown("4"))
        // {
        //     spawnNote(3, 85,4);
        // }
    }
}
