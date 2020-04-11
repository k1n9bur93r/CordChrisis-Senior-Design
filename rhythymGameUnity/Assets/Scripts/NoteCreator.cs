using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour
{
    // objects to create
    private GameObject note;
    private GameObject holdLine;
    private GameObject endNote;
    public GameObject line;

    // object info
    private Color original;
    private Color hover;
    private float scalar;
    private double beat;
    private double length;
    private int queueNum;

    // input info
    private bool clicked;
    private bool isHeld;
    private float holdTime;

    void Start()
    { 
        note = holdLine = endNote = null;

        original = GetComponent<MeshRenderer>().material.color;
        hover = original;
        hover.a = 1;

        clicked = false;
        scalar = holdTime = 0;
    }

    void Update()
    {
        if (clicked && note != null)
        {
            if (Input.GetMouseButton(0))
            {
                // should have a max size for a hold note
                scalar += 0.0005f;
                holdTime += Time.deltaTime;

                isHeld = holdTime > 0.25f ? true : false;
                if (isHeld)
                {
                    holdLine.SetActive(true);
                    holdLine.GetComponent<Transform>().transform.localPosition += new Vector3(0f, 0f, scalar / 2.0f);
                    holdLine.GetComponent<Transform>().transform.localScale += new Vector3(0f, 0f, scalar);
                    note.GetComponent<NoteCreator>().length += scalar;
                }

                Debug.Log(note.GetComponent<NoteCreator>().length);
                //Debug.Log(holdTime);
            }
        }
    }

    // in theory, these functions parallel touch input
    private void OnMouseUp()
    {
        clicked = false;

        if (endNote == null && isHeld)
        {
            endNote = Instantiate(gameObject);
            endNote.GetComponent<Transform>().position =
                new Vector3(holdLine.GetComponent<Transform>().position.x,
                            holdLine.GetComponent<Transform>().position.y,
                            holdLine.GetComponent<Transform>().position.z * 2f);

            endNote.GetComponent<MeshRenderer>().material.color = hover;
            endNote.GetComponent<BoxCollider>().enabled = false;
            endNote.GetComponent<NoteCreator>().beat = 0.0;
            endNote.GetComponent<NoteCreator>().length = 0.0;
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
            note = Instantiate(gameObject);
            note.GetComponent<Transform>().position += new Vector3(0f, 1f, 0f);
            note.GetComponent<MeshRenderer>().material.color = hover;
            note.GetComponent<BoxCollider>().enabled = false;

            // beat must be set based on current position in editor
            note.GetComponent<NoteCreator>().beat = 0.0;

            // tap notes length default to zero;
            note.GetComponent<NoteCreator>().length = 0.0;

            // queue number
            note.GetComponent<NoteCreator>().queueNum = 0;

            holdLine = Instantiate(line);
            holdLine.GetComponent<Transform>().position = note.GetComponent<Transform>().position;
            holdLine.SetActive(false);
        }
        else
        {
            Destroy(note);
            Destroy(holdLine);
        }
    }

    private void CreateNote(int qnum, double beat, double length)
    {

    }

    private void OnMouseEnter()
    {
        GetComponent<MeshRenderer>().material.color = hover;
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.color = original;
    }
}
