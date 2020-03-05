using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Metronome class

	The driving force of the game's timing logic: a running count of how many beats have elapsed in the music.
	This value is used to help position notes, judge the player's timing, and synchronize the music.

	With some exceptions, ALL game logic references to time should be relative to the current beat via beatsElapsed (because we're dealing with objects located by their assigned beat).
	Getting the current time via Time.time and Time.deltaTime will cause audio/visual drift!
	The public variables startOffset and globalOffset are exceptions to this due to needing a precise time in the music OUTSIDE of the context of the game.
	
	The Time class is less accurate than AudioSettings.dspTime (which beatsElapsed is derived from)
	due to the latter being tied to the sound system rather than frame updates.

	Notes must have their position set directly relative to the receptor, at a distance determined via beatsElapsed.
	Using actual velocity to move the notes will cause audio/visual drift!
	
	Important public variables:
		- double beatsElapsed: Current position in the song (in number of beats).
		- double startOffset: Chart-determined chart delay (in seconds). Creates an offset between the chart's and song's start times.
		- double globalOffset: User-determined chart delay (in seconds). Creates an offset to compensate for audio/visual and input lag.
		- double tempo: Chart-determined tempo of the song.

	TO DO:
		- Arbitrary seeking/playback
		- See if anything can be done about sound latency
*/

public class Metronome : MonoBehaviour
{
	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute
	private const double FRAME_LENGTH = 1.0 / 60.0;
	private const double BUFFER_DELAY = 10.0 * FRAME_LENGTH;

	public Track meta;

	public double tempo; // Song speed in beats per minute
	public double beatsPerSec; // How many beats in one second <- Public for Judgment
	public double beatsElapsed; // Song position in beats
	public double startOffset; // Chart-determined chart/song offset
	public double globalOffset; // User-determined chart/song offset

	private double songStart; // DSP time reference point for beginning of playback
	private double secPerBeat; // How many seconds in one beat
	private double timeElapsed; // Song position based on DSP time's original reference point and current point in time
	private double timeElapsedLast;
	private double timeElapsedDelta; // DSP time elapsed since the last frame

	/*
		Initialize timekeepers.
		Determine amount of seconds per beat and beats per second.
	*/
	
	void Start()
	{
		beatsElapsed = 0.0;
		timeElapsed = 0.0;

		//GetSongData(); // Game manager?
		//UpdateRates();
	}

	/*
		Calculate what beat we're on using tempo and time elapsed, relative to when the song started playing according to the DSP (not Time.time).
		Notes are utilized relative to the exact beat (as opposed to the exact time) they are used on.
	*/

	void Update()
	{
		GetSongData();

		if ((Input.GetKeyDown(KeyCode.Z)) && (!GetComponent<AudioSource>().isPlaying))
		{
			startSong();
		}

		if (GetComponent<AudioSource>().isPlaying) // Returns true if PlayScheduled is used, regardless if audio is actually playing
		{
			// Increment the timer by calculating DSP delta time (rather than using Time.deltaTime) instead of directly setting it to accomodate for tempo changes
			timeElapsed = AudioSettings.dspTime - songStart;
			timeElapsedDelta = timeElapsed - timeElapsedLast;
			timeElapsedLast = timeElapsed;

			// If the initial delays have not elapsed yet, do not increase beatsElapsed
			if (timeElapsed >= (startOffset + globalOffset + BUFFER_DELAY))
			{
				// Calculate how much DSP time has passed since the last frame and update beat counter accordingly
				beatsElapsed += timeElapsedDelta / secPerBeat;
			}
		}

		UpdateRates();
	}

	/*
		Initialize song start point.
		Play music.
	*/

	private void startSong()
	{
		songStart = AudioSettings.dspTime; //- startOffset;
		timeElapsedLast = AudioSettings.dspTime - songStart;
		//GetComponent<AudioSource>().Play(); // Eventually, we'll want it to play some time that isn't immediately because sounds don't play "immediately", delay it instead
		GetComponent<AudioSource>().PlayScheduled(songStart + BUFFER_DELAY);//startOffset);
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