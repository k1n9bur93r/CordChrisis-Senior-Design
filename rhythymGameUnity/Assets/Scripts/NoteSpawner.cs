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
    void Start()
    {
        noteReciever = transform.Find("noteReceiver").transform;
        noteTravelTime = startDistance / noteSpeed;
        print("Note travel time in seconds is: " + noteTravelTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            spawnNote(0);
        }
        if (Input.GetKeyDown("w"))
        {
            spawnNote(1);
        }
        if (Input.GetKeyDown("e"))
        {
            spawnNote(2);
        }
        if (Input.GetKeyDown("r"))
        {
            spawnNote(3);
        }
    }

    //This function takes a note number from 0-3 and spawns it
    public void spawnNote(int noteNum)
    {
        GameObject curNote = Instantiate(noteObjects[noteNum], new Vector3(noteXoffsets[noteNum], yOffset, noteReciever.position.z+startDistance), transform.rotation);
        curNote.GetComponent<NoteMovement>().noteSpeed = noteSpeed;
        curNote.transform.parent = transform;
    }
}
