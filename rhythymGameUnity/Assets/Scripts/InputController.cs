using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    /*  This class handles player input of keyboard and/or touch presses.
     *  
     *  Key Functions:
     *  
     *  BeatOnKeyPress()
     *      - Returns a double indicating the beat that the player has pressed  
     *      
     *  DeleteCurrNote()
     *      - Deletes the note block at the top of the queue if the player
     *      has pressed within a valid timing window
     *      
     *  IsNoteOutOfRange()
     *      - Returns a boolean indicating if the note block's position has 
     *      moved passed the designated receptor
     */

    // used to indicate key presses
    private Renderer r;
    private Color pressedColor;
    private Color btnColor;

    // input types
    public KeyCode keyPressed;
    public bool mouseClick;

    public NoteSpawner noteSpawner;
    public Judgment judge;
    public Metronome metronome;
    private double bpm;
    private double beatPressed;
    private double noteBeat;
    private List<GameObject>[] noteQueues = new List<GameObject>[4];
    
    public Text t1;
    public Text t2;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        btnColor = r.material.color;
        pressedColor = new Color(btnColor.r + 0.2f, btnColor.g + 0.2f, btnColor.b + 0.2f);

        mouseClick = false;

        bpm = metronome.tempo;
        noteQueues = noteSpawner.notes;
        
        t1.text = "Key: ";
        t2.text = "Beat: ";
    }

    // Update is called once per frame
    void Update()
    {
        if (IsKeyDown())
        {
            SetPressedBtnColor();
            t2.text = "Beat: " + BeatOnKeyPress().ToString();



            int beatToCheck = judge.JudgeTiming(beatPressed);
            if (beatToCheck > 0)
            {
                // player hit within valid window, delete top note from queue
                //DeleteCurrNote();
            }
            
            if (beatToCheck == 0)
            {
                // player tried to hit too early (before "good" window)
                // don't delete top note in queue
                
            }
        }

        if (IsKeyUp())
        {
            SetDefaultBtnColor();
        }
    }

    public double BeatOnKeyPress()
    {
        beatPressed = metronome.beatsElapsed;
        return beatPressed;
    }

    private void DeleteCurrNote()
    {

        noteQueues[0].RemoveAt(0);
        noteQueues[1].RemoveAt(0);
        noteQueues[2].RemoveAt(0);
        noteQueues[3].RemoveAt(0);
    }

    private bool IsNoteOutOfRange()
    {
        return false;
    }

    private bool IsKeyDown()
    {
        return Input.GetKeyDown(keyPressed);
    }

    private bool IsKeyUp()
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
        t1.text = "Key: " + keyPressed.ToString();
        r.material.color = pressedColor;
    }

    private void SetDefaultBtnColor()
    {
        r.material.color = btnColor;
    }

}
