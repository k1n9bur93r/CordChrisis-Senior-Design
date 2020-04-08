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

    // input info
    private bool clicked;
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
                scalar += 0.0001f;
                holdTime += Time.deltaTime;

                if (holdTime > 0.2f)
                {
                    holdLine.GetComponent<Transform>().transform.localPosition += new Vector3(0f, 0f, scalar / 2f);
                    holdLine.GetComponent<Transform>().transform.localScale += new Vector3(0f, 0f, scalar);
                }

                note.GetComponent<MeshRenderer>().material.color = hover;
                note.GetComponent<BoxCollider>().enabled = false;
                Debug.Log(holdTime);
            }
        }
    }

    // in theory, these functions parallel touch input
    private void OnMouseUp()
    {
        clicked = false;

        if (endNote == null && holdTime > 0.2f)
        {
            endNote = Instantiate(gameObject);
            endNote.GetComponent<Transform>().position =
                new Vector3(holdLine.GetComponent<Transform>().position.x,
                            holdLine.GetComponent<Transform>().position.y,
                            holdLine.GetComponent<Transform>().position.z * 2f);

            endNote.GetComponent<MeshRenderer>().material.color = hover;
            endNote.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            Destroy(endNote);
        }

        holdTime = 0;
    }

    private void OnMouseDown()
    {
        clicked = true;
        if (note == null)
        {
            note = Instantiate(gameObject);
            note.GetComponent<Transform>().position += new Vector3(0f, 1f, 0f);
            note.GetComponent<MeshRenderer>().material.color = hover;
            note.GetComponent<BoxCollider>().enabled = false;
            
            holdLine = Instantiate(line);
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
