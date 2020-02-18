using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Judgment : MonoBehaviour
{
	public Metronome master;
	public Text judgmentText;

	// There is no timing window for hitting a note more than 10 frames early
	private const double framesJustCrisis = 1.0 / 60.0; // 1 frame early/late, assuming 60 FPS, so 1/60th of a second of earliness and lateness
	private const double framesCrisis = 2.0 / 60.0; // 2 frames early/late
	private const double framesGreat = 5.0 / 60.0; // 5 frames early/late
	private const double framesGood = 10.0 / 60.0; // 10 frames early/late
	//private double timingMiss; // More than 10 frames late

	void Start()
	{
		judgmentText.text = "Press SPACE on beat 8.0!";
	}

	void Update()
	{
		JudgeTiming();
	}

	/*
		Debug function to compare timing of a key press versus the metronome.

		For the purposes of actually timing the player, compare beat.
		For the purposes of displaying the results, compare time.
	*/

	void JudgeTiming()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			double currentBeat = Time.time / master.secPerBeat; // Compensate for song start offset once needed
			float diff = (float)(currentBeat - 8.0); // 8.0 represents a note at this position for testing purposes

			PrintText(diff);

			//Debug.Log(Mathf.Abs((float)(currentTiming - master.beatsElapsed)));
		}
	}

	/*
		Update the information displayed onscreen.
	*/

	void PrintText(float diff)
	{
		diff = diff * (float)master.secPerBeat;

		if (diff < -framesGood)
		{
			judgmentText.text = "Way too early!";
		}

		else
		{
			if (Mathf.Abs(diff) <= framesJustCrisis) { judgmentText.text = "Extremely good timing!"; }
			else if (Mathf.Abs(diff) <= framesCrisis) { judgmentText.text = "Very good timing!"; }
			else if (Mathf.Abs(diff) <= framesGreat) { judgmentText.text = "Great timing!"; }
			else if (Mathf.Abs(diff) <= framesGood) { judgmentText.text = "Good timing!"; }
			else { judgmentText.text = "Way too late!"; }
		}

		judgmentText.text = judgmentText.text + "\n" + diff + " sec";
	}
}