﻿using System.Collections;
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
		double songDelay: Measurement (in seconds) of how much silence there is in the audio file before audio starts playing.
		double beatsElapsed: Current position in the song (in number of beats).

	Important public methods:
		- None yet, maybe
*/

public class Metronome : MonoBehaviour
{
	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute

	public double tempo;
	public double songDelay; // Time before audio starts, determine via AudioSettings.dspTime later

	public GameObject visualTicker;

	public Text tempoText;
	public Text timeElapsedText;
	public Text beatsElapsedText;
	//public Text secPerBeatText;

	public double secPerBeat; // How many seconds in one beat
	public double beatsPerSec; // How many beats in one second
	private double timeElapsed; // Position in seconds
	private double timeElapsedDelta;
	public double beatsElapsed; // Position in beats

	/*
		Initialize all tickers to 0.
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
		ChangeTempo();

        // These will have to be determined via (AudioSettings.dspTime - songDelay) once we get music-playing going
		timeElapsed = Time.time; //Time.time; //AudioSettings.dspTime;
		timeElapsedDelta = Time.deltaTime;

		//beatsElapsed = timeElapsed / secPerBeat; // Reminder to avoid this, this would completely change what beat we're on if the BPM changes
		beatsElapsed += timeElapsedDelta / secPerBeat;

		DrawTicker();
		PrintStats();
	}

	/*
		Call this to recalculate tick rates.
	*/

	void UpdateRates()
	{
		secPerBeat = SEC_PER_MIN / tempo;
		beatsPerSec = tempo / SEC_PER_MIN;
	}

	/*
		Debugging tool. Changes song BPM.
	*/

	void ChangeTempo()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			tempo += 20;
		}

		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			tempo -= 20;
		}

		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			tempo -= 1;
		}

		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			tempo += 1;
		}

		UpdateRates();
	}

	/*
		Print debugging text.
	*/

	void PrintStats()
	{
		tempoText.text = "Tempo: " + tempo + " (" + secPerBeat + " sec/beat)";
		timeElapsedText.text = "Time: " + timeElapsed;
		beatsElapsedText.text = "Beat: " + beatsElapsed;
		//secPerBeatText.text = "Sec/Beat: " + secPerBeat;
	}

	/*
		Draw the metronome's visual aid.
	*/

	void DrawTicker()
	{
		float tickerBeat = (Mathf.Repeat((float)beatsElapsed, 4.0f)); // WHY IS % NOT MODULO
		//Debug.Log("tickerBeat: " + tickerBeat);

		if (tickerBeat < 1.0f)
		{
			visualTicker.transform.position = new Vector3(-3.0f, 0.0f, 5.0f);
		}

		else if ((tickerBeat >= 1.0f) && (tickerBeat < 2.0f))
		{
			visualTicker.transform.position = new Vector3(-1.0f, 0.0f, 5.0f);
		}

		else if ((tickerBeat >= 2.0f) && (tickerBeat < 3.0f))
		{
			visualTicker.transform.position = new Vector3(1.0f, 0.0f, 5.0f);
		}

		else if ((tickerBeat >= 3.0f) && (tickerBeat < 4.0f))
		{
			visualTicker.transform.position = new Vector3(3.0f, 0.0f, 5.0f);
		}

		else
		{
			Debug.Log("ERROR: Metronome visual aid fell through!");
		}
	}
}