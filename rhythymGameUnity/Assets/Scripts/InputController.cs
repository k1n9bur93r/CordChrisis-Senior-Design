using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direction { None, Up, Down, Left, Right };

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
     */

    private const int MAX_KEYS = 4;

    // input types
    public KeyCode keyPressed;

    // needed classes
    public NoteSpawner noteSpawner;
    public Judgment judge;
    public Metronome metronome;
    public NoteController noteController;
    public GameObject[] button;

    private double beatPressed;

    // Text for testing
    public Text t1;
    public Text t2;
    public Text t3;

    // Start is called before the first frame update
    void Start()
    {
        // ...
    }

    // Update is called once per frame
    public void Action() //Update()
    {
        t1.text = "Current beat: " + metronome.beatsElapsed.ToString();
        t3.text = "Tempo: " + metronome.tempo.ToString();

        // Check if the notes at this beat should be deleted and marked as missed
        
        for (int i = 0; i < MAX_KEYS; i++)
        {
            if (judge.CheckMiss(noteController.GetFirstBeat(i), noteController.GetSecondBeat(i)))
            {
                noteController.RemoveTopNote(i);
            }
        }

        // An equivalent function for swipe notes goes here

        // Process tap notes

        SetBeatOnKeyPress();

        bool[] pressedKeys = { false, false, false, false };

        if (Input.GetKeyDown(KeyCode.A)) { pressedKeys[0] = true; button[0].GetComponent<ButtonAnimator>().SetPressedBtnColor(); }
        if (Input.GetKeyDown(KeyCode.S)) { pressedKeys[1] = true; button[1].GetComponent<ButtonAnimator>().SetPressedBtnColor(); }
        if (Input.GetKeyDown(KeyCode.D)) { pressedKeys[2] = true; button[2].GetComponent<ButtonAnimator>().SetPressedBtnColor(); }
        if (Input.GetKeyDown(KeyCode.F)) { pressedKeys[3] = true; button[3].GetComponent<ButtonAnimator>().SetPressedBtnColor(); }

        for (int i = 0; i < MAX_KEYS; i++)
        {
            if (pressedKeys[i] == true)
            {
                if (judge.CheckHit(noteController.GetFirstBeat(i)))
                {
                    noteController.RemoveTopNote(i);
                }

                t2.text = "Beat on press: " + beatPressed.ToString();
            }
        }

        // An equivalent function for swipe notes goes here

        // Animate the buttons (could also be basis for hold note detection?)

        if (Input.GetKey(KeyCode.A)) { button[0].GetComponent<ButtonAnimator>().SetPressedBtnColor(); }
        else { button[0].GetComponent<ButtonAnimator>().SetDefaultBtnColor(); }

        if (Input.GetKey(KeyCode.S)) { button[1].GetComponent<ButtonAnimator>().SetPressedBtnColor(); }
        else { button[1].GetComponent<ButtonAnimator>().SetDefaultBtnColor(); }

        if (Input.GetKey(KeyCode.D)) { button[2].GetComponent<ButtonAnimator>().SetPressedBtnColor(); }
        else { button[2].GetComponent<ButtonAnimator>().SetDefaultBtnColor(); }

        if (Input.GetKey(KeyCode.F)) { button[3].GetComponent<ButtonAnimator>().SetPressedBtnColor(); }
        else { button[3].GetComponent<ButtonAnimator>().SetDefaultBtnColor(); }
    }

    public double GetBeatOnKeyPress()
    {
        return beatPressed;
    }

    private void SetBeatOnKeyPress()
    {
        beatPressed = metronome.beatsElapsed;
    }
}
