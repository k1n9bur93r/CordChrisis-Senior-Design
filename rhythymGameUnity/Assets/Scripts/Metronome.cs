using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Metronome

	The driving force of the game's timing logic: an internal clock expressed in number of beats progressed in the song.
	This value is used to help position notes and judge the player's input timing.

	ALL game logic references to time should be relative to the beat via beatsElapsed and NOT Time.time!
	The only exception to this so far should be the instance in the Judgment class to to needing time-relativity.
	
	Important public variables:
		double beatsElapsed: Current position in the song (in number of beats).
		double tempo: Current tempo of the song.

	Important enums:
		[NOT USABLE YET] AccRating?
*/

public class Metronome : MonoBehaviour
{
	//public enum AccRating { Marvelous, Perfect, Great, Good, Miss, None };

	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute

	public double tempo; // Song speed in beats per minute
	public double secPerBeat; // How many seconds in one beat
	public double beatsPerSec; // How many beats in one second
	public double beatsElapsed; // Song position in beats

	private double songDelay; // Time before audio starts, determine via AudioSettings.dspTime later
	private double timeElapsed; // Song position in seconds
	private double timeElapsedDelta; // Time passed since the last frame

	/*
		Initialize all timekeepers to 0.0.
		Determine amount of seconds per beat and beats per second.
	*/
	
	void Start()
	{
		beatsElapsed = 0.0;
		timeElapsed = 0.0;
		timeElapsedDelta = 0.0;

		UpdateRates();
	}

	/*
		Calculate what beat we're on using tempo and time elapsed (relative to when the song actually started).
		Eventually we'll want to store notes based on what beat and button they occur (and under no circumstances what time they occur).
	*/

	void Update()
	{
		// These will have to be determined via (AudioSettings.dspTime - songDelay) once we get music-playing going
		timeElapsed = Time.time; //AudioSettings.dspTime;
		timeElapsedDelta = Time.deltaTime;

		beatsElapsed += timeElapsedDelta / secPerBeat;

		UpdateRates();
	}

	/*
		Call this to recalculate tick rates if the tempo changes.
	*/

	private void UpdateRates()
	{
		secPerBeat = SEC_PER_MIN / tempo;
		beatsPerSec = tempo / SEC_PER_MIN;
	}
}