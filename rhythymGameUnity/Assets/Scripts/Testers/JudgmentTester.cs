using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> JudgmentTester class

	NOTICE: This class represents an outdated version of Judgment! Please don't reference it!

	Test version of the hit detection system. For use in the Metronome Tester scene only.
	Compares the time of the user's input (in beats) versus the 16th beat ticked by the metronome.
*/

public class JudgmentTester : MonoBehaviour
{
	public Metronome master;
	public Text judgmentText;
	public Text judgmentHelpText;

	// Timing window measurements are on the assumption that that the game runs at 60 FPS. Measurements are made in fractions of a second (ie: 1.0/60.0 = 1/60th of a second)
	private const double framesMarvelous = 1.0 / 60.0;
	private const double framesPerfect = 2.0 / 60.0;
	private const double framesGreat = 5.0 / 60.0;
	private const double framesGood = 12.0 / 60.0;

	private double beatsMarvelous;
	private double beatsPerfect;
	private double beatsGreat;
	private double beatsGood;

	void Start()
	{
		judgmentText.text = "Press Z to start the music! Then press SPACE on beat 16.0!";
		CalculateWindows();
	}

	void Update()
	{
		CalculateWindows();
		JudgeTimingDebug();
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

		//master.UpdateRates();
		UpdateHelpText(); // DEBUG
	}

	/*
		DEBUG: Update the help information
	*/

	private void UpdateHelpText()
	{
		judgmentHelpText.text =
			"Marvelous: +/- " + (float)beatsMarvelous + " beats (+/- " + (float)(framesMarvelous) + " sec)\n"
			+ "Excellent: +/- " + (float)beatsPerfect + " beats (+/- " + (float)(framesPerfect) + " sec)\n"
			+ "Great: +/- " + (float)beatsGreat + " beats (+/- " + (float)(framesGreat) + " sec)\n"
			+ "Good: +/- " + (float)beatsGood + " beats (+/- " + (float)(framesGood) + " sec)\n"
			+ "Attempting to hit a note beyond the [Good] window is either not counted (early) or missed (late)";
	}

	/*
		DEBUG: compare timing of a key press versus the metronome's ticker.
	*/

	private void JudgeTimingDebug()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			double currentBeat = master.beatsElapsed;
			double noteBeat = 16.0; // Placeholder until note-reading process is figured out

			float diff = (float)(currentBeat - noteBeat);

			if (diff < -beatsGood) //(diff < -framesGood)
			{
				judgmentText.text = "Way too early!";
			}

			else
			{
				if (Mathf.Abs(diff) <= beatsMarvelous) { judgmentText.text = "Marvelous timing!"; }
				else if (Mathf.Abs(diff) <= beatsPerfect) { judgmentText.text = "Excellent timing!"; }
				else if (Mathf.Abs(diff) <= beatsGreat) { judgmentText.text = "Great timing!"; }
				else if (Mathf.Abs(diff) <= beatsGood) { judgmentText.text = "Good timing!"; }
				else { judgmentText.text = "Way too late!"; }
			}

			/*
			judgmentText.text = 
				judgmentText.text + "\n" + diff + " beats\n"
				+ (diff * (float)master.secPerBeat) + " sec";
			*/

			//Debug.Log("Spacebar pressed at: " + master.timeElapsed + " sec, " + master.beatsElapsed + " beats");
		}
	}
}