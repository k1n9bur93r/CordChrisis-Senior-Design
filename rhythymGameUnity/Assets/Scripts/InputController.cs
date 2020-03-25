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
    private const int MAX_GESTURES = 4;

    // needed classes
    public NoteSpawner noteSpawner;
    public Judgment judge;
    public Metronome metronome;
    public NoteController noteController;
    public GameObject[] button;
    public GestureRecognizer gestureRecognizer;

    private double beatPressed;
    private double[] lengthRemain;

    // Text for testing
    public Text t1;
    public Text t2;
    public Text t3;
    public Text t4;

    // Splash flag
    private bool[] splashFlag = new bool[4];

    public int SplashDensity = 15;
    // Array of receptor splash
    public ParticleSystem[] receptorSplash = new ParticleSystem[4];

    void Awake()
    {
        lengthRemain = new double[] { 0.0, 0.0, 0.0, 0.0 };
    }

    void Start()
    {
        // Set all splash flag falses
        for (int i = 0; i < MAX_KEYS; i++)
            splashFlag[i] = true;
        // ...
    }

    public void Update()
    {
        t1.text = "Beat: " + metronome.beatsElapsed.ToString();
        t3.text = "Tempo: " + metronome.tempo.ToString();
        SetBeatOnKeyPress();

        // An equivalent function for swipe notes goes here
        for (int i = 0; i < MAX_GESTURES; i++) {
            if (judge.CheckMiss(noteController.GetFirstGesture(i), noteController.GetSecondGesture(i)))
            {
                noteController.RemoveTopGesture(i);
            }
        }
        
        for (int i = 0; i < MAX_KEYS; i++)
        {
            double noteLength;

            // Check if the notes at this beat should be deleted and marked as missed
            if (judge.CheckMiss(noteController.GetFirstBeat(i), noteController.GetSecondBeat(i)))// && (lengthRemain[i] <= 0))
            {
                //check if it was the beginning of hold note
                if (noteSpawner.notes[i][0].GetComponent<HoldNoteLine>().secondNote != null)
                {
                    //if so, then delete the end of the hold note as well
                    noteSpawner.notes[i][0].GetComponent<HoldNoteLine>().secondNote.SetActive(false);
                    noteSpawner.notes[i].Remove(noteSpawner.notes[i][0].GetComponent<HoldNoteLine>().secondNote);
                }
                noteController.RemoveTopNote(i);
                noteLength = 0;
                //noteController.SetNoteLength(i, noteLength); // !
                lengthRemain[i] = 0.0;
            }

            else
            {
                noteLength = noteController.GetNoteLength(i);
            }

            // Process taps
            if (Input.GetKeyDown(button[i].GetComponent<ButtonAnimator>().btnKey))
            {
                if (judge.CheckHit(noteController.GetFirstBeat(i)))
                {
                    lengthRemain[i] = noteLength;
                    // Trigger the splash
                    splashFlag[i] = true;
                    // Change note length to compensate for timing
                    if (noteLength > 0)
                    {
                        lengthRemain[i] = judge.ReduceHoldInitial(lengthRemain[i], noteController.GetFirstBeat(i));
                    }

                    noteController.RemoveTopNote(i);
                }

                t2.text = "Beat on press: " + beatPressed.ToString();
            }

            // Process taps length > 0
            if (Input.GetKey(button[i].GetComponent<ButtonAnimator>().btnKey))
            {
                if (lengthRemain[i] > 0.0)
                {
                    // Trigger the splash
                    splashFlag[i] = true;
                    lengthRemain[i] = judge.ReduceHoldDuring(lengthRemain[i]);
                    //noteController.SetNoteLength(i, lengthRemain[i]); // !

                    t4.text = "Current note length: " + lengthRemain[i].ToString();

                    if (lengthRemain[i] <= 0.0)
                    {                        
                        //noteController.RemoveTopNote(i);
                        judge.HoldSuccess();
                    }
                }
                button[i].GetComponent<ButtonAnimator>().SetPressedBtnColor();
            }

            else
            {
                if (lengthRemain[i] > 0.0)
                {
                    lengthRemain[i] = 0.0;
                    //noteController.RemoveTopNote(i);
                    //noteController.SetNoteLength(i, lengthRemain[i]); // !
                    judge.HoldFailure();
                    //if hold failed then remove visual corresponding hold
                    noteSpawner.holds[0].SetActive(false);
                    noteSpawner.holds.RemoveAt(0);
                }
                button[i].GetComponent<ButtonAnimator>().SetDefaultBtnColor();
            }
        }

        // An equivalent function for swipe notes goes here
        void processGesture(int gesture) {
            if (judge.CheckSwipe(noteController.GetFirstGesture(gesture))) {
                // All receptors splash
                for (int i = 0; i < MAX_KEYS; i++)
                    splashFlag[i] = true;
                noteController.RemoveTopGesture(gesture);
            }
        }

        string swipe = gestureRecognizer.IsSwipe();
        switch (swipe) {
            case "up":
                processGesture(0);
                break;
            case "right":
                processGesture(1);
                break;
            case "down":
                processGesture(2);
                break;
            case "left":
                processGesture(3);
                break;
            default:
                break;
        }

        // Trigger the splash effects
        for(int i = 0; i < MAX_KEYS; i++)
        {
            if(splashFlag[i])
            {
                receptorSplash[i].Emit(SplashDensity);
                splashFlag[i] = false;
            }
            /*
            // Debugging purpose to see if the particle are emitting
            if (splashFlag[i])
            {
                receptorSplash[i].Emit(15);
            }
            */
        }

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
