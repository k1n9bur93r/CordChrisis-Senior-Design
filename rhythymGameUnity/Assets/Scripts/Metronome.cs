using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> Metronome class

	The driving force of the game's timing logic: a running count of how many beats have elapsed in the music.
	This value is used to help position notes, judge the player's timing, and synchronize the music.

	With some exceptions, ALL game logic references to time should be relative to the current beat via beatsElapsed (because we're dealing with objects located by their assigned beat).
	
	The Time class is less accurate than AudioSettings.dspTime (which beatsElapsed is derived from)
	due to the latter being tied to the sound system rather than frame updates.

	Notes must have their position set directly relative to the receptor, at a distance determined via beatsElapsed.
	Using actual velocity to move the notes will cause audio/visual drift!
	
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
	private const double BUFFER_DELAY = 30.0 * FRAME_LENGTH; // Forces a delay of this length before starting the music

	public Track meta;

	public double tempo; // Song speed in beats per minute
	public double beatsPerSec; // How many beats in one second <- Public for Judgment
	public double beatsElapsed; // Song position in beats
	public double startOffset; // Chart-determined chart/song offset
	public double globalOffset; // User-determined chart/song offset

	private bool startFlag;
	private bool overtimeFlag;
	//private double songStart; // DSP time reference point for beginning of playback
	private double secPerBeat; // How many seconds in one beat
	private double timeMaster;
	private double timeElapsedDSP; // Song position based on DSP time's original reference point and current point in time
	private double timeElapsedLastDSP;
	private double timeElapsedDeltaDSP; // DSP time elapsed since the last frame
	private double scheduledTime;
	//private double timeElapsedReal; // Overtime compensator

	/*
		Initialize timekeepers.	
		Determine amount of seconds per beat and beats per second.
	*/
	
	void Start()
	{
		beatsElapsed = 0.0;
		timeElapsedDSP = 0.0;

		startFlag = false;
		overtimeFlag = false;

		//GetSongData(); // Game manager?
		//UpdateRates();
	}

	/*
		Calculate what beat we're on using tempo and time elapsed, relative to when the song started playing according to the audio system.
	*/

	void Update()
	{
		GetSongData();

		if ((Input.GetKeyDown(KeyCode.Z)) && (!startFlag))
		{
			startSong();
		}

		if (startFlag)
		{
			// DSP delta time calculation
			timeElapsedDSP = AudioSettings.dspTime;
			
			if (!overtimeFlag)
			{
				if (timeElapsedDSP > scheduledTime)
				{
					timeElapsedDSP = scheduledTime;
					Debug.Log("Overtime!");

					Debug.Log("(scheduledTime + startOffset + globalOffset) = " + (scheduledTime + startOffset + globalOffset) + " | DSP: " + timeElapsedDSP);

					overtimeFlag = true;
				}
			}

			timeElapsedDeltaDSP = timeElapsedDSP - timeElapsedLastDSP;
			timeElapsedLastDSP = timeElapsedDSP;

			// If the initial delays have not elapsed yet, do not increase beatsElapsed
			if (((timeElapsedDSP > (scheduledTime + startOffset + globalOffset)) && overtimeFlag))
			{
				// Calculate how much DSP time has passed since the last frame and update beat counter accordingly
				// Increment the timer by calculating DSP delta time instead of directly setting it to accomodate for tempo changes
				beatsElapsed += timeElapsedDeltaDSP / secPerBeat;
			}
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

	private void startSong()
	{
		timeElapsedLastDSP = AudioSettings.dspTime;
		scheduledTime = timeElapsedLastDSP + BUFFER_DELAY;

		GetComponent<AudioSource>().PlayScheduled(BUFFER_DELAY);

		startFlag = true;
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