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

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject gs;    
    
    public double scrollIncrement;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        notes = new Dictionary<double,GameObject[]>(); 
        curBeat = 0;
        scrollIncrement = 0.25;     // default increment
        camera = Camera.main;
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

        ChangeIncrement();
        ScrollBeat();
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

    public void ScrollBeat()
    {
        bool scrollable =   p1.GetComponent<NoteCreator>().isNoteAlive ||
                            p2.GetComponent<NoteCreator>().isNoteAlive ||
                            p3.GetComponent<NoteCreator>().isNoteAlive ||
                            p4.GetComponent<NoteCreator>().isNoteAlive ||
                            gs.GetComponent<GestureSpawner>().isGestureAlive ? true : false;

        if (scrollable)
        {
            // scroll up/down either the camera/the track here

            if (Input.GetKeyDown(KeyCode.W))
            {
                camera.transform.position += new Vector3(0, 0, (float)scrollIncrement);
                //curBeat += scrollIncrement;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                camera.transform.position -= new Vector3(0, 0, (float)scrollIncrement);
                //curBeat -= scrollIncrement;
            }
        }
    }

    public void ChangeIncrement()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            scrollIncrement += 0.25;              
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            scrollIncrement -= 0.25;
        }
        //Debug.Log(scrollIncrement);
    }
}
