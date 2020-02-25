using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    private Renderer r;
    private Color pressedColor;
    private Color btnColor;
    public KeyCode key;
    public bool mouseClick;
    public Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        mouseClick = false;
        r = GetComponent<Renderer>();
        btnColor = r.material.color;
        pressedColor = new Color(btnColor.r + 0.2f, btnColor.g + 0.2f, btnColor.b + 0.2f);

        colliders = GetComponents<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            SetPressedBtnColor();
        }
        else if (Input.GetKeyUp(key))
        {
            SetDefaultBtnColor();
        }
    }

    // using MouseDown functions in lieu of touch inputs for now
    private void OnMouseDown()
    {
        mouseClick = true;
        SetPressedBtnColor();
    }

    private void OnMouseUp()
    {
        mouseClick = false;
        SetDefaultBtnColor();
    }

    private void SetPressedBtnColor()
    {
        r.material.color = pressedColor;
    }

    private void SetDefaultBtnColor()
    {
        r.material.color = btnColor;
    }

}
