using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    //holds all 4 note prefabs so they may be spawned
    public List<GameObject> noteObjects;
    public List<float> noteXoffsets;
    //units of movement per second
    public float noteSpeed;
    //where the notes start from
    public float startDistance;
    //how long until the note hits the reciever - this gets computed from startDistance and noteSpeed
    private float noteTravelTime;
    //what y position the notes should be spawned at
    public float yOffset;
    private Transform noteReciever;
    // Start is called before the first frame update
    public double bpm; // this will need to be a reference to the metronomes bpm
    public Metronome metronome;

    //creating the queues for notes
    public List<GameObject>[] notes = new List<GameObject>[4];

    void Start()
    {
           for (int i=0;i<4;i++)
            {
                notes[i] = new List<GameObject>();
            }

        bpm = metronome.tempo; //getting tempo from metronome
        noteReciever = transform.Find("noteReceiver").transform;
        noteTravelTime = startDistance / noteSpeed;
        print("The first notes should play in " + noteTravelTime + " seconds");
    }

    // Update is called once per frame
    void Update()
    {
        //bpm = metronome.tempo; //getting tempo from metronome
        if (Input.GetKeyDown("q"))
        {
            spawnNote(0, 0);
        }
        if (Input.GetKeyDown("w"))
        {
            spawnNote(1, 0);
        }
        if (Input.GetKeyDown("e"))
        {
            spawnNote(2, 0);
        }
        if (Input.GetKeyDown("r"))
        {
            spawnNote(3, 0);
        }
    }

    private float beatToDistance(double beat)
    {
        double time, distance;
        //convert beat to time
        time = (60 / bpm) * beat;
        //convert time to distance
        distance = (time * noteSpeed);
        return (float)distance;
    }

    //This function takes a note number from 0-3 and spawns it
    public void spawnNote(int noteNum, double beat)
    {
        GameObject curNote =
            Instantiate(noteObjects[noteNum], new Vector3(noteXoffsets[noteNum], yOffset, noteReciever.position.z + startDistance + beatToDistance((float) (beat))), transform.rotation);
        
        notes[noteNum].Add(curNote.gameObject);

        curNote.GetComponent<NoteMovement>().noteSpeed = noteSpeed;
        curNote.transform.parent = transform;
        curNote.GetComponent<NoteMovement>().metronome = metronome;
        curNote.GetComponent<NoteMovement>().beat = beat;
    }

    void FixedUpdate()
    {
        for (int x=0;x<4;x++)
        {
            for (int y=0;y<notes[x].Count;y++)
            {
                double curBeat = notes[x][y].GetComponent<NoteMovement>().beat;
                notes[x][y].transform.position = new Vector3 
                    (
                        notes[x][y].transform.position.x,
                        notes[x][y].transform.position.y,
                        (float)( noteReciever.transform.position.z + (curBeat-metronome.beatsElapsed) * noteSpeed )
                    );
            }
        }
    }

}
