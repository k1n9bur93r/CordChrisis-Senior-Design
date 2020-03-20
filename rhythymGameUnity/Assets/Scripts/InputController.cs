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
    public void Update()
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

        for (int i = 0; i < MAX_KEYS; i++)
        {
            pressedKeys[i] = Input.GetKeyDown(button[i].GetComponent<ButtonAnimator>().keyPressed) ? true : false;
        }

        for (int i = 0; i < MAX_KEYS; i++)
        {
            if (pressedKeys[i])
            {
                if (judge.CheckHit(noteController.GetFirstBeat(i)))
                {
                    noteController.RemoveTopNote(i);
                }
                t2.text = "Beat on press: " + beatPressed.ToString();
            }

            // Animate the buttons (could also be basis for hold note detection?)
            if (Input.GetKey(button[i].GetComponent<ButtonAnimator>().keyPressed))
                button[i].GetComponent<ButtonAnimator>().SetPressedBtnColor();
            else
                button[i].GetComponent<ButtonAnimator>().SetDefaultBtnColor();
        }


        // An equivalent function for swipe notes goes here


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
