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
    //how long until the note hits the reciever
    public float noteTravelTime;
    //what y position the notes should be spawned at
    public float yOffset;
    private Transform noteReciever;
    private float zOffset;
    // Start is called before the first frame update
    void Start()
    {
        noteReciever = transform.Find("noteReceiver").transform;
        print(noteReciever.position.z);
        zOffset = noteTravelTime * noteSpeed + noteReciever.position.z;
        
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
        print(zOffset);
        GameObject curNote = Instantiate(noteObjects[noteNum], new Vector3(noteXoffsets[noteNum], yOffset, zOffset), transform.rotation);
        curNote.transform.parent = transform;
    }
}
