using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureSpawner : MonoBehaviour
{
    public EditorNoteController editorNoteController;
    public List<GameObject> gestureObjects;
    // public List<GameObject>[] gestures = new List<GameObject>[4];

    public Dictionary<double, GameObject> gestureGameObjects = new Dictionary<double, GameObject>();
    public Dictionary<double, int> gestureNums = new Dictionary<double, int>();

    private int cycle = 0;
    public bool isGestureAlive = false;
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;

    void Awake()
    {
        // for (int i = 0; i < 4; i++)
        // {
        //     gestures[i] = new List<GameObject>();
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void spawnGesture(int gestureNum, double beat)
    {
        removeGesture();
        GameObject curGesture = Instantiate(gestureObjects[gestureNum], new Vector3(0f, 2.0f, 0f), Quaternion.Euler(31.69f, 0, 0));

        curGesture.AddComponent<NoteData>();
        curGesture.GetComponent<NoteData>().beat = editorNoteController.curBeat;
        curGesture.GetComponent<NoteData>().gestureNum = gestureNum + 5;
        
        editorNoteController.AddNote(4, curGesture);
    }

    void removeGesture()
    {
        if (editorNoteController.isNoteIn(4))
        {
            Destroy(editorNoteController.notes[editorNoteController.curBeat][4]);
            //adding null at spot 5 is removing gesture from that beat
            editorNoteController.AddNote(4, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up")) {
            spawnGesture(0, 4);
        } else if (Input.GetKey("right")) {
            spawnGesture(1, 4);
        } else if (Input.GetKey("down")) {
            spawnGesture(2, 4);
        } else if (Input.GetKey("left")) {
            spawnGesture(3, 4);
        } else if (Input.GetKey("backspace")) {
            Debug.Log("delete");
            removeGesture();
        }

        // is any note out there?
        bool check = (  p1.GetComponent<NoteCreator>().isNoteAlive ||
                        p2.GetComponent<NoteCreator>().isNoteAlive ||
                        p3.GetComponent<NoteCreator>().isNoteAlive ||
                        p4.GetComponent<NoteCreator>().isNoteAlive) ? true : false;

        if (Input.GetMouseButtonDown(1) && !check)
        {
            if (cycle >= 0 && cycle < 4)
            {
                spawnGesture(cycle, 4);
                isGestureAlive = true;
            }
            else
            {
                removeGesture();
                cycle = -1;
                isGestureAlive = false;
            }
            cycle++;
        }
    }
}
