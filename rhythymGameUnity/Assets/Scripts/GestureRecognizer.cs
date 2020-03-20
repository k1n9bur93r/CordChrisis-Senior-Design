using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureRecognizer : MonoBehaviour
{
    private Vector3 start_position = Vector3.zero;
    private bool has_swiped = false;

    private float abs(float x) {return (x < 0) ? -x : x;}

    void OnMouseDown()
    {
        start_position = Input.mousePosition;
        // Debug.Log(Input.mousePosition);
    }

    void OnMouseUp()
    {
        has_swiped = true;
    }

    public string IsSwipe()
    {
        // only send each swipe once
        // swipe is ended on mouse up
        if (!has_swiped) {
            return "";
        }
        has_swiped = false;

        Vector3 delta_position = Input.mousePosition - start_position;
        if (abs(delta_position.x) > abs(delta_position.y))
        {
            if (delta_position.x > 0)
            {
                return "right";
            }
            else
            {
                return "left";
            }
        }
        else
        {
            if (delta_position.y > 0)
            {
                return "up";
            }
            else
            {
                return "down";
            }
        }
    }
}
