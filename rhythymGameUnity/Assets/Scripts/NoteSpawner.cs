using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    private const float NOTE_PADDING = 2.0f;

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

    void Start()
    {
           for (int i=0;i<4;i++)
            {
                notes[i] = new List<GameObject>();
            }

        bpm = metronome.tempo; //getting tempo from metronome
        noteReciever = transform.Find("noteReceiver").transform;
        noteTravelTime = startDistance / speedMod;
        print("The first notes should play in " + noteTravelTime + " seconds");
    }

    
    // Update is called once per frame
    // Code for debugging purposes
    void Update()
    {
        //bpm = metronome.tempo; //getting tempo from metronome
        if (Input.GetKeyDown("q"))
        {
            spawnGesture(0, 12);
        }
        if (Input.GetKeyDown("w"))
        {
            spawnGesture(1, 12);
        }
        if (Input.GetKeyDown("e"))
        {
            spawnGesture(2, 12);
        }
        if (Input.GetKeyDown("r"))
        {
            spawnGesture(3, 12);
        }
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
            Instantiate(noteObjects[noteNum], new Vector3(noteXoffsets[noteNum], yOffset, noteReciever.position.z + startDistance + beatToDistance((float) (beat))), transform.rotation);
        
        notes[noteNum].Add(curNote.gameObject);

        curNote.GetComponent<NoteMovement>().speedMod = speedMod;
        curNote.transform.parent = transform;
        curNote.GetComponent<NoteMovement>().metronome = metronome;
        curNote.GetComponent<NoteMovement>().beat = beat;
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
                
                var curNoteColor = notes[x][y].gameObject.GetComponent<MeshRenderer>().material.color;
                //set transparency of note
                if (beatDistance < transparentEnd)
                {
                    curNoteColor.a = 1f;
                }
                else if (beatDistance > transparentStart)
                {
                    curNoteColor.a = 0f;
                }
                else
                {
                    curNoteColor.a = 1f-((beatDistance-transparentEnd)/(transparentStart-transparentEnd));
                }
                notes[x][y].gameObject.GetComponent<MeshRenderer>().material.color = curNoteColor;
            }

            
        }


    }

}
