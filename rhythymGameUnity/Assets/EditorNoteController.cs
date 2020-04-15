using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorNoteController : MonoBehaviour
{ 
    public int curBeat = 0;

    /*
        atBeat:
            key = beat (double)
            value = array of 5 bools which 
                signify what gestures
                and notes start on it

            ex: value TFTFFFFF
            means that there is a chord of a first and third note

            the first 4 are the 4 notes and the next 4 are the 
            four gestures
    */
    public Dictionary<double, GameObject[]> notes;
    
    // Start is called before the first frame update
    void Start()
    {
        notes = new Dictionary<double,GameObject[]>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (KeyValuePair<double,GameObject[]> kvp in notes)
            {
                print(kvp.Key);

                foreach (GameObject gb in kvp.Value)
                {
                    print(gb);
                }
            }
        }
    }
    public bool isNoteIn(int noteNum)
    {
        //check if beat has any entries in dict
        if (notes.ContainsKey(curBeat))
        {
            //if it doesn't, then 
            return false;
        }
        else if (notes[curBeat][noteNum] == null)
        {
            return false;
        }
        //the note already exists!
        return true;
    }
    public void AddNote(int spot, GameObject newNote)
    {
        GameObject[] beatInfo;

        if (!notes.ContainsKey(curBeat))
        {
            //then create a new empty beat info key
            beatInfo = new GameObject[] {null,null,null,null};
        }
        else
        {
            beatInfo = notes[curBeat];
        }

        beatInfo[spot] = newNote;

        notes[curBeat] = beatInfo;
    }

}
