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
	public Metronome clock;
	//public Scoreboard stats;

	// Placeholders until Scoreboard/something else implements UI!
	public Text ratingText;
	public Text leanText;
	public Text comboText;
	public Text statsText;
	
	private int notesMarvelous, notesPerfect, notesGreat, notesGood, notesMiss;
	private int notesEarly, notesLate;
	private int combo, comboMax;
	private int score;
	// End of placeholders

	enum Ratings { Miss, Good, Great, Perfect, Marvelous };
	enum Leanings { Early, Late };

	//private const double ONE_FRAME = 1.0 / 60.0; // 0.0167

	// Large-ish timing windows
	private const double FRAMES_MARVELOUS = 22.5 / 1000.0;
	private const double FRAMES_PERFECT = 45.0 / 1000.0;
	private const double FRAMES_GREAT = 90.0 / 1000.0;
	private const double FRAMES_GOOD = 180.0 / 1000.0;

	/*
	// Stricter windows for testing overtime
	private const double FRAMES_MARVELOUS = 1.0 / 60.0;
	private const double FRAMES_PERFECT = 2.0 / 60.0;
	private const double FRAMES_GREAT = 4.0 / 60.0;
	private const double FRAMES_GOOD = 8.0 / 60.0;
	*/

	private double beatsMarvelous, beatsPerfect, beatsGreat, beatsGood;

	void Start()
	{
		//CalculateWindows();
		ratingText.text = "";
		leanText.text = "";
		comboText.text = "";

		notesMarvelous = 0;
		notesPerfect = 0;
		notesGreat = 0;
		notesGood = 0;
		notesMiss = 0;
		combo = 0;
		comboMax = 0;
		score = 0;
	}

	void Update() // Update() won't actually be used, thus not converted to Action()
	{
		//CalculateWindows();
		//PrintWindows();
		DrawStats(); // DEBUG
	}

	/*
		Determine how many beats are in each timing window.
	*/

	private void CalculateWindows()
	{
		beatsMarvelous = clock.beatsPerSec * FRAMES_MARVELOUS;
		beatsPerfect = clock.beatsPerSec * FRAMES_PERFECT;
		beatsGreat = clock.beatsPerSec * FRAMES_GREAT;
		beatsGood = clock.beatsPerSec * FRAMES_GOOD;
	}

	/*
		When a key is pressed, judge the player's timing by checking the beat of the note sent from the queue versus the current beat.
		Returns true if the note was inside a timing window during the input.
	*/

	public bool CheckHit(double noteBeat)
	{
		CalculateWindows();

		double currentBeat = clock.beatsElapsed;
		double diff = currentBeat - noteBeat;

		// ---

		// Check if the player hits at least the early "Good" window
		if (diff >= -beatsGood)
		{
			if (Math.Abs(diff) <= beatsMarvelous)
			{
				ratingText.text = "Superb!!!";
				leanText.text = "";
				notesMarvelous++;
				combo++;
				CalculateScore(Ratings.Marvelous);
				//CheckLean(diff); // DEBUG ONLY

				return true;
			}

			else
			{
				// Pass to some Scoreboard function later
				if (Math.Abs(diff) <= beatsPerfect)
				{
					ratingText.text = "Atomic!!";
					notesPerfect++; 
					combo++;
					CalculateScore(Ratings.Perfect);
				}

				else if (Math.Abs(diff) <= beatsGreat)
				{
					ratingText.text = "Bravo!";
					notesGreat++;
					combo++;
					CalculateScore(Ratings.Great);
				}
				
				else if (Math.Abs(diff) <= beatsGood)
				{
					combo = 0;
					comboText.text = "";
					ratingText.text = "Close";
					notesGood++;
					CalculateScore(Ratings.Good);
				}
				
				else
				{
					Debug.Log("[Judgment] CheckHit() fell through!");
					Debug.Log("[Judgment] currentBeat: " + currentBeat + " | noteBeat: " + noteBeat + " | diff: " + diff);

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
		When the beginning of a swipe is detected, get that time. If it falls into a timing window, returns true.		
		If that function returns true, when the end of a swipe is detected, get that time. If it falls into a timing window, returns true and scores.
	*/

	public bool CheckSwipeStart(double noteBeat)
	{
		CalculateWindows();

		double currentBeat = clock.beatsElapsed;
		double diff = currentBeat - noteBeat;

		// ---

		// Huge start window
		if (diff >= -beatsGood)
		{
			return true;
		}

		// Too early, do not judge
		else
		{
			return false;
		}
	}

	public bool CheckSwipeEnd(double noteBeat)
	{
		CalculateWindows();

		double currentBeat = clock.beatsElapsed;
		double diff = currentBeat - noteBeat;

		// ---

		// Huge start window
		if (diff >= -beatsGood)
		{
			// Pass stuff to scoreboard
			return true;
		}

		// Too early, do not judge
		else
		{
			return false;
		}
	}

	/*
		Check if the non-best input hit the early or late side of the timing window.
	*/

	private void CheckLean(double diff)
	{
		if (diff < 0.0)  // Pass to some Scoreboard function later
		{ 
			leanText.text = "EARLY";
			notesEarly++;
		}

		else if (diff > 0.0)
		{
			leanText.text = "LATE";
			notesLate++; // This too
		}

		// DEBUG ONLY
		else if (diff == 0.0)
		{
			leanText.text = "JUST";
		}
	}

	/*
		Check if the note at the top of the queue has gone unpressed for too long.
		Returns true if its beat exceeds the current threshold of the "Miss" window (which is actually just the area beyond the late "Good" window).
	*/

	public bool CheckMiss(double noteBeat)
	{
		CalculateWindows();

		double currentBeat = clock.beatsElapsed;
		double diff = currentBeat - noteBeat;

		 // If the beat difference between now and the note's location exceeds the size of the late "Good" window, it's now too late to hit, delete from the queue
		if (diff > beatsGood)
		{
			// Pass to some Scoreboard function later
			//stats.UpdateScoreTap(Ratings.Miss);
			ratingText.text = "Miss...";
			comboText.text = "";
			leanText.text = "";
			notesMiss++;
			combo = 0;

			return true;
		}

		// Otherwise, do not delete from the queue
		else
		{
			return false;
		}
	}

	/*
		Debug functions to test out a normalized combo scoring system.
	*/

	private void CalculateScore(Ratings rate)
	{
		//AccuracyScore(rate);
		//ComboScore();
	}

	private void AccuracyScore(Ratings rate)
	{
		const double accScoreBase = 700000.0;
		const double totalNotes = 164.0;

		double baseNoteValue = accScoreBase / totalNotes;

		double valueMarvelous = baseNoteValue;
		double valuePerfect = baseNoteValue * 0.99;//- 50;
		double valueGreat = baseNoteValue * 0.66;
		double valueGood = baseNoteValue * 0.33;

		switch (rate)
		{
			case Ratings.Marvelous:
				score += (int)valueMarvelous;
				break;

			case Ratings.Perfect:
				score += (int)valuePerfect;
				break;

			case Ratings.Great:
				score += (int)valueGreat;
				break;

			case Ratings.Good:
				score += (int)valueGood;
				break;

			default:
				break;
		}
	}

	private void ComboScore()
	{
		const double comboScoreBase = 300000.0;
		const double totalNotes = 164.0;

		double comboBonus = comboScoreBase * (1.0 / (totalNotes - 10.0));

		if (combo > 10)
		{
			score += (int)comboBonus;
		}
	}

	/*
		Debug function to print the size of the timing windows.
	*/

	private void PrintWindows()
	{
		string judgmentWindows =
			"Marvelous: +/- " + beatsMarvelous + " beats (+/- " + FRAMES_MARVELOUS + " sec)\n"
			+ "Excellent: +/- " + beatsPerfect + " beats (+/- " + FRAMES_PERFECT + " sec)\n"
			+ "Great: +/- " + beatsGreat + " beats (+/- " + FRAMES_GREAT + " sec)\n"
			+ "Good: +/- " + beatsGood + " beats (+/- " + FRAMES_GOOD + " sec)";

		Debug.Log(judgmentWindows);
	}

	/*
		Debug function to display play statistics.
	*/

	private void DrawStats()
	{
		if (combo > 0)
		{
			comboText.text = combo.ToString();
		}

		if (comboMax < combo)
		{
			comboMax = combo;
		}

		statsText.text =
			score.ToString() + " Score\n\n"
			+ notesMarvelous.ToString() + " Superb\n"
			+ notesPerfect.ToString() + " Atomic\n"
			+ notesGreat.ToString() + " Bravo\n"
			+ notesGood.ToString() + " Close\n"
			+ notesMiss.ToString() + " Miss\n\n"
			+ notesEarly.ToString() + " Early\n"
			+ notesLate.ToString() + " Late\n\n"
			+ comboMax.ToString() + " Max Combo";
	}
}