using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    /*  This class handles player input of keyboard and/or touch presses.
     *  Also handles the removal of notes if the player has hit within a 
     *  timing window or when the note has moved past the largest window.
     *  
     *  Public Functions:
     *  
     *  GetBeatOnKeyPress()
     *      - Returns a double indicating the beat at which the player pressed  
     *      
     *  GetHitGrade()
     *      - Returns an int indicating the resultant grade player got on her hit
     */

    // used to indicate key presses
    private Renderer r;
    private Color pressedColor;
    private Color btnColor;

    // input types
    public KeyCode keyPressed;
    public bool mouseClick;

    // needed classes
    public NoteSpawner noteSpawner;
    public Judgment judge;
    public Metronome metronome;
    public NoteController noteController;

    private double beatPressed;
    private double nextBeat;
    private static int hitGrade;
    private int queueNum;
    private bool[] notesOnNextBeat;

    // Text for testing
    public Text t1;
    public Text t2;
    public Text t3;
    public Text t4;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        btnColor = r.material.color;
        pressedColor = new Color(btnColor.r + 0.2f, btnColor.g + 0.2f, btnColor.b + 0.2f);

        mouseClick = false;

        if (name == "Btn1") queueNum = 0;
        else if (name == "Btn2") queueNum = 1;
        else if (name == "Btn3") queueNum = 2;
        else if (name == "Btn4") queueNum = 3;
        else queueNum = -1;

        nextBeat = 0;
        notesOnNextBeat = new bool[4];
    }

    // Update is called once per frame
    void Update()
    {
        // look at the top of each queue for the next beat        
        double temp = double.MaxValue;
        for (int i = 0; i < 4; i++)
        {
            if (noteSpawner.notes[i].Count > 0)
            {
                if (noteSpawner.notes[i][0].GetComponent<NoteMovement>().beat < temp)
                {
                    temp = noteSpawner.notes[i][0].GetComponent<NoteMovement>().beat;
                }
            }
        }
        nextBeat = temp;
        t3.text = "NextBeatToPress: " + nextBeat.ToString();

        // mark all notes on nextBeat
        for (int i = 0; i < 4; i++)
        {
            if (noteSpawner.notes[i].Count > 0)
            {
                if (noteSpawner.notes[i][0].GetComponent<NoteMovement>().beat == nextBeat)
                    notesOnNextBeat[i] = true;
                else
                    notesOnNextBeat[i] = false;
            }
        }

        if (IsKeyDown())
        {
            SetPressedBtnColor();
            SetBeatOnKeyPress();

            // the beat that the player pressed on
            t2.text = "BeatOnKeyPress: " + beatPressed.ToString();

            // check if it's the right queue before grading the attempt
            if (notesOnNextBeat[queueNum])
            {
                int hitResult = judge.JudgeTiming(nextBeat);

                if (hitResult > 0)
                {
                    // player hit within valid window, delete top note from queue
                    noteController.RemoveTopNote(queueNum);
                }

                SetHitGrade(hitResult);
            }
        }

        if (IsKeyUp())
        {
            SetDefaultBtnColor();
        }

        // remove those notes who share the beat if they were missed
        if (notesOnNextBeat[queueNum])
        {
            if (judge.CheckMiss(nextBeat))
            {
                noteController.RemoveTopNote(queueNum);
                SetHitGrade(0); 
            }
        }

        t4.text = "HitResult: " + GetHitGrade().ToString();
    }

    public int GetHitGrade()
    {
        return hitGrade;
    }

    private void SetHitGrade(int q)
    {
        hitGrade = q;
    }

    public double GetBeatOnKeyPress()
    {
        return beatPressed;
    }

    private void SetBeatOnKeyPress()
    {
        beatPressed = metronome.beatsElapsed;
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
