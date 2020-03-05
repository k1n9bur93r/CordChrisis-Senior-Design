using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Judgment class

	The timing aspect of the hit detection system.
	Compares the time of the user's input (in beats) versus the time of the note in question (in beats).
	The difference between these two elements is used to rate the user's timing.

	Important public methods:
		- bool JudgeTiming(): For use by InputController. Recieves the beat of a pressed note from the top of the note queue when a key is pressed, and returns a value based on timing.
		- bool CheckMiss(): For use by InputController. Recieves the beat of an unpressed note from the top of the queue. Returns whether or not the note has been missed completely.
*/

public class Judgment : MonoBehaviour
{
	public Metronome master;

	private const double framesMarvelous = 1.0 / 60.0; // +/-16.7ms
	private const double framesPerfect = 2.0 / 60.0; // +/-33.3ms
	private const double framesGreat = 6.0 / 60.0; // +/-100.0ms
	private const double framesGood = 12.0 / 60.0; // +/-200.0ms

	private double beatsMarvelous;
	private double beatsPerfect;
	private double beatsGreat;
	private double beatsGood;

	void Start()
	{
		//CalculateWindows();
	}

	void Update()
	{
		//CalculateWindows();
		//PrintWindows();
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
		When a key is pressed, judge the player's timing by checking the beat of the note sent from the queue versus the current beat.

		Returns a number to pass back to InputController:
			- 1 to 4: The note was hit, delete it from the queue, score accordingly
			- 0: The note was hit too early, don't delete from the queue
	*/

	public int JudgeTiming(double receivedBeat)
	{
		CalculateWindows();

		double currentBeat = master.beatsElapsed; //master.beatsElapsedOld;
		double noteBeat = receivedBeat;
		double diff = currentBeat - noteBeat;

		// ---

		// Check if the player hits at least the early "Good" window
		if (diff >= -beatsGood)
		{
			if (Math.Abs(diff) <= beatsMarvelous)
			{
				return 4;
			}

			else if (Math.Abs(diff) <= beatsPerfect)
			{
				JudgeLean(diff);
				return 3;
			}
			else if (Math.Abs(diff) <= beatsGreat)
			{
				JudgeLean(diff);
				return 3;
			}

			else if (Math.Abs(diff) <= beatsGood)
			{
				JudgeLean(diff);
				return 3;
			}
			
			else
			{
				Debug.Log("ERROR: JudgeTiming() fell through!");
				return 0;
			}
		}

		// The player tried to hit a note before it passed the early "Good" window
		else
		{
			return 0;
		}

		// It should not be possible to hit beyond the late "Good" window, because the note should be deleted from the queue by now
	}

	/*
		Check if the non-best input hit the early or late side of the timing window.
	*/

	private void JudgeLean(double diff)
	{
		if (diff < 0.0) { ; } // Something happens
		else if (diff > 0.0) { ; } // Something happens
	}

	/*
		Check if the note at the top of the queue has gone unpressed for too long.
		Returns true if its beat exceeds the current threshold of the "Miss" window (which is actually just the area beyond the late "Good" window).
	*/

	public bool CheckMiss(double receivedBeat)
	{
		CalculateWindows();

		double currentBeat = master.beatsElapsed;
		double noteBeat = receivedBeat;
		double diff = currentBeat - noteBeat;

		 // If the beat difference between now and the note's location exceeds the size of the late "Good" window, it's now too late to hit, delete from the queue
		if (diff > beatsGood) { return true; } //{ missCount++; Debug.Log("Miss! (" + missCount + ")"); return true; }

		// Otherwise, do not delete from the queue
		else { return false; }
	}

	/*
		Debug function to check the size of the timing windows.
	*/

	private void PrintWindows()
	{
		string judgmentWindows =
			"Marvelous: +/- " + beatsMarvelous + " beats (+/- " + framesMarvelous + " sec)\n"
			+ "Excellent: +/- " + beatsPerfect + " beats (+/- " + framesPerfect + " sec)\n"
			+ "Great: +/- " + beatsGreat + " beats (+/- " + framesGreat + " sec)\n"
			+ "Good: +/- " + beatsGood + " beats (+/- " + framesGood + " sec)";

		Debug.Log(judgmentWindows);
	}
}