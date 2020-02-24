using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Metronome class

	The driving force of the game's timing logic: an internal clock expressed in number of beats progressed in the song.
	This value is used to help position notes and judge the player's input timing.

	This class also plays the music.

	ALL game logic references to time should be relative to the beat via beatsElapsed and NOT Time.time!
	The only exception to this so far should be the instance in the Judgment class to to needing time-relativity.
	
	Important public variables:
		- double beatsElapsed: Current position in the song (in number of beats).
		- double tempo: Current tempo of the song.
*/

public class Metronome : MonoBehaviour
{
	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute
	private const float SONG_DELAY = 5.0f;

	public double tempo; // Song speed in beats per minute
	public double secPerBeat; // How many seconds in one beat <- So far MetronomeDebugger needs this to be public
	public double beatsPerSec; // How many beats in one second
	public double beatsElapsed; // Song position in beats

	private bool startFlag;
	private double songStart; // DSP time of song <- More precise than Time.time
	private double timeElapsed; // Song position in seconds
	private double timeElapsedLast;
	private double timeElapsedDelta; // Time elapsed since the last frame

	/*
		Initialize all timekeepers to 0.0.
		Determine amount of seconds per beat and beats per second.
		Start the music.
	*/
	
	void Start()
	{
		startFlag = false;
		beatsElapsed = 0.0;
		timeElapsed = 0.0;

		UpdateRates();

		songStart = AudioSettings.dspTime;
		timeElapsedLast = AudioSettings.dspTime - songStart;
		GetComponent<AudioSource>().Play(); // Eventually, we'll want it to play some time that isn't immediately
		//GetComponent<AudioSource>().PlayScheduled(songStart + SONG_DELAY);
	}

	/*
		Calculate what beat we're on using tempo and time elapsed (relative to when the song actually started).
		Eventually we'll want to store notes based on what BEAT and button they occur (and under no circumstances what TIME they occur).
	*/

	void Update()
	{
		if (GetComponent<AudioSource>().isPlaying) //if (startFlag)
		{
			// These will have to be determined via (AudioSettings.dspTime - songDelay) once we get music-playing going
			/*
			timeElapsed = Time.time; //AudioSettings.dspTime;
			timeElapsedDelta = Time.deltaTime;

			beatsElapsed += timeElapsedDelta / secPerBeat; // Needs to increment rather than set to honor BPM changes
			*/

			/*
			// BAD - Does not honor tempo changes!
			timeElapsed = AudioSettings.dspTime - songStart;
			beatsElapsed = timeElapsed / secPerBeat;
			*/

			// Increment to accomodate for tempo changes

			timeElapsed = AudioSettings.dspTime - songStart;
			timeElapsedDelta = timeElapsed - timeElapsedLast;
			beatsElapsed += timeElapsedDelta / secPerBeat;
			timeElapsedLast = timeElapsed;

			Debug.Log("timeElapsed: " + timeElapsed + " | delta: " + timeElapsedDelta);

			/*
			timeElapsed = AudioSettings.dspTime - songStart;
			timeElapsedDelta = timeElapsed - timeElapsedDelta;
			beatsElapsed = (timeElapsedDelta) / secPerBeat;
			*/			
		}

		UpdateRates();
	}

	/*
		DEBUG: Get time elapsed via DSP. Delete once no longer needed.
	*/

	public double getTimeElapsedDEBUG()
	{
		return timeElapsed;
	}

	/*
		Recalculate tick rates to compensate for tempo changes.
	*/

	private void UpdateRates()
	{
		secPerBeat = SEC_PER_MIN / tempo;
		beatsPerSec = tempo / SEC_PER_MIN;
	}
}