using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Metronome class

	The driving force of the game's timing logic: an internal clock expressed in number of beats progressed in the song.
	This value is used to help position notes, judge the player's timing, and time the music (it also plays it).

	ALL game logic references to time should be relative to the beat via beatsElapsed and NOT Time.time! It will desync otherwise!
	Time.time/Time.timeSinceLevelLoad is less accurate than AudioSettings.dspTime (which beatsElapsed is derived from)
	due to the latter being tied to the sound system and updating faster than a frame can update.
	
	Important public variables:
		- double beatsElapsed: Current position in the song (in number of beats).
		- double songDelay: User-determined delay. Used to compensate for silence at the beginning of the song. NOT IMPLEMENTED YET
		- double tempo: Current tempo of the song.
*/

public class Metronome : MonoBehaviour
{
	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute

	public double tempo; // Song speed in beats per minute
	public double secPerBeat; // How many seconds in one beat <- So far MetronomeDebugger needs this to be public
	public double beatsPerSec; // How many beats in one second
	public double beatsElapsed; // Song position in beats
	public double songDelay; // User-determined song start delay

	private double songStart; // DSP time of song <- More precise than Time.time
	private double timeElapsed; // Song position in seconds
	private double timeElapsedLast;
	private double timeElapsedDelta; // Time elapsed since the last frame

	/*
		Initialize all timekeepers to 0.0.
		Determine amount of seconds per beat and beats per second.
	*/
	
	void Start()
	{
		beatsElapsed = 0.0;
		timeElapsed = 0.0;

		UpdateRates();
		//startSong();
	}

	/*
		Calculate what beat we're on using tempo and time elapsed (relative to when the song actually started).
		Eventually we'll want to store notes based on what BEAT and button they occur (and under no circumstances what TIME they occur).
	*/

	void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Z)) && !GetComponent<AudioSource>().isPlaying)
		{
			startSong();
		}

		if (GetComponent<AudioSource>().isPlaying)
		{
			// Increment the timer by calculating DSP delta time (rather than using Time.deltaTime) instead of setting it to accomodate for tempo changes
			// Calculate how much DSP time has passed since the last frame and update beat counter accordingly

			timeElapsed = AudioSettings.dspTime - songStart;
			timeElapsedDelta = timeElapsed - timeElapsedLast;
			beatsElapsed += timeElapsedDelta / secPerBeat;
			timeElapsedLast = timeElapsed;

			//Debug.Log("timeElapsed: " + timeElapsed + " | delta: " + timeElapsedDelta);
			//Debug.Log("DSP time: " + timeElapsed + " | Real time: " + Time.time);
		}

		UpdateRates();
	}

	/*
		Initialize song start point.
		Play music.
	*/

	private void startSong()
	{
		songStart = AudioSettings.dspTime;
		timeElapsedLast = AudioSettings.dspTime - songStart;
		GetComponent<AudioSource>().Play(); // Eventually, we'll want it to play some time that isn't immediately to compensate for silence
		//GetComponent<AudioSource>().PlayScheduled(songStart + songDelay);
	}

	/*
		Calculate tick rates based on tempo.
	*/

	private void UpdateRates()
	{
		secPerBeat = SEC_PER_MIN / tempo;
		beatsPerSec = tempo / SEC_PER_MIN;
	}

	/*
		DEBUG: Get time elapsed via DSP time. Delete once no longer needed.
	*/

	public double getTimeElapsedDEBUG()
	{
		return timeElapsed;
	}
}