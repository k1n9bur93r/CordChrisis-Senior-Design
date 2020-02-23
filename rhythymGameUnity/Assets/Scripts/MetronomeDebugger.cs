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

	private bool ticked;
	private int currentPos;
	private int lastPos;

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

		if ((tickerBeat < 1.0f) && (currentPos != 0))
		{
			transform.position = new Vector3(-3.0f, 0.0f, 5.0f);
			currentPos = 0;
			GetComponent<AudioSource>().Play();
		}

		else if ((tickerBeat >= 1.0f) && (tickerBeat < 2.0f) && (currentPos != 1))
		{
			transform.position = new Vector3(-1.0f, 0.0f, 5.0f);
			currentPos = 1;
			GetComponent<AudioSource>().Play();
		}

		else if ((tickerBeat >= 2.0f) && (tickerBeat < 3.0f) && (currentPos != 2))
		{
			transform.position = new Vector3(1.0f, 0.0f, 5.0f);
			currentPos = 2;
			GetComponent<AudioSource>().Play();
		}

		else if ((tickerBeat >= 3.0f) && (tickerBeat < 4.0f) && (currentPos != 3))
		{
			transform.position = new Vector3(3.0f, 0.0f, 5.0f);
			currentPos = 3;
			GetComponent<AudioSource>().Play();
		}

		/*
		else
		{
			Debug.Log("ERROR: Metronome debugger fell through! tickerBeat = " + tickerBeat + " | currentPos = " + currentPos);
		}
		*/
	}

	/*
		Print debugging text.
	*/

	private void PrintStats()
	{
		tempoText.text = "Tempo: " + master.tempo + " (" + master.secPerBeat + " sec/beat)";
		timeElapsedText.text = "Time: " + master.getTimeElapsedDEBUG();//Time.time;
		beatsElapsedText.text = "Beat: " + master.beatsElapsed;
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
	}
}
