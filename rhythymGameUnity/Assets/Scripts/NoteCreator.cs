using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour
{
    public int noteNum;
    public EditorNoteController editorController;
    // objects to create
    private GameObject note;
    private GameObject holdLine;
    private GameObject endNote;
    public GameObject line;
    public GameObject originalNote;

    // indicators
    private Color originalColor;
    private Color hoverColor;

    // input info
    private bool clicked;
    private bool isHeld;
    private float holdTime;
    private static float scalar;

    void Start()
    {
        originalColor = GetComponent<MeshRenderer>().material.color;
        note = holdLine = endNote = null;
        hoverColor = originalColor;
        scalar = holdTime = 0;
        hoverColor.a = 1;
        clicked = false;
    }

    void Update()
    {
        if (clicked && note != null)
        {
            if (Input.GetMouseButton(0))
            {
                // should a hold note have a max length?
                scalar += 0.0025f;
                holdTime += Time.deltaTime;

                isHeld = holdTime > 0.20f ? true : false;

                if (isHeld)
                {
                    holdLine.SetActive(true);

                    // increases length on the z-axis
                    holdLine.GetComponent<Transform>().transform.localPosition += new Vector3(0f, 0f, scalar / 2.0f);
                    holdLine.GetComponent<Transform>().transform.localScale += new Vector3(0f, 0f, scalar);

                    
                    note.GetComponent<NoteData>().length += scalar * 10.0;
                }

                //Debug.Log(holdTime);
            }
            scalar = 0;
        }
        
    }

    // in theory, these functions parallel touch input
    private void OnMouseUp()
    {
        clicked = false;

        if (endNote == null && isHeld)
        {
            endNote = Instantiate(originalNote);
            endNote.GetComponent<Transform>().parent = note.GetComponent<Transform>();
            endNote.GetComponent<Transform>().position =
                new Vector3(holdLine.GetComponent<Transform>().position.x,
                            holdLine.GetComponent<Transform>().position.y,
                            holdLine.GetComponent<Transform>().position.z * 2f);

            endNote.GetComponent<MeshRenderer>().material.color = hoverColor;
            endNote.GetComponent<BoxCollider>().enabled = false;

            // beat should be divisible by 0.25 (quarter beat)
            endNote.GetComponent<NoteData>().beat = 
                note.GetComponent<NoteData>().beat + note.GetComponent<NoteData>().length;

            //endNote.GetComponent<NoteData>().length = 0.0;
            //endNote.GetComponent<NoteData>().queueNum = 0;

            Debug.Log("End Note Beat: " + endNote.GetComponent<NoteData>().beat);
        }
        else
        {            
            Destroy(endNote);
        }

        holdTime = 0;
    }

    public void OnMouseDown()
    {
        clicked = true;

        if (note == null)
        {
            // init note setup
            note = Instantiate(originalNote);
            note.GetComponent<Transform>().parent = this.transform;
            note.GetComponent<Transform>().position = this.transform.position;
            note.GetComponent<Transform>().position += new Vector3(0f, 0.5f, 0f);
            note.GetComponent<MeshRenderer>().material.color = hoverColor;
            note.GetComponent<BoxCollider>().enabled = false;
            
            //add the note to the editor dictionary
            editorController.AddNote(noteNum, note.gameObject);

            /** things to pass to NoteSpawner **/
            // note's beat should be set to current position in editor
            note.GetComponent<NoteData>().beat = 0.0;

            // tap notes length default to zero; length increases based on hold time
            note.GetComponent<NoteData>().length = 0.0;

            // queue number
            //note.GetComponent<NoteData>().queueNum = 0;
            Debug.Log(note.GetComponent<NoteData>().queueNum);

            // hold note setup
            holdLine = Instantiate(line);
            holdLine.GetComponent<Transform>().parent = note.GetComponent<Transform>();
            holdLine.GetComponent<Transform>().position = note.GetComponent<Transform>().position;
            holdLine.GetComponent<MeshRenderer>().material.color = Color.white;
            holdLine.GetComponent<BoxCollider>().enabled = false;

            holdLine.SetActive(false);

            
        }
        else
        {
            Destroy(note);
            Destroy(holdLine);
            //adding a null note is the same as removing it from the dict
            editorController.AddNote(noteNum, null);
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
