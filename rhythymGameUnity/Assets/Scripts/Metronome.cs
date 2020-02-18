using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
	private const double SEC_PER_MIN = 60.0; // 60 seconds per minute

	public double tempo;
	public double songDelay; // Time before audio starts

	public GameObject visualTicker;

	public Text tempoText;
	public Text timeElapsedText;
	public Text beatsElapsedText;
	//public Text secPerBeatText;

	/*private*/ public double secPerBeat; // How many seconds in one beat
	public double beatsPerSec; // How many beats in one second
	/*private*/ public double timeElapsed;
	private double timeElapsedDelta;
	/*private*/ public double beatsElapsed;

	/*
		Initialize all tickers to 0.
		Determine amount of seconds per beat.
	*/
	
	void Start()
	{
		beatsElapsed = 0.0;
		timeElapsed = 0.0;
		timeElapsedDelta = 0.0;

		UpdateRate();
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
		Call this to recalculate amount of seconds per beat to account for BPM changes.
	*/

	void UpdateRate()
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

		UpdateRate();
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