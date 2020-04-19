using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Ratings { Miss, Good, Perfect, Marvelous };
public enum Leanings { None, Early, Late };

/*
	> Scoreboard class

	Displays play mode UI and calculates score.
*/

public struct JudgeContainer
{
	public double score;
	public int notesMarvelous, notesPerfect, notesGood, notesMiss;
	public int notesEarly, notesLate;
	public int comboMax;
}

public class Scoreboard : MonoBehaviour
{
	//private const double ACC_SCORE_MAX = 800000;
	//private const double COMBO_SCORE_MAX = 200000;
	private const double MAX_SCORE = 1000000.0;
	private readonly string[] RATING_NAMES = { "MISS", "GOOD", "GREAT", "PERFECT" };
	private readonly string[] LEAN_NAMES = { " ", "EARLY", "LATE" };

	// Other classes
	public Track meta;
	private GameObject files; // SiteHandler

	// UI Text Variables
	public TextMeshPro scoreText;
	public TextMeshPro streakText;
	public TextMeshPro multText;
	public TextMeshPro ratingText;
	public TextMeshPro leanText;

	public Animator ratingAnim;
	public Material[] ratingColors;

	private int scoreDisplayed;

	// Statistics for scoring
	public JudgeContainer stats;
	private bool scoreEnabled = true;
	//private int notesMarvelous, notesPerfect, notesGood, notesMiss;
	//private int notesEarly, notesLate;
	//private int combo, negativeCombo, comboMax;
	//private double score;
	private int combo, negativeCombo;

	double baseNoteValue;
	double[] ratingValues;

	void Awake()
	{
		stats.notesMarvelous = 0;
		stats.notesPerfect = 0;
		stats.notesGood = 0;
		stats.notesMiss = 0;
		combo = 0;
		negativeCombo = 0;
		stats.comboMax = 0;
		stats.score = 0;
		scoreDisplayed = 0;

		ratingText.text = "";
		leanText.text = "";
		streakText.text = "";

		// ---

		files = GameObject.Find("SiteHandler");

		if (!files.GetComponent<SiteHandler>().gameMode)
		{
			scoreEnabled = false;
			scoreText.text = "";
		}
	}

	void Start()
	{
		baseNoteValue = MAX_SCORE / (double)meta.noteTotal;

		ratingValues = new double[4] {
			0, // Miss
			baseNoteValue * 0.5, // Good
			baseNoteValue, // Perfect
			baseNoteValue + 1  // Marvelous
		};
	}

	// Update is called once per frame
	public void Update()
	{
		if (scoreEnabled)
		{
			DrawScore();
		}
	}

	private void DrawScore()
	{
		//scoreDisplayed = (int)Mathf.Lerp((float)scoreDisplayed, (float)score, 16.0f * Time.deltaTime);
		scoreDisplayed = (int)Mathf.MoveTowards((float)scoreDisplayed, (float)stats.score, 8.0f * (float)baseNoteValue * Time.deltaTime);
		scoreText.text = (scoreDisplayed).ToString("000,000");
	}

	public void UpdateScore(Ratings rate, Leanings lean)
	{
		// Accuracy scoring
		switch (rate)
		{
			case Ratings.Marvelous:
				stats.notesMarvelous++;
				combo++;
				break;

			case Ratings.Perfect:
				stats.notesPerfect++;
				combo++;
				break;
		
			case Ratings.Good:
				stats.notesGood++;
				combo++;
				break;

			case Ratings.Miss:
				stats.notesMiss++;
				combo = 0;
				break;

			default:
				Debug.Log("[Scoreboard] UpdateScore() accuracy fell through!");
				break;
		}

		// Early/Late
		switch (lean)
		{
			case Leanings.Early:
				//leanText.text = "EARLY";
				stats.notesEarly++;
				break;

			case Leanings.Late:
				//leanText.text = "LATE";
				stats.notesLate++;
				break;

			case Leanings.None:
				leanText.text = "";
				break;

			default:
				Debug.Log("[Scoreboard] UpdateScore() lean fell through!");
				break;
		}

		AnimateRating(rate, lean);

		// Negative combo scoring
		//SetNegativeCombo(rate);
		AnimateCombo();

		// Calculate score
		
		if (negativeCombo < 0)
		{
			stats.score += ratingValues[(int)rate] * 0.5;
		}

		else
		{
			stats.score += ratingValues[(int)rate];
		}
	}

	private void AnimateRating(Ratings rate, Leanings lean)
	{
		ratingAnim.ForceStateNormalizedTime(0.0f);

		ratingText.text = RATING_NAMES[(int)rate];
		leanText.text += LEAN_NAMES[(int)lean];

		/*
		// Draw lean on ratings
		if (lean == Leanings.None)
		{
			ratingText.text = RATING_NAMES[(int)rate];
		}

		else if (lean == Leanings.Early)
		{
			ratingText.text = "-";
			ratingText.text += RATING_NAMES[(int)rate];
			ratingText.text += " ";
		}

		else if (lean == Leanings.Late)
		{
			ratingText.text = " ";
			ratingText.text += RATING_NAMES[(int)rate];
			ratingText.text += "-";
		}
		*/

		ratingText.fontMaterial = ratingColors[(int)rate];
	}

	private void AnimateCombo()
	{
		// DEBUG
		//streakText.text = negativeCombo.ToString() + " Combo";

		if (combo >= 3)
		{
			if (stats.notesMiss > 0)
			{
				streakText.fontMaterial = ratingColors[4];
			}

			else
			{
				if ((stats.notesPerfect == 0) && (stats.notesGood == 0)) { streakText.fontMaterial = ratingColors[5]; } // Unbroken Marvelous
				else if ((stats.notesGood == 0)) { streakText.fontMaterial = ratingColors[(int)Ratings.Perfect]; } // Unbroken Perfect
				else { streakText.fontMaterial = ratingColors[(int)Ratings.Good]; } // Unbroken Good
			}

			streakText.text = combo.ToString();
		}

		else
		{
			streakText.text = "";
		}
	}

	private void SetNegativeCombo(Ratings rate)
	{
		if (rate == Ratings.Miss)
		{
			negativeCombo++;
		}

		else
		{
			if (negativeCombo > 0)
			{
				negativeCombo = 0;
			}

			else
			{
				negativeCombo--;
			}			
		}
	}
}
