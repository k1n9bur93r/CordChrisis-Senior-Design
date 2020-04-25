using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class EditorNoteController : MonoBehaviour
{ 
    public double distPerBeat;
    public double curBeat;

    // stuff for creating the json file
    public TextMeshProUGUI title;
    public TextMeshProUGUI artist;
    public TextMeshProUGUI genre;
    public TMP_Text tempo;
    public TMP_Text offset;
    public TextMeshProUGUI difficulty;

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
    public SortedDictionary<double, GameObject[]> notes;

    // note objects
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject gs;

    public double scrollIncrement;
    private double[] divisions;
    private int divIndex;

    public TMP_Text beatText;
    public TMP_Text tempoText;
    public TMP_Text incrementText;    

    // Start is called before the first frame update
    void Start()
    {
        notes = new SortedDictionary<double,GameObject[]>(); 
        curBeat = 0;
        divIndex = 0;
        divisions = new double[6] { 1.0, 0.5, 0.33, 0.25, 0.125, 0.0625 };
        scrollIncrement = divisions[divIndex];     // default increment
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) {
            Debug.Log(ToJson());
        }

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
        beatText.text = "current beat: " + curBeat;
        tempoText.text = "current tempo: ";// + tempo;
        incrementText.text = "beat increment: " + scrollIncrement;
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

    double ConvertToDouble(string num)
    {
        // TODO: SHOULD ADD ERROR CHECKING AND NOTIFY THE USER

        // TextMeshPro ends things in a ZERO WIDTH SPACE
        Char lastChar = num.ToCharArray()[num.Length - 1];
        if (Convert.ToInt16(lastChar) == 8203) {
            // ends in a ZERO WIDTH SPACE
            num = num.Substring(0, num.Length - 1);
        }

        if (num == null || num.Equals("")) {
            return 0;
        }

        return Convert.ToDouble(num);
    }

    public string ToJson()
    {
        GameObject go;
        JsonTrack json = new JsonTrack();
        List<double> beats = new List<double>();
        List<int> notes_nums = new List<int>();
        List<double> note_lengths = new List<double>();

        foreach (KeyValuePair<double, GameObject[]> kvp in notes)
        {

            // tap / held notes
            for (int i = 0; i < 4; i++) {
                go = kvp.Value[i];
                if (go != null) {
                    beats.Add(kvp.Key);
                    notes_nums.Add(i + 1);
                    note_lengths.Add(go.GetComponent<NoteData>().length);
                }
            }

                // gesture notes
            go = kvp.Value[4];
            if (go != null) {
                beats.Add(kvp.Key);
                notes_nums.Add(go.GetComponent<NoteData>().gestureNum);
                note_lengths.Add(go.GetComponent<NoteData>().length);
            }
        }

        json.beats = beats.ToArray();
        json.notes = notes_nums.ToArray();
        json.note_lengths = note_lengths.ToArray();

        json.title = title.text;
        json.artist = artist.text;
        json.genre = genre.text;
        json.difficulty = difficulty.text;

        json.offset = ConvertToDouble(offset.text);
        json.tempo_normal = ConvertToDouble(tempo.text);

        return JsonUtility.ToJson(json);
    }

    public void ScrollBeat()
    {
        bool scrollable = p1.GetComponent<NoteCreator>().isNoteAlive ||
                            p2.GetComponent<NoteCreator>().isNoteAlive ||
                            p3.GetComponent<NoteCreator>().isNoteAlive ||
                            p4.GetComponent<NoteCreator>().isNoteAlive ||
                            gs.GetComponent<GestureSpawner>().isGestureAlive ? true : false;

        // if (scrollable)
        
        // keys for scrolling up/down
        if (Input.GetKeyDown(KeyCode.W))
        {
            curBeat += scrollIncrement;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            curBeat -= scrollIncrement;
        }
        
    }

    public void ChangeIncrement()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {            
            divIndex = divIndex < divisions.Length - 1 ? divIndex + 1 : 0;
            scrollIncrement = divisions[divIndex];
            //Debug.Log(scrollIncrement);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            divIndex = divIndex > 0 ? divIndex - 1 : divisions.Length - 1;
            scrollIncrement = divisions[divIndex];
            //Debug.Log(scrollIncrement);
        }
    }
}
