using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	METRONOME CLASS

	The driving force of the game's timing logic: an internal clock expressed in number of beats.
	By using the song's time elapsed and tempo, it expresses where we are in the song by number of beats.
	This value is used to help position notes (see NoteSpawner) and rate the player's input timing (see Judgment).

	Please keep ALL references to time beat-relative by using the public variable beatsElapsed!
	
	Important public variables:
		double tempo: Current tempo of the song.
		double beatsElapsed: Current position in the song (in number of beats).

	Important public methods:
		void UpdateRates(): Update the beats per second and seconds per beat values in the event of a tempo change.
*/

public class Metronome : MonoBehaviour
{
	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute

	public double tempo;
	private double songDelay; // Time before audio starts, determine via AudioSettings.dspTime later

	public double secPerBeat; // How many seconds in one beat
	public double beatsPerSec; // How many beats in one second
	private double timeElapsed; // Position in seconds
	private double timeElapsedDelta;
	public double beatsElapsed; // Position in beats

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
		Calculate what beat we're on via deltas (instead of the initially described tick counter).
		Eventually we'll want to store notes based on what beat they occur (and under no circumstances what time they occur).
	*/

	void Update()
	{
        // These will have to be determined via (AudioSettings.dspTime - songDelay) once we get music-playing going
		timeElapsed = Time.time; //Time.time; //AudioSettings.dspTime;
		timeElapsedDelta = Time.deltaTime;

		beatsElapsed += timeElapsedDelta / secPerBeat;
	}

	/*
		Call this to recalculate tick rates if the tempo changes.
	*/

	public void UpdateRates()
	{
		secPerBeat = SEC_PER_MIN / tempo;
		beatsPerSec = tempo / SEC_PER_MIN;
	}
}