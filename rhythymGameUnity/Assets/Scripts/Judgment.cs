using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Judgment class

	The timing aspect of the hit detection system.
	Compares the time of the user's input versus the time of the note in question.

	Important public methods:
		- bool CheckHit(): For use by InputController. Recieves the beat of a pressed note from the top of the tap/hold queue when a key is pressed. Returns whether or not the note was hit in a timing window. Sends information to scoreboard.
		- bool CheckMiss(): For use by InputController. Recieves the beats of the next two notes from either note queue. Returns whether or not the note has been missed completely. Sends information to scoreboard.
		- bool CheckSwipe(): For use by InputController. Receives the beat of swiped note from the top of the swipe queue when a swipe occurs. Returns whether or not the note was hit. Sends information to scoreboard.
		- double ReduceHoldInitial(): For use by InputController. Adjusts the required amount of time for a hold to be held dependent on timing. Returns a new required hold time.
		- double ReduceHoldDuring(): For use by InputController. Decrements the time left for a hold note to be held. Returns a new required hold time.
		- bool HoldSuccess(): For use by InputController. Sends information to scoreboard.
		- bool HoldFailure(): For use by InputController. Sends information to scoreboard.

	KNOWN ISSUES:
		- 32nd notes at 300+ BPM have spotty miss detection.
*/

public class Judgment : MonoBehaviour
{
	public Metronome clock;
	public Scoreboard stats;

	//private const double ONE_FRAME = 1.0 / 60.0; // 0.0167

	// Colossal timing windows
	private const double FRAMES_MARVELOUS = 2.0 / 60.0; // Rainbow Perfect
	private const double FRAMES_PERFECT = 4.0 / 60.0; // Perfect
	private const double FRAMES_GOOD = 8.0 / 60.0; // Good

	/*
	// Accurate timing windows
	private const double FRAMES_MARVELOUS = 2.0 / 60.0; // Rainbow Perfect
	private const double FRAMES_PERFECT = 4.0 / 60.0; // Perfect
	private const double FRAMES_GOOD =  8.0 / 60.0; // Good
	private const double FRAMES_BAD = 10.0 / 60.0; // Bad
	*/

	private double beatsMarvelous, beatsPerfect, beatsGreat, beatsGood;

	void Awake()
	{
		//CalculateWindows();
	}

	void Update()
	{
		//CalculateWindows();
		//PrintWindows();
		//DrawStats(); // DEBUG
	}

	/*
		Determine how many beats are in each timing window.
	*/

	private void CalculateWindows()
	{
		beatsMarvelous = clock.beatsPerSec * FRAMES_MARVELOUS;
		beatsPerfect = clock.beatsPerSec * FRAMES_PERFECT;
		beatsGood = clock.beatsPerSec * FRAMES_GOOD;
	}

	/*
		When a key is pressed, judge the player's timing by checking the beat of the note sent from the queue versus the current beat.
		Returns true if the note was inside a timing window during the input.
	*/

	public bool CheckHit(double receivedBeat)
	{
		CalculateWindows();

		double currentBeat = clock.beatsElapsed;
		double noteBeat = receivedBeat;
		double diff = currentBeat - noteBeat;

		// ---

		// Check if the player hits at least the early "Good" window
		if (diff >= -beatsGood)
		{
			if (Math.Abs(diff) <= beatsMarvelous)
			{
				stats.UpdateScore(Ratings.Marvelous, Leanings.None);
				return true;
			}

			else
			{
				if (Math.Abs(diff) <= beatsPerfect)
				{
					stats.UpdateScore(Ratings.Perfect, CheckLean(diff));
				}
				
				else if (Math.Abs(diff) <= beatsGood)
				{
					stats.UpdateScore(Ratings.Good, CheckLean(diff));
				}
				
				else
				{
					Debug.Log("[Judgment] CheckHit() fell through!");
					Debug.Log("[Judgment] currentBeat: " + currentBeat + " | noteBeat: " + noteBeat + " | diff: " + diff);
					return false;
				}

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
		Hold "judgment" functions
	*/

	public double ReduceHoldInitial(double beatsLeft, double initialBeat)
	{
		double currentBeat = clock.beatsElapsed;
		double noteBeat = initialBeat;
		double diff = currentBeat - noteBeat;

		return beatsLeft - diff;
	}

	public double ReduceHoldDuring(double beatsLeft)
	{
		return beatsLeft - clock.beatsElapsedDelta;
	}

	public void HoldSuccess()
	{
		stats.UpdateScore(Ratings.Marvelous, Leanings.None);
	}

	public void HoldFailure()
	{
		stats.UpdateScore(Ratings.Miss, Leanings.None);
	}

	/*
		Swipe judgment function
	*/

	public bool CheckSwipe(double receivedBeat)
	{
		CalculateWindows();

		double currentBeat = clock.beatsElapsed;
		double noteBeat = receivedBeat;
		double diff = currentBeat - noteBeat;

		// ---

		// If the swipe is caught in ANY valid timing window, automatically rate it a "Marvelous"
		if (diff >= -beatsGood)
		{
			stats.UpdateScore(Ratings.Marvelous, Leanings.None);
			return true;
		}

		// The player tried to swipe before it passed the early "Good" window
		else
		{
			return false;
		}
	}

	/*
		Check if the note at the top of the queue has gone unpressed for too long.
		Returns true if either...
			- its beat exceeds the current threshold of the "Miss" window (which is actually just the area beyond the late "Good" window).
			- the beat of the note after the top note is overlapping the receptor.
	*/

	public bool CheckMiss(double nearBeat, double farBeat)
	{
		CalculateWindows();

		double currentBeat = clock.beatsElapsed;
		double diff = currentBeat - nearBeat;

		// If the note after the next one is overlapping the receptor, immediately miss
		if (currentBeat >= farBeat)
		{
			stats.UpdateScore(Ratings.Miss, Leanings.None);
			return true;
		}

		// If the beat difference between now and the note's location exceeds the size of the late "Good" window, it's now too late to hit, delete from the queue
		if (diff > beatsGood)
		{
			stats.UpdateScore(Ratings.Miss, Leanings.None);
			return true;
		}

		// Otherwise, do not delete from the queue
		else
		{
			return false;
		}
	}

	/*
		Check if the non-Marvelous input hit the early or late side of the timing window.
	*/

	private Leanings CheckLean(double diff)
	{
		if (diff < 0.0)
		{ 
			return Leanings.Early;
		}

		else //if (diff > 0.0)
		{
			return Leanings.Late;
		}
	}

	/*
		Debug function to print the size of the timing windows.
	*/

	private void PrintWindows()
	{
		string judgmentWindows =
			"R-Perfect: +/- " + beatsMarvelous + " beats (+/- " + FRAMES_MARVELOUS + " sec)\n"
			+ "Perfect: +/- " + beatsPerfect + " beats (+/- " + FRAMES_PERFECT + " sec)\n"
			+ "Good: +/- " + beatsGood + " beats (+/- " + FRAMES_GOOD + " sec)";

		Debug.Log(judgmentWindows);
	}
}