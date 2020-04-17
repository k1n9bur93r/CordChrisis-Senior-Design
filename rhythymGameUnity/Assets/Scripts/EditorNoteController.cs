using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorNoteController : MonoBehaviour
{ 
    public double distPerBeat;
    public double curBeat;

    /*
        notes:
            key = beat (double)
            value = 5 game objects at that beat

        if gameobjects are null at that beat, then that beat
        does not have that type of note

        the first 4 are for the 4 notes, and the fifth is for
        any gesture note (multiple gesture notes may not be
        on the same beat)
    */
    public Dictionary<double, GameObject[]> notes;
    
    // Start is called before the first frame update
    void Start()
    {
        notes = new Dictionary<double,GameObject[]>(); 
        curBeat = 0;
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

        foreach (KeyValuePair<double,GameObject[]> kvp in notes)
        {
            foreach (GameObject gb in kvp.Value)
            {
                //set objects position
                if (gb != null)
                {
                    float newZ = (float)((gb.GetComponent<NoteData>().beat - curBeat) * (float)distPerBeat);

                    gb.transform.position = new Vector3 (gb.transform.position.x, gb.transform.position.y, newZ);
                }
            }
        }
    }
    public bool isNoteIn(int noteNum)
    {
        //check if beat has any entries in dict
        if (notes.ContainsKey(curBeat))
        {
            if (notes[curBeat][noteNum] != null)
            {
                return true;
            }
        }
        return false;
    }
    public void AddNote(int spot, GameObject newNote)
    {
        GameObject[] beatInfo;

        if (!notes.ContainsKey(curBeat))
        {
            //then create a new empty beat info key
            beatInfo = new GameObject[] {null,null,null,null,null};
        }
        else
        {
            beatInfo = notes[curBeat];
        }

        beatInfo[spot] = newNote;

        notes[curBeat] = beatInfo;
    }

}
