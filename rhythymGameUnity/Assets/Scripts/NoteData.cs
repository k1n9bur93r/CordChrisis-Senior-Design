using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // info created based on NoteCreator
    public double beat;
    public double length;
    public int queueNum;

    public GameObject holdLine;
    public GameObject endNote;

    public void OnBeginDrag(PointerEventData ped)
    {

    }

    public void OnDrag(PointerEventData ped)
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = ped.pressEventCamera.ScreenPointToRay(ped.position);
        Vector2 mouseDelta = ped.delta;

        float distance;

        if (plane.Raycast(ray, out distance))
        {
            float scalar = 0.0025f;

            if (gameObject != null)
            {
                holdLine.SetActive(true);
                endNote.SetActive(true);
                Transform parent = gameObject.transform.parent;

                if (mouseDelta.y > 0)
                {
                    holdLine.GetComponent<Transform>().transform.localPosition += new Vector3(0f, 0f, scalar / 2.0f);
                    holdLine.GetComponent<Transform>().transform.localScale += new Vector3(0f, 0f, scalar);
                    endNote.GetComponent<Transform>().position = 
                        new Vector3(holdLine.GetComponent<Transform>().position.x,
                                    holdLine.GetComponent<Transform>().position.y,
                                    holdLine.GetComponent<Transform>().position.z * 2f);
                    parent.GetComponent<NoteData>().length += scalar * 10.0;
                }
                else if (mouseDelta.y == 0)
                {
                    holdLine.GetComponent<Transform>().transform.localPosition += Vector3.zero;
                    holdLine.GetComponent<Transform>().transform.localScale += Vector3.zero;
                    endNote.GetComponent<Transform>().transform.position += Vector3.zero;
                    parent.GetComponent<NoteData>().length += 0;

                }
                else
                {
                    holdLine.GetComponent<Transform>().transform.localPosition -= new Vector3(0f, 0f, scalar / 2.0f);
                    holdLine.GetComponent<Transform>().transform.localScale -= new Vector3(0f, 0f, scalar);
                    endNote.GetComponent<Transform>().position =
                        new Vector3(holdLine.GetComponent<Transform>().position.x,
                                    holdLine.GetComponent<Transform>().position.y,
                                    holdLine.GetComponent<Transform>().position.z * 2f);
                    parent.GetComponent<NoteData>().length -= scalar * 10.0;
                }
            }
            //Debug.Log(GetComponent<BoxCollider>().size);
        }
    }

    public void OnEndDrag(PointerEventData ped)
    {

    }
}
