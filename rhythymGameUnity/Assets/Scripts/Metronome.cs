using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Metronome class

	The driving force of the game's timing logic: a running count of how many beats have elapsed in the music.
	This value is used to help position notes, judge the player's timing, and synchronize the music.

	With some exceptions, all game logic references to time should be relative to the current beat via beatsElapsed (because we're dealing with objects located by their assigned beat).
	
	The Time class is less accurate than AudioSettings.dspTime (which beatsElapsed is derived from)
	due to the latter being tied to the sound system rather than frame updates.

	Notes must have their position set directly relative to the receptor, at a distance determined via beatsElapsed.
	Using actual velocity to move the notes will cause audio/visual drift!
	
	Important public methods:
		- void StartSong(): Starts the music.

	Important public variables:
		- double beatsElapsed: Current position in the song (in number of beats).
		- double startOffset: Chart-determined chart delay (in seconds). Creates an offset between the chart's and song's start times.
		- double globalOffset: User-determined calibration for chart delay (in seconds). Creates an offset to compensate for audio/visual and input lag.
		- double tempo: Chart-determined tempo of the song.

	TO DO:
		- Arbitrary seeking/playback
		- See if anything can be done about sound latency
*/

public class Metronome : MonoBehaviour
{
	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute
	private const double FRAME_LENGTH = 1.0 / 60.0; // 0.0167 seconds in one frame
	private const double BUFFER_DELAY = 10.0 * FRAME_LENGTH; // Forces a delay of this length before starting the music

	public Track meta;

	public double tempo; // Song speed in beats per minute
	public double beatsPerSec; // How many beats in one second <- Public for Judgment
	public double beatsElapsed; // Song position in beats
	public double startOffset; // Chart-determined chart/song offset
	public double globalOffset; // User-determined chart/song offset

	private bool pastSchedule;
	private double songStart; // DSP time reference point for beginning of playback
	private double secPerBeat; // How many seconds in one beat
	private double timeElapsed; // Song position based on DSP time's original reference point and current point in time
	private double timeElapsedLast;
	private double timeElapsedDelta; // DSP time elapsed since the last frame
	private double overtime;

	/*
		Initialize timekeepers.	
		Determine amount of seconds per beat and beats per second.
	*/
	
	void Start()
	{
		beatsElapsed = 0.0;
		timeElapsed = 0.0;
		timeElapsedDelta = 0.0;
		pastSchedule = false;

		GetSongData();
		UpdateRates();
	}

	/*
		Calculate what beat we're on using tempo and time elapsed, relative to when the song started playing according to the sound system.
	*/

	public void Action() //Update()
	{
		if (GetComponent<AudioSource>().isPlaying) // This will return true once Play() or PlayScheduled()[!!!] is called, regardless if there's any sound playing
		{
			// Increment the timer by calculating DSP delta time (rather than using Time.deltaTime) instead of directly setting it to accomodate for tempo changes
			timeElapsed = AudioSettings.dspTime - songStart - overtime;

			// If the initial delays have not elapsed yet, do not increase beatsElapsed
			if (timeElapsed >= (startOffset + globalOffset + BUFFER_DELAY))
			{
				// Once the timer passes the initial delay for the first time, force its value to equal the initial delay.
				if (!pastSchedule)
				{
					//Debug.Log("timeElapsed (old): " + timeElapsed + " | stuff: " + (startOffset + globalOffset + BUFFER_DELAY));
					overtime = timeElapsed - (startOffset + globalOffset + BUFFER_DELAY);
					//timeElapsed -= overtime;
					//Debug.Log("timeElapsed (new): " + timeElapsed);

					pastSchedule = true;
				}
				
				// Calculate how much DSP time has passed since the last frame and update beat counter accordingly
				beatsElapsed += timeElapsedDelta / secPerBeat;
			}

			timeElapsedDelta = timeElapsed - timeElapsedLast;
			timeElapsedLast = timeElapsed;
		}

		UpdateRates();
	}

	/*
		Initialize song start point.
		Play music after a brief delay.

		Sounds in Unity do not play immediately when Play() is called (audio latency).
		The only(?) way to guarantee that they will play on time is to schedule them to play in the future via PlayScheduled().

		ISSUES:
		- Unpredictable start times means there's still a slight random offset
	*/

	public void StartSong()
	{
		songStart = AudioSettings.dspTime; //- startOffset;
		timeElapsedLast = AudioSettings.dspTime - songStart;
		GetComponent<AudioSource>().PlayScheduled(songStart + BUFFER_DELAY);
	}

	/*
		Get song metadata from JSON file.
	*/

	private void GetSongData()
	{
		tempo = meta.json.tempo;
		startOffset = meta.json.offset;
	}

	/*
		Calculate tick rates based on tempo.
	*/

	private void UpdateRates()
	{
		secPerBeat = SEC_PER_MIN / tempo;
		beatsPerSec = tempo / SEC_PER_MIN;
	}
}