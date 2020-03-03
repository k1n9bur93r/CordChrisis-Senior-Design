using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> MetronomeDebugger class

	Debugging tools for the Metronome class.

	Draws these visual aids:
		Aid 1: A green shape that changes position with every beat that occurs.
		Aid 2: A red shape that scrolls to the left towards a white shape. Simulates note scrolling and the receptor, respectively.

	Displays current beat, tempo, and time.
	Tempo can be changed directly using the arrow keys.

	Important public variables:
		- float speedMod: Affects scroll speed of visual aid 2.
*/

public class MetronomeDebugger : MonoBehaviour
{
	private const float NOTE_PADDING = 1.0f;

	public Metronome master;
	public GameObject note;
	public GameObject receptor;

	public Text tempoText;
	public Text timeElapsedText;
	public Text beatsElapsedText;

	public float speedMod;

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
		DrawNote();
		PrintStats();
		ChangeTempo();
	}

	/*
		Draw the visual aid 1: a ticker that changes position with every beat.
	*/

	private void DrawTicker()
	{
		float tickerBeat = (Mathf.Repeat((float)master.beatsElapsed, 4.0f)); // WHY IS % NOT MODULO
		//Debug.Log("tickerBeat: " + tickerBeat);

		if ((tickerBeat < 1.0f) && (currentPos != 0))
		{
			//Debug.Log(master.beatsElapsed);
			transform.position = new Vector3(0.5f, 2.0f, 5.0f);
			currentPos = 0;
			GetComponent<AudioSource>().Play();
		}

		else if ((tickerBeat >= 1.0f) && (tickerBeat < 2.0f) && (currentPos != 1))
		{
			//Debug.Log(master.beatsElapsed);
			transform.position = new Vector3(1.5f, 2.0f, 5.0f);
			currentPos = 1;
			GetComponent<AudioSource>().Play();
		}

		else if ((tickerBeat >= 2.0f) && (tickerBeat < 3.0f) && (currentPos != 2))
		{
			//Debug.Log(master.beatsElapsed);
			transform.position = new Vector3(2.5f, 2.0f, 5.0f);
			currentPos = 2;
			GetComponent<AudioSource>().Play();
		}

		else if ((tickerBeat >= 3.0f) && (tickerBeat < 4.0f) && (currentPos != 3))
		{
			//Debug.Log(master.beatsElapsed);
			transform.position = new Vector3(3.5f, 2.0f, 5.0f);
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
		Draw visual aid 2: a mockup note at beat 16.0.

		"Note location" refers to where in the chart the note exists, in beats.
		"Speed mod" refers to a user-selected (public) note spacing setting, effectively changing scroll speed and overall note density.
		"Padding" refers to an arbitrary constant to space the notes further. Not user-selectable (private).

		The note is drawn at a distance relative to the receptor's position, modified by beats elapsed, tempo, speed mod, and padding.
		When the metronome's current location equals the notes's location, the note will land exactly on top of the receptor.
	*/

	private void DrawNote()
	{
		// Note position formula: Receptor position + ((Note location - Beats elapsed) * Speed mod * Padding))

		note.transform.position = 
			new Vector3(receptor.transform.position.x + ((16.0f - (float)master.beatsElapsed) * speedMod * NOTE_PADDING),
			receptor.transform.position.y, receptor.transform.position.z);
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
