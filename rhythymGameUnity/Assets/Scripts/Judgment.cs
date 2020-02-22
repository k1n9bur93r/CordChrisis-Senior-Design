using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Judgment class

	The receptor versus note hit detection system.
	Compares the time of the user's input (in beats) versus the time of the note in question (in beats).
	The difference between these two elements is used to rate the user's timing.

	This class currently has no functions usable by other classes besides Metronome yet.
	
	Important public variables:
		- None yet, maybe

	Important public methods:
		- None yet
*/

public class Judgment : MonoBehaviour
{
	public Metronome master;

	private const double framesMarvelous = 1.0 / 60.0; // 1/60th of a second early/late
	private const double framesPerfect = 2.0 / 60.0; // 2/60ths
	private const double framesGreat = 5.0 / 60.0; // 5/60ths
	private const double framesGood = 10.0 / 60.0; // 10/60ths

	private double beatsMarvelous;
	private double beatsPerfect;
	private double beatsGreat;
	private double beatsGood;

	void Start()
	{
		CalculateWindows();
	}

	void Update()
	{
		CalculateWindows();
	}

	/*
		Determine how many beats are in each timing window.
	*/

	private void CalculateWindows()
	{
		beatsMarvelous = master.beatsPerSec * framesMarvelous;
		beatsPerfect = master.beatsPerSec * framesPerfect;
		beatsGreat = master.beatsPerSec * framesGreat;
		beatsGood = master.beatsPerSec * framesGood;
	}

	/*
		This function will eventually rate the player's input timing versus incoming notes.
	*/

	void JudgeTiming()
	{
		// ...
	}

	/*
		FOR REFERENCE ONLY: Compare timing of a key press versus the metronome's 8th beat.
	*/

	/*
	private void JudgeTimingDebug()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			double currentBeat = master.beatsElapsed; // Compensate for song start offset and dsp stuff later
			double noteBeat = 8.0; // Placeholder until note-reading process is figured out

			float diff = (float)(currentBeat - noteBeat);

			if (diff < -beatsGood) //(diff < -framesGood)
			{
				judgmentText.text = "Way too early!";
			}

			else
			{
				if (Mathf.Abs(diff) <= beatsMarvelous) { judgmentText.text = "Fantastic timing!"; }
				else if (Mathf.Abs(diff) <= beatsPerfect) { judgmentText.text = "Excellent timing!"; }
				else if (Mathf.Abs(diff) <= beatsGreat) { judgmentText.text = "Great timing!"; }
				else if (Mathf.Abs(diff) <= beatsGood) { judgmentText.text = "Good timing!"; }
				else { judgmentText.text = "Way too late!"; }
			}

			judgmentText.text = 
				judgmentText.text + "\n" + diff + " beats\n"
				+ (diff * (float)master.secPerBeat) + " sec";

			//Debug.Log("Spacebar pressed at: " + master.timeElapsed + " sec, " + master.beatsElapsed + " beats");
		}
	}
	*/
}