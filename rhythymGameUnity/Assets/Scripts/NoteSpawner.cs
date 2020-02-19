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
    public float bpm; // this will need to be a reference to the metronomes bpm
    void Start()
    {
        bpm = 60f; //this is temporary and for testing purposes!
        noteReciever = transform.Find("noteReceiver").transform;
        noteTravelTime = startDistance / noteSpeed;
        print("The first notes should play in " + noteTravelTime + " seconds");
    }

    // Update is called once per frame
    void Update()
    {
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

    private float beatToDistance(float beat)
    {
        float time, distance;
        //convert beat to time
        time = (60 / bpm) * beat;
        print(time);
        //convert time to distance
        distance = (time * noteSpeed);
        return distance;
    }

    //This function takes a note number from 0-3 and spawns it
    public void spawnNote(int noteNum, float beat)
    {
        GameObject curNote =
            Instantiate(noteObjects[noteNum], new Vector3(noteXoffsets[noteNum], yOffset, noteReciever.position.z+startDistance+beatToDistance(beat)), transform.rotation);
        curNote.GetComponent<NoteMovement>().noteSpeed = noteSpeed;
        curNote.transform.parent = transform;
    }


}
