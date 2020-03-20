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
		- void StartSong(): For use by GameManager. Starts the music.

	Important public variables:
		- double beatsElapsed: Current position in the song (in number of beats).
		- double startOffset: Chart-determined chart delay (in seconds). Creates an offset between the chart's and song's start times.
		- double globalOffset: User-determined calibration for chart delay (in seconds). Creates an offset to compensate for audio/visual and input lag.
		- double tempo: Chart-determined tempo of the song.

	ISSUES:
		- Latency
		- Stutter: https://forum.unity.com/threads/heavy-audio-stutter-in-5-4-but-not-5-3.436664/
*/

public class Metronome : MonoBehaviour
{
	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute
	private const double FRAME_LENGTH = 1.0 / 60.0; // 0.0167 seconds in one frame
	private const double BUFFER_DELAY = 10.0 * FRAME_LENGTH; // Forces a delay of this length before starting the music

	public Track meta;

	// -DEBUG VARS-
	public double startBeat;
	private double startTime;

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
	private int tempoIndex;

	/*
		Initialize timekeepers.	
	*/
	
	void Awake()
	{
		beatsElapsed = 0.0;
		timeElapsed = 0.0;
		timeElapsedDelta = 0.0;
		pastSchedule = false;
		tempoIndex = 0;
	}

	/*
		Calculate what beat we're on using tempo and time elapsed, relative to when the song started playing according to the sound system.
	*/

	public void Update()
	{
		//UpdateTime();
		UpdateTimeAnywhere(); // DEBUG ONLY
		UpdateRates();
	}

	/*
		Initialize song start point.
		Play music after a brief delay.

		Sounds in Unity do not play immediately when Play() is called (audio latency).
		The only(?) way to guarantee that they will play on time is to schedule them to play in the future via PlayScheduled().

		ISSUES:
		- Unpredictable start times means there's still a slight amount of random, uncontrolled latency (enough to mess up timing)
	*/

	public void StartSong()
	{
		songStart = AudioSettings.dspTime; //- startOffset;
		timeElapsedLast = AudioSettings.dspTime - songStart;

		UpdateRates();
		GetComponent<AudioSource>().PlayScheduled(songStart + BUFFER_DELAY);
	}

	/*
		Update timers.
	*/

	public void UpdateTime()
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
					overtime = timeElapsed - (startOffset + globalOffset + BUFFER_DELAY);
					pastSchedule = true;
				}				

				// Calculate how much DSP time has passed since the last frame and update beat counter accordingly
				beatsElapsed += timeElapsedDelta / secPerBeat;
			}

			timeElapsedDelta = timeElapsed - timeElapsedLast;
			timeElapsedLast = timeElapsed;
		}
	}

	/*
		-DEBUG FUNCTION-
		Start music at an arbitrary point.
	*/

	public void StartSongAnywhere()
	{
		GetSongData();
		UpdateRates();

		GetComponent<AudioSource>().Play();
	}

	/*
		-DEBUG FUNCTION-
		Update the timer based on the arbitrary start point.
	*/

	public void UpdateTimeAnywhere()
	{
		if (Input.GetKey(KeyCode.P) && !GetComponent<AudioSource>().isPlaying)
		{
			StartSongAnywhere();
		}

		else if (GetComponent<AudioSource>().isPlaying) // This will return true once Play() or PlayScheduled()[!!!] is called, regardless if there's any sound playing
		{
			if (!pastSchedule)
			{
				UpdateRates();
				GetSongData();

				startTime = (startBeat * secPerBeat) + startOffset + globalOffset;
				//beatsElapsed = startBeat;

				//Debug.Log("[Metronome] startTime: " + startTime);

				songStart = AudioSettings.dspTime + startTime;
				timeElapsedLast = AudioSettings.dspTime - songStart + startTime;
				GetComponent<AudioSource>().time = (float)startTime;
				beatsElapsed = startBeat;
				pastSchedule = true;
			}

			// Increment the timer by calculating DSP delta time instead of directly setting it to accomodate for tempo changes
			timeElapsed = AudioSettings.dspTime - songStart + startTime;

			// Calculate how much DSP time has passed since the last frame and update beat counter accordingly
			beatsElapsed += timeElapsedDelta / secPerBeat;

			timeElapsedDelta = timeElapsed - timeElapsedLast;
			timeElapsedLast = timeElapsed;
		}
	}

	/*
		Get song metadata from JSON file.
	*/

	private void GetSongData()
	{
		tempo = meta.json.tempo_change_amount[0]; //meta.json.tempo;
		startOffset = meta.json.offset;
	}

	/*
		Check if tempo has changed.
		Calculate tick rates based on tempo.
	*/

	private void UpdateRates()
	{
		if ((tempoIndex < meta.json.tempo_change_beat.Length) && (beatsElapsed >= meta.json.tempo_change_beat[tempoIndex]))
		{
			tempo = meta.json.tempo_change_amount[tempoIndex];
			tempoIndex++;
		}

		secPerBeat = SEC_PER_MIN / tempo;
		beatsPerSec = tempo / SEC_PER_MIN;
	}
}