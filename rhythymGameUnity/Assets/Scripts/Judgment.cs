using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	JUDGMENT

	The receptor versus note hit detection system.
	Compares the time of the user's input (in beats) versus the time of the note in question (in beats).
	The difference between these two elements is used to rate the user's timing.

	While the amount of time required for each timing window is constant, the function actually checks how many BEATS fall within each timing window.
	This can be demonstrated by changing the tempo in the Metronome Test scene.

	At the moment, this class is cluttered with testing tools specific to the Metronome Test scene, so I wouldn't use anything from here yet.
	
	Important public variables:
		- None yet, maybe

	Important public methods:
		[NOT USABLE YET] AccRating JudgeTiming(<name of note class>)
*/

public class Judgment : MonoBehaviour
{
	public Metronome master;
	public Text judgmentText;
	public Text judgmentHelpText;

	private const double framesMarvelous = 1.0 / 60.0; // 1/60th of a second early/late
	private const double framesPerfect = 2.0 / 60.0; // 2/60ths
	private const double framesGreat = 5.0 / 60.0; // 5/60ths
	private const double framesGood = 10.0 / 60.0; // 10/60ths

	double beatsMarvelous;
	double beatsPerfect;
	double beatsGreat;
	double beatsGood;

	void Start()
	{
		judgmentText.text = "Press SPACE on beat 8.0!";
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

	void CalculateWindows()
	{
		beatsMarvelous = master.beatsPerSec * framesMarvelous;
		beatsPerfect = master.beatsPerSec * framesPerfect;
		beatsGreat = master.beatsPerSec * framesGreat;
		beatsGood = master.beatsPerSec * framesGood;

		UpdateHelpText();
	}

	/*
		Update the help information
	*/

	void UpdateHelpText()
	{
		judgmentHelpText.text =
			"Fantastic: +/- " + (float)beatsMarvelous + " beats (+/- " + (float)(framesMarvelous) + " sec)\n"
			+ "Excellent: +/- " + (float)beatsPerfect + " beats (+/- " + (float)(framesPerfect) + " sec)\n"
			+ "Great: +/- " + (float)beatsGreat + " beats (+/- " + (float)(framesGreat) + " sec)\n"
			+ "Good: +/- " + (float)beatsGood + " beats (+/- " + (float)(framesGood) + " sec)\n"
			+ "Attempting to hit a note beyond the [Good] window is either not counted (early) or missed (late)";
	}

	/*
		Debug function to compare timing of a key press versus the metronome's ticker.
	*/

	void JudgeTimingDebug()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			double currentBeat = Time.time / master.secPerBeat; // Compensate for song start offset and dsp stuff later
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
		}
	}

	/*
		Compares timing of the current note(s?) versus the time of the player's input.
		This function can't be used until the note's actual class exists.
		Eventually, make it return what judgment came through, probably using an enum.
	*/

	/*
	enum AccRating { Marvelous, Perfect, Great, Good, Miss, None }; // Put in some global object somewhere? Maybe in Metronome?

	AccRating JudgeTimingDebug(<name of note type> currentNote)
	{
		double currentBeat = Time.time / master.secPerBeat; // Compensate for song start offset and dsp stuff later
		double noteBeat = 8.0; // Placeholder until note-reading process is figured out

		AccRating rating;
		float diff = (float)(currentBeat - noteBeat);

		if (!(diff < -beatsGood))
		{
			if (Mathf.Abs(diff) <= beatsMarvelous) { rating = AccRating.Marvelous; }
			else if (Mathf.Abs(diff) <= beatsPerfect) { rating = AccRating.Perfect; }
			else if (Mathf.Abs(diff) <= beatsGreat) { rating = AccRating.Great; }
			else if (Mathf.Abs(diff) <= beatsGood) { rating = AccRating.Good; }
			else { judgmentText.text = "Way too late!"; } // Not sure how to handle misses yet
		}

		else
		{
			rating = AccRating.None;
		}
				
		return rating; // C# scoping can't handle this?!
	}
	*/
}