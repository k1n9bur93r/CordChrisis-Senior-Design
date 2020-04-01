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
	private const double SIXTYFOUR_NOTE = 0.03125;

	public Track meta;
	public YoutubeSimplified player;

	public Text timer;

	private bool playbackStarted;

	public double startBeat;
	private double startTime;

	private double videoTime;

	public double tempo; // Song speed in beats per minute
	public double beatsPerSec; // How many beats in one second <- Public for Judgment
	public double beatsElapsed; // Song position in beats
	public double beatsElapsedDelta;
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
		beatsElapsedDelta = 0.0;
		timeElapsed = 0.0;
		timeElapsedDelta = 0.0;
		
		playbackStarted = false;
		pastSchedule = false;
		tempoIndex = 0;
	}

	/*
		Calculate what beat we're on using tempo and time elapsed, relative to when the song started playing according to the sound system.
	*/

	public void Update()
	{
		UpdateTimeAnywhere();
		UpdateRates();
	}

	/*
		Start music at an arbitrary point.
	*/

	public void StartSongAnywhere()
	{
		GetSongData();
		
		// Step through every tempo change and increment the correct amount of time!

		double anyTime = 0.0;
		startTime = 0.0;

		// Move forward
		for (double i = 0.0; i <= startBeat; i += SIXTYFOUR_NOTE)
		{
			if ((tempoIndex < meta.json.tempo_change_beat.Length) && (i >= meta.json.tempo_change_beat[tempoIndex]))
			{
				tempo = meta.json.tempo_change_amount[tempoIndex];
				tempoIndex++;
			}

			secPerBeat = SEC_PER_MIN / tempo / 32.0;
			anyTime += secPerBeat;

			beatsElapsed = i;
		}

		startTime = anyTime;

		player.Play();
	}

	/*
		Check if the video restarted for any reason and recover by resetting everything.
	*/

	private void CheckRestart()
	{
		if (((startBeat != 0) && (videoTime == 0))
			|| ((startBeat == 0) && (beatsElapsed > 0.0) && ((timeElapsed - videoTime) >= 0.2)))
			//|| ((startBeat == 0) && (videoTime == 0) && ((timeElapsed - videoTime) >= 0.33)))
		{
			player.player.Pause();
			player.player.Seek(0.0);
			beatsElapsed = startBeat;
			pastSchedule = false;

			Debug.Log("[Metronome] Video restarted!");
		}
	}

	/*
		Update the timer based on the arbitrary start point.
	*/

	public void PlaybackStarted()
	{
		playbackStarted = true;
	}

	public void UpdateTimeAnywhere()
	{
		videoTime = player.player.videoPlayer.time;

		timer.text = 
			"DSP time: " + timeElapsed.ToString() + "\n"
			+ "Video time: " + videoTime.ToString();

		if (!playbackStarted)
		{
			if (Input.GetKey(KeyCode.P))
			{
				StartSongAnywhere();
			}
		}

		else
		{
			if (!pastSchedule)
			{
				if (startBeat != 0)
				{
					player.player.Seek(startTime + startOffset);
				}
				
				player.player.Pause();

				if ((startBeat == 0.0) || (videoTime != 0))
				{
					// Need a check for if the video restarted if warping
					player.player.Play();
					songStart = AudioSettings.dspTime + startTime;
					timeElapsedLast = AudioSettings.dspTime - songStart + startTime;

					pastSchedule = true;
				}
			}

			else
			{
				timeElapsed = AudioSettings.dspTime - songStart + startTime;

				if (startBeat == 0.0)
				{
					if ((timeElapsed >= (globalOffset + startOffset)))
					{
						beatsElapsed += beatsElapsedDelta;
					}
				}

				else
				{
					if (timeElapsed >= globalOffset)
					{
						beatsElapsed += beatsElapsedDelta;
					}
				}

				timeElapsedDelta = timeElapsed - timeElapsedLast;
				timeElapsedLast = timeElapsed;

				CheckRestart();
			}
		}
	}

	/*
		Get song metadata from JSON file.
	*/

	private void GetSongData()
	{
		tempo = meta.json.tempo_change_amount[0];
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
		beatsElapsedDelta = timeElapsedDelta / secPerBeat;
	}

	/*
		YOUTUBE PLAYER CALLBACK TESTERS
	*/

	/*
	public void a1()
	{
		Debug.Log("When the url's are loaded");
	}

	public void a2()
	{
		Debug.Log("When the videos are ready to play");
	}

	public void a3()
	{
		Debug.Log("When the video start playing");
	}

	public void a4()
	{
		Debug.Log("When the video pause");
	}

	public void a5()
	{
		Debug.Log("When the video finish");
	}
	*/
}