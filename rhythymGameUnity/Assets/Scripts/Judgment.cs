using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Judgment : MonoBehaviour
{
	public Metronome master;
	public Text judgmentText;
	public Text judgmentHelpText;

	private const double framesJustCrisis = 1.0 / 60.0; // 1 frame early/late, assuming 60 FPS, so 1/60th of a second of earliness and lateness
	private const double framesCrisis = 2.0 / 60.0; // 2 frames early/late
	private const double framesGreat = 5.0 / 60.0; // 5 frames early/late
	private const double framesGood = 10.0 / 60.0; // 10 frames early/late

	double beatsJustCrisis;
	double beatsCrisis;
	double beatsGreat;
	double beatsGood;

	void Start()
	{
		judgmentText.text = "Press SPACE on beat 8.0!";
		CalculateWindows();
	}

	void Update()
	{
		CalculateWindows();
		JudgeTiming();
	}

	/*
		Determine how many beats are in each timing window.
	*/

	void CalculateWindows()
	{
		beatsJustCrisis = master.beatsPerSec * framesJustCrisis;
		beatsCrisis = master.beatsPerSec * framesCrisis;
		beatsGreat = master.beatsPerSec * framesGreat;
		beatsGood = master.beatsPerSec * framesGood;

		UpdateHelpText();
	}

	/*
		Update the help information
	*/

	void UpdateHelpText()
	{
		judgmentHelpText.text =
			"Fantastic: +/- " + (float)beatsJustCrisis + " beats (+/- 0.016 sec)\n"
			+ "Excellent: +/- " + (float)beatsCrisis + " beats (+/- 0.033 sec)\n"
			+ "Great: +/- " + (float)beatsGreat + " beats (+/- 0.083 sec)\n"
			+ "Good: +/- " + (float)beatsGood + " beats (+/- 0.166 sec)\n"
			+ "Attempting to hit a note beyond the [Good] window is either not counted (early) or missed (late)";
	}

	/*
		Debug function to compare timing of a key press versus the metronome.
	*/

	void JudgeTiming()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			double currentBeat = Time.time / master.secPerBeat; // Compensate for song start offset and dsp stuff later
			double noteBeat = 8.0; // Placeholder until note-reading process is figured out

			float diff = (float)(currentBeat - noteBeat);

			if (diff < -beatsGood) //(diff < -framesGood)
			{
				judgmentText.text = "Way too early!";
			}

			else
			{
				if (Mathf.Abs(diff) <= beatsJustCrisis) { judgmentText.text = "Fantastic timing!"; }
				else if (Mathf.Abs(diff) <= beatsCrisis) { judgmentText.text = "Excellent timing!"; }
				else if (Mathf.Abs(diff) <= beatsGreat) { judgmentText.text = "Great timing!"; }
				else if (Mathf.Abs(diff) <= beatsGood) { judgmentText.text = "Good timing!"; }
				else { judgmentText.text = "Way too late!"; }
			}

			judgmentText.text = 
				judgmentText.text + "\n" + diff + " beats\n"
				+ (diff * (float)master.secPerBeat) + " sec";
		}
	}
}