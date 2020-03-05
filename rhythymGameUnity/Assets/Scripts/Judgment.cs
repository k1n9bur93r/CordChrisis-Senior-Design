using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Judgment class

	The timing aspect of the hit detection system.
	Compares the time of the user's input versus the time of the note in question

	Important public methods:
		- bool CheckHit(): For use by InputController. Recieves the beat of a pressed note from the top of the note queue when a key is pressed. Returns whether or not the note was hit in a timing window.
		- bool CheckMiss(): For use by InputController. Recieves the beat of an unpressed note from the top of the queue. Returns whether or not the note has been missed completely.
*/

public class Judgment : MonoBehaviour
{
	public Metronome master;
	//public Scoreboard stats;

	// Placeholders until Scoreboard/something else implements UI!
	public Text ratingText;
	public Text leanText;

	enum Ratings { Miss, Good, Great, Perfect, Marvelous };
	
	private const double framesMarvelous = 1.0 / 60.0; // +/-16.7ms
	private const double framesPerfect = 2.0 / 60.0; // +/-33.3ms
	private const double framesGreat = 3.0 / 60.0; // +/-50.0ms
	private const double framesGood = 6.0 / 60.0; // +/-100.0ms

	private double beatsMarvelous;
	private double beatsPerfect;
	private double beatsGreat;
	private double beatsGood;

	void Start()
	{
		//CalculateWindows();
		ratingText.text = "";
		leanText.text = "";
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
		Returns true if the note was inside a timing window during the input.
	*/

	public bool CheckHit(double receivedBeat)
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
				ratingText.text = "Marvelous!!!";
				leanText.text = "";
				return true;
			}

			else
			{
				// Pass to some Scoreboard function later
				if (Math.Abs(diff) <= beatsPerfect) { ratingText.text = "Excellent!!"; }
				else if (Math.Abs(diff) <= beatsGreat) { ratingText.text = "Great!"; }
				else if (Math.Abs(diff) <= beatsGood) { ratingText.text = "Good"; }
				
				else
				{
					Debug.Log("ERROR: CheckHit() fell through!");
					Debug.Log("diff = currentBeat - noteBeat: " + diff + " = " + " currentBeat - " + " noteBeat");
					return false;
				}

				CheckLean(diff);
				return true;
			}
		}

		// The player tried to hit a note before it passed the early "Good" window
		else
		{
			return false;
		}

		// It should not be possible to hit beyond the late "Good" window, because the note should be deleted from the queue by now
	}

	/*
		Check if the non-best input hit the early or late side of the timing window.
	*/

	private void CheckLean(double diff)
	{
		if (diff < 0.0) { leanText.text = "EARLY"; } // Pass to some Scoreboard function later
		else if (diff > 0.0) { leanText.text = "LATE"; } // This too
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
		if (diff > beatsGood)
		{
			// Pass to some Scoreboard function later
			//stats.UpdateScoreTap(Ratings.Miss);
			ratingText.text = "Miss...";
			leanText.text = "";
			return true;
		}

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