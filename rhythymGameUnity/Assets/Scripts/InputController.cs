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
     *  GetBeatOnKeyPress()
     *      - Returns a double indicating the beat at which the player pressed  
     *      
     *  RemoveNote()
     *      - Deletes the note block at the top of the queue if the player
     *      has pressed within a valid timing window
     *      
     *  NotIsOutOfRange()
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
    public double nextBeat;

    // Text for testing
    public Text t1;
    public Text t2;
    public Text t3;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        btnColor = r.material.color;
        pressedColor = new Color(btnColor.r + 0.2f, btnColor.g + 0.2f, btnColor.b + 0.2f);

        mouseClick = false;

        bpm = metronome.tempo;
        nextBeat = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // looking for the next beat
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < noteSpawner.notes[i].Count; j++)
                if (noteSpawner.notes[i][j].GetComponent<NoteMovement>().beat < metronome.beatsElapsed)
                {
                    nextBeat = noteSpawner.notes[i][j].GetComponent<NoteMovement>().beat;
                }
        }

        t3.text = " NextBeatToPress: " + nextBeat.ToString();

        if (IsKeyDown())
        {
            SetPressedBtnColor();
            SetBeatOnKeyPress();

            // the beat that the player pressed on
            t2.text = "BeatOnKeyPress: " + GetBeatOnKeyPress().ToString();
                       
            int beatToCheck = judge.JudgeTiming(beatPressed);
            if (beatToCheck > 0)
            {
                // player hit within valid window, delete top note from queue
                //DeleteCurrNote();
            }
            
            if (beatToCheck == 0)
            {
                // player tried to hit too early (before "good" window)
                // don't delete
                
            }
        }

        if (IsKeyUp())
        {
            SetDefaultBtnColor();
        }

/*        for (int i = 0; i < 4; i++)
        {
            if (NoteIsOutOfRange(i))
                RemoveTopNote(i);
        }
*/
    }

    public double GetBeatOnKeyPress()
    {
        return beatPressed;
    }

    public void SetBeatOnKeyPress()
    {
        beatPressed = metronome.beatsElapsed;
    }

    /*
    private void RemoveTopNote(int queueNum)
    {
        noteSpawner.notes[queueNum][0].SetActive(false);

        if (!(noteSpawner.notes[queueNum].Count < 0)) 
            noteSpawner.notes[queueNum].RemoveAt(0);
    }

    private bool NoteIsOutOfRange(int queueNum)
    {
        if (noteSpawner.notes[queueNum] != null)
            return (noteSpawner.notes[queueNum][0].GetComponent<NoteMovement>().transform.position.z < 0) ? true : false;
        else return false;
    }
    */

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
