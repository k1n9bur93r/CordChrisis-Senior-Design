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
		- double beatZeroOffset: Chart-determined chart delay (in seconds). Creates an offset between the chart's and song's start times.
		- double userOffset: User-determined calibration for chart delay (in seconds). Creates an offset to compensate for audio/visual and input lag.
		- double tempo: Chart-determined tempo of the song.

	ISSUES:
		- Latency
		- Stutter: https://forum.unity.com/threads/heavy-audio-stutter-in-5-4-but-not-5-3.436664/
*/

public class Metronome : MonoBehaviour
{
	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute
	private const double FRAME_LENGTH = 1.0 / 60.0; // 0.0167 seconds in one frame
	private const double BUFFER_DELAY = 30.0 * FRAME_LENGTH; // Forces a delay of this length before starting the music
	private const double SIXTYFOUR_NOTE = 0.0625;
	private const double BASE_OFFSET = 0.1; // Base visual delay

	public Track meta;
	public Text timer;
	private GameObject files; // SiteHandler
	private GameObject switcher; // PlaytestSwitcher

	private bool playbackStarted = false;

	private double startBeat;
	private double startTime;

	[Header("Used by SiteHandler - LEAVE THESE BLANK")]
	public double tempo; // Song speed in beats per minute
	public double beatsPerSec; // How many beats in one second <- Public for Judgment
	public double beatsElapsed = 0.0; // Song position in beats
	public double beatsElapsedDelta = 0.0;
	public double beatZeroOffset; // Chart-determined visual delay
	public double userOffset; // User-determined visual delay

	private bool pastSchedule = false;
	private double songStart; // DSP time reference point for beginning of playback
	private double secPerBeat; // How many seconds in one beat
	private double timeElapsed = 0.0; // Song position based on DSP time's original reference point and current point in time
	private double timeElapsedLast = 0.0;
	private double timeElapsedDelta = 0.0; // DSP time elapsed since the last frame
	//private double overtime;
	private double negativeDelay = 0.0;
	private int tempoIndex = 0;

	/*
		Initialize timekeepers.	
	*/
	
	void Awake()
	{		
		files = GameObject.Find("SiteHandler");
		
		if (files.GetComponent<SiteHandler>().webMode)
		{
			GetComponent<AudioSource>().clip = files.GetComponent<SiteHandler>().audioFile;
		}

		else
		{
			GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(files.GetComponent<SiteHandler>().audioURL);
		}

		userOffset = files.GetComponent<SiteHandler>().userOffset;

		// ---

		switcher = GameObject.Find("PlaytestSwitcher");

		if (switcher != null)
		{
			startBeat = switcher.GetComponent<PlaytestSwitcher>().currentBeat;		
		}
	}

	void Start()
	{		
		UpdateRates();
	}

	/*
		Calculate what beat we're on using tempo and time elapsed, relative to when the song started playing according to the sound system.
	*/

	void Update()
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
		
		// ステップ through every tempo change and increment the correct amount of time!

		double anyTime = 0.0;
		startTime = 0.0;

		// Step through song forwards
		if (startBeat >= 0.0)
		{
			for (double i = 0.0; i < startBeat; i += SIXTYFOUR_NOTE)
			{
				if ((tempoIndex < meta.json.tempo_change_beat.Length) && (i >= meta.json.tempo_change_beat[tempoIndex]))
				{
					tempo = meta.json.tempo_change_amount[tempoIndex];
					tempoIndex++;
				}

				secPerBeat = SEC_PER_MIN / tempo / 16.0;
				anyTime += secPerBeat;

				beatsElapsed = i;
			}
		}

		// Step through song backwards
		else
		{
			/*
				-6: "Are you ready?"
				-4: "Three!"
				-3: "Two!"
				-2: "One!"
				-1: "GO!"
			*/

			//beatsElapsed = startBeat;

			for (double i = 0.0; i >= startBeat; i -= SIXTYFOUR_NOTE)
			{
				tempo = meta.json.tempo_change_amount[0];

				secPerBeat = SEC_PER_MIN / tempo / 16.0;
				anyTime -= secPerBeat;

				beatsElapsed = i;
			}
		}

		startTime = anyTime;
		//Debug.Log("[Metronome] startTime: " + startTime);		

		if (startTime < 0.0)
		{

			startTime = 0.0;
			negativeDelay = -anyTime;
		}

		//Debug.Log("[Metronome] negativeDelay: " + negativeDelay);	
	}

	/*
		Update the timer based on the arbitrary start point.
	*/

	public bool PlayButton()
	{
		return (Input.GetKeyDown(KeyCode.P) || Input.GetKey(KeyCode.Mouse0));
	}

	public void UpdateTimeAnywhere()
	{
		timer.text = "DSP: " + timeElapsed.ToString() + "\n"
			+ "Clip: " + GetComponent<AudioSource>().time;

		if (!playbackStarted)
		{
			if (PlayButton())
			{
				StartSongAnywhere();
				playbackStarted = true;
			}
		}

		else
		{
			if (!pastSchedule)
			{
				GetComponent<AudioSource>().time = (float)(startTime + beatZeroOffset);

				// ---

				songStart = AudioSettings.dspTime + startTime; // REQUIRED - DO NOT CHANGE OR MOVE
				timeElapsedLast = AudioSettings.dspTime - songStart + startTime;

				pastSchedule = true;

				GetComponent<AudioSource>().PlayScheduled(AudioSettings.dspTime + BUFFER_DELAY + negativeDelay);
			}

			else
			{
				timeElapsed = AudioSettings.dspTime - songStart + startTime;

				if (timeElapsed >= (BASE_OFFSET + userOffset + BUFFER_DELAY))
				{
					beatsElapsed += beatsElapsedDelta;
				}

				timeElapsedDelta = timeElapsed - timeElapsedLast;
				timeElapsedLast = timeElapsed;
			}
		}
	}

	/*
		Get song metadata from JSON file.
	*/

	private void GetSongData()
	{
		tempo = meta.json.tempo_change_amount[0];
		beatZeroOffset = meta.json.offset;
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
}