using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // used for testing button presses
    private Renderer r;
    private Color pressedColor;
    private Color btnColor;

    // input types
    public KeyCode keyPressed;
    public bool mouseClick;

    public NoteSpawner noteQueue;
    public Judgment judge;
    public Metronome metronome;
    public double bpm;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        btnColor = r.material.color;
        pressedColor = new Color(btnColor.r + 0.2f, btnColor.g + 0.2f, btnColor.b + 0.2f);

        mouseClick = false;

        bpm = metronome.tempo;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyPressed))
        {
            SetPressedBtnColor();
        }
        else if (Input.GetKeyUp(keyPressed))
        {
            SetDefaultBtnColor();
        }
    }

    public bool IsKeyDown()
    {
        return Input.GetKeyDown(keyPressed);
    }

    public bool IsKeyUp()
    {
        return Input.GetKeyUp(keyPressed);
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
