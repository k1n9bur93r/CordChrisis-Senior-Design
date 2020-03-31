using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour
{
    private GameObject note;
    private Color original;
    private Color hover;
    private bool clicked;
    private float scalar;

    void Start()
    {
        note = null;
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

                note.GetComponent<Transform>().transform.localPosition += new Vector3(0f, 0f, scalar / 2f);
                note.GetComponent<Transform>().transform.localScale += new Vector3(0f, 0f, scalar);
                
                note.GetComponent<MeshRenderer>().material.color = hover;
                note.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    // in theory, these functions parallel touch input 
    private void OnMouseUp()
    {
        clicked = false;
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
        }
        else
        {
            Destroy(note);
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
