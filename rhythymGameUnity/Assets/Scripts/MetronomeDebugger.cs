using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> MetronomeDebugger class

	Debugging tools for the Metronome class.
	
	Draws a box that changes position every time a beat occurs.
	Displays current beat, tempo, and time.
	Tempo can be changed directly using the arrow keys.
*/

public class MetronomeDebugger : MonoBehaviour
{
	public Metronome master;

	public Text tempoText;
	public Text timeElapsedText;
	public Text beatsElapsedText;

	void Start()
	{
		// ...
	}

	void Update()
	{
		DrawTicker();
		PrintStats();
		ChangeTempo();
	}

	/*
		Draw the visual aid.
	*/

	void DrawTicker()
	{
		float tickerBeat = (Mathf.Repeat((float)master.beatsElapsed, 4.0f)); // WHY IS % NOT MODULO
		//Debug.Log("tickerBeat: " + tickerBeat);

		if (tickerBeat < 1.0f)
		{
			transform.position = new Vector3(-3.0f, 0.0f, 5.0f);
		}

		else if ((tickerBeat >= 1.0f) && (tickerBeat < 2.0f))
		{
			transform.position = new Vector3(-1.0f, 0.0f, 5.0f);
		}

		else if ((tickerBeat >= 2.0f) && (tickerBeat < 3.0f))
		{
			transform.position = new Vector3(1.0f, 0.0f, 5.0f);
		}

		else if ((tickerBeat >= 3.0f) && (tickerBeat < 4.0f))
		{
			transform.position = new Vector3(3.0f, 0.0f, 5.0f);
		}

		else
		{
			Debug.Log("ERROR: Ticker fell through!");
		}
	}

	/*
		Print debugging text.
	*/

	private void PrintStats()
	{
		tempoText.text = "Tempo: " + master.tempo + " (" + master.secPerBeat + " sec/beat)";
		//timeElapsedText.text = "Time: " + master.timeElapsed; // Whoops it's private now
		timeElapsedText.text = "Time: " + Time.time; //(master.beatsElapsed * master.secPerBeat);
		beatsElapsedText.text = "Beat: " + master.beatsElapsed;
		//secPerBeatText.text = "Sec/Beat: " + secPerBeat;
	}

	/*
		Changes song BPM.
	*/

	private void ChangeTempo()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			master.tempo += 20;
		}

		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			master.tempo -= 20;
		}

		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			master.tempo -= 1;
		}

		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			master.tempo += 1;
		}

		master.UpdateRates();
	}
}
