using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour
{
    public int noteNum;
    public EditorNoteController editorController;
    public GameObject gs;

    // objects to create
    private GameObject note;
    private GameObject holdLine;
    private GameObject endNote;
    public GameObject line;
    public GameObject originalNote;

    // indicators
    private Color originalColor;
    private Color hoverColor;

    public bool isNoteAlive;

    void Start()
    {
        originalColor = GetComponent<MeshRenderer>().material.color;
        note = holdLine = endNote = null;
        hoverColor = originalColor;
        hoverColor.a = 1;
        isNoteAlive = false;
    }

    void Update()
    {
        
    }

    // in theory, these functions parallel touch input
    private void OnMouseUp()
    {

    }

    public void OnMouseDown()
    {
        if (editorController.isNoteIn(noteNum) == false && !gs.GetComponent<GestureSpawner>().isGestureAlive)
        {
            // init note setup
            note = Instantiate(originalNote);
            //note.GetComponent<Transform>().parent = this.transform;
            note.GetComponent<Transform>().position = this.transform.position;
            note.GetComponent<Transform>().position += new Vector3(0f, 0.5f, 0f);
            note.GetComponent<MeshRenderer>().material.color = hoverColor;
            note.GetComponent<BoxCollider>().enabled = false;
            
            //add the note to the editor dictionary
            editorController.AddNote(noteNum, note.gameObject);
            
            // note's beat should be set to current position in editor
            note.GetComponent<NoteData>().beat = editorController.curBeat;

            // tap notes length default to zero; length increases based on hold time
            note.GetComponent<NoteData>().length = 0.0;

            // hold note setup
            holdLine = Instantiate(line);
            holdLine.GetComponent<Transform>().parent = note.GetComponent<Transform>();
            holdLine.GetComponent<Transform>().position = note.GetComponent<Transform>().position;
            holdLine.GetComponent<MeshRenderer>().material.color = Color.white;
            holdLine.GetComponent<BoxCollider>().enabled = false;
            holdLine.SetActive(false);

            // end note setup
            endNote = Instantiate(originalNote);
            endNote.GetComponent<Transform>().parent = note.GetComponent<Transform>();
            endNote.GetComponent<Transform>().position = note.GetComponent<Transform>().position;

            // setting up for NoteData
            note.GetComponent<NoteData>().holdLine = holdLine;
            endNote.GetComponent<NoteData>().endNote = endNote;
            endNote.GetComponent<NoteData>().holdLine = holdLine;

            isNoteAlive = true;
        }
        else
        {
            Destroy(editorController.notes[editorController.curBeat][noteNum]);
            //Destroy(holdLine);
            //adding a null note is the same as removing it from the dict
            editorController.AddNote(noteNum, null);

            isNoteAlive = false;
        }
    }

    private void OnMouseEnter()
    {
        GetComponent<MeshRenderer>().material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.color = originalColor;
    }
}
