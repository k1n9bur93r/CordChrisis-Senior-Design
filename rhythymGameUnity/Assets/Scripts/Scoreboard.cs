using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Ratings { Miss, Good, Great, Perfect, Marvelous };
public enum Leanings { None, Early, Late };

/*
	[documentation here]
*/

public class Scoreboard : MonoBehaviour
{
	//private const double ACC_SCORE_MAX = 800000;
	//private const double COMBO_SCORE_MAX = 200000;
	private const double MAX_SCORE = 1000000.0;
	private readonly string[] RATING_NAMES = { "MISS", "GOOD", "GREAT", "PERFECT", "PERFECT" };

	// Other classes
	public Track meta;

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
	private int notesMarvelous, notesPerfect, notesGreat, notesGood, notesMiss;
	private int notesEarly, notesLate;
	private int combo, negativeCombo, comboMax;
	private double score;

	double baseNoteValue;
	double[] ratingValues;

	void Awake()
	{
		notesMarvelous = 0;
		notesPerfect = 0;
		notesGreat = 0;
		notesGood = 0;
		notesMiss = 0;
		combo = 0;
		negativeCombo = 0;
		comboMax = 0;
		score = 0;
		scoreDisplayed = 0;

		ratingText.text = "";
		leanText.text = "";
		streakText.text = "";
	}

	void Start()
	{
		baseNoteValue = MAX_SCORE / (double)meta.noteTotal;

		ratingValues = new double[5] {
			0, // Miss
			baseNoteValue * 0.3, // Good
			baseNoteValue * 0.7, // Great
			baseNoteValue, // Perfect
			baseNoteValue + 1.0  // Marvelous
		};
	}

	// Update is called once per frame
	public void Update()
	{
		DrawScore();
	}

	private void DrawScore()
	{
		scoreDisplayed = (int)Mathf.Lerp((float)scoreDisplayed, (float)score, 16.0f * Time.deltaTime);
		//scoreDisplayed = (int)Mathf.MoveTowards((float)scoreDisplayed, (float)score, 4.0f * (float)baseNoteValue * Time.deltaTime);
		scoreText.text = (scoreDisplayed).ToString("000,000");
	}

	public void UpdateScore(Ratings rate, Leanings lean)
	{
		// Accuracy scoring
		switch (rate)
		{
			case Ratings.Marvelous:
				notesMarvelous++;
				combo++;
				break;

			case Ratings.Perfect:
				notesPerfect++;
				combo++;
				break;

			case Ratings.Great:
				notesGreat++;
				combo++;
				break;
		
			case Ratings.Good:
				notesGood++;
				combo = 0;
				break;

			case Ratings.Miss:
				notesMiss++;
				combo = 0;
				break;

			default:
				Debug.Log("[Scoreboard] UpdateScore() accuracy fell through!");
				break;
		}

		AnimateRating(rate);

		// Negative combo scoring
		SetNegativeCombo(rate);
		AnimateCombo();

		// Calculate score
		
		if (negativeCombo < 0)
		{
			score += ratingValues[(int)rate] * 0.8;
		}

		else
		{
			score += ratingValues[(int)rate];
		}

		// Early/Late
		switch (lean)
		{
			case Leanings.Early:
				leanText.text = "EARLY";
				notesEarly++;
				break;

			case Leanings.Late:
				leanText.text = "LATE";
				notesLate++;
				break;

			case Leanings.None:
				leanText.text = "";
				break;

			default:
				Debug.Log("[Scoreboard] UpdateScore() lean fell through!");
				break;
		}
	}

	private void SetNegativeCombo(Ratings rate)
	{
		if (rate >= Ratings.Great)
		{
			negativeCombo++;
		}

		/*
		else if (rate == Ratings.Good)
		{
			// Nothing happens
		}
		*/

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

	private void AnimateRating(Ratings rate)
	{
		ratingAnim.ForceStateNormalizedTime(0.0f); // Deprecated function!

		ratingText.text = RATING_NAMES[(int)rate];
		ratingText.fontMaterial = ratingColors[(int)rate];
	}

	private void AnimateCombo()
	{
		// DEBUG
		//streakText.text = negativeCombo.ToString() + " Combo";

		if (combo > 0)
		{
			if ((notesGood > 0) || (notesMiss > 0))
			{
				/*
				if (negativeCombo < 0) { streakText.fontMaterial = ratingColors[7]; }
				else { streakText.fontMaterial = ratingColors[5]; }
				*/

				streakText.fontMaterial = ratingColors[5];
			}

			else
			{
				if ((notesPerfect == 0) && (notesGreat == 0) && (notesGood == 0)) { streakText.fontMaterial = ratingColors[6]; }
				else if ((notesGreat == 0) && (notesGood == 0)) { streakText.fontMaterial = ratingColors[(int)Ratings.Perfect]; }
				else { streakText.fontMaterial = ratingColors[(int)Ratings.Great]; }
			}

			streakText.text = combo.ToString();
		}

		else
		{
			streakText.text = "";
		}
	}
}
