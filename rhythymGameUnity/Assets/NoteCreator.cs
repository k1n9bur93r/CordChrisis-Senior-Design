using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour
{
    private GameObject note;
    private GameObject holdLine;
    private GameObject endNote;

    public GameObject line;
    private Color original;
    private Color hover;
    private bool clicked;
    private float scalar;

    void Start()
    { 
        note = holdLine = endNote = null;
        original = GetComponent<MeshRenderer>().material.color;
        hover = original;
        hover.a = 1;
        clicked = false;
        scalar = 0;
    }

    void Update()
    {
        if (clicked && note != null)
        {
            if (Input.GetMouseButton(0))
            {
                // should have a max size for a hold note
                scalar += 0.0001f;

                holdLine.GetComponent<Transform>().transform.localPosition += new Vector3(0f, 0f, scalar / 2f);
                holdLine.GetComponent<Transform>().transform.localScale += new Vector3(0f, 0f, scalar);

                note.GetComponent<MeshRenderer>().material.color = hover;
                note.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    // in theory, these functions parallel touch input 
    private void OnMouseUp()
    {
        clicked = false;
        if (endNote == null)
        {
            endNote = Instantiate(gameObject);
            endNote.GetComponent<Transform>().position =
                new Vector3(holdLine.GetComponent<Transform>().position.x,
                            holdLine.GetComponent<Transform>().position.y,
                            holdLine.GetComponent<Transform>().position.z * 2f);
        }
        else
        {
            Destroy(endNote);
        }
    }

    private void OnMouseDown()
    {
        clicked = true;
        if (note == null)
        {
            note = Instantiate(gameObject);
            holdLine = Instantiate(line);
            
            note.GetComponent<Transform>().position += new Vector3(0f, 1f, 0f);
            note.GetComponent<MeshRenderer>().material.color = hover;
            note.GetComponent<BoxCollider>().enabled = false;

            holdLine.GetComponent<Transform>().position = note.GetComponent<Transform>().position;
        }
        else
        {
            Destroy(note);
            Destroy(holdLine);
        }
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
