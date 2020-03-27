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
	private const double ACC_SCORE_MAX = 900000;
	private const double COMBO_SCORE_MAX = 100000;
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

	//private double baseAccValue;
	//private double baseComboValue;

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

		/*
		baseAccValue = ACC_SCORE_MAX / (double)maxNotesTemp;
		baseComboValue = COMBO_SCORE_MAX * (1.0 / ((double)maxNotesTemp - 1.0));
		*/

		//Debug.Log("acc:" + baseAccValue);
		//Debug.Log("com:" + baseComboValue);
	}

	void Start()
	{
		//pointValue = new int[5] { 0, 1, 2, 3, 4 };
		//rating = new string[5] { "Miss", "Good", "Great!", "Excellent!!", "Marvelous!!!" };
		//ratingAnim = GameObject.Find("RatingText").GetComponent<Animator>();
		//scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshPro>();
		//streakText = GameObject.Find("StreakText").GetComponent<TextMeshPro>();
		//multText = GameObject.Find("MultText").GetComponent<TextMeshPro>();
		//ratingText = GameObject.Find("RatingText").GetComponent<TextMeshPro>();

	}

	// Update is called once per frame
	public void Update()
	{
		DrawScore();
	}

	private void DrawScore()
	{
		scoreDisplayed = (int)Mathf.Lerp((float)scoreDisplayed, (float)score, 20.0f * Time.deltaTime);
		scoreText.text = (scoreDisplayed).ToString("000,000");
	}

	public void UpdateScore(Ratings rate, Leanings lean)
	{
		double baseAccValue = ACC_SCORE_MAX / (double)meta.noteTotal;
		double baseComboValue = COMBO_SCORE_MAX / (double)meta.noteTotal; //COMBO_SCORE_MAX * (1.0 / ((double)meta.noteTotal - 1.0));

		AnimateRating(rate);
		AnimateCombo();

		// Accuracy scoring
		switch (rate)
		{
			case Ratings.Marvelous:
				score += baseAccValue;
				notesMarvelous++;
				combo++;
				break;

			case Ratings.Perfect:
				score += baseAccValue * 0.99;
				notesPerfect++;
				combo++;
				break;

			case Ratings.Great:
				score += baseAccValue * 0.66;
				notesGreat++;
				combo++;
				break;
		
			case Ratings.Good:
				score += baseAccValue * 0.33;
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

		// Negative combo scoring
		SetNegativeCombo(rate);
		
		if (negativeCombo >= 0)
		{
			score += baseComboValue;
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

		else
		{
			if (negativeCombo >= 0)
			{
				negativeCombo = -1;
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

		if (rate == Ratings.Marvelous)
		{
			// rainbow texture
		}

		else
		{
			// no texture
		}

		ratingText.text = RATING_NAMES[(int)rate];
		ratingText.fontMaterial = ratingColors[(int)rate];
	}

	private void AnimateCombo()
	{
		// DEBUG
		//streakText.text = negativeCombo.ToString() + " Combo";

		if (combo > 0)
		{
			if ((notesPerfect == 0) && (notesGreat == 0) && (notesGood == 0) && (notesMiss == 0)) { streakText.fontMaterial = ratingColors[6]; }
			else if ((notesGreat == 0) && (notesGood == 0) && (notesMiss == 0)) { streakText.fontMaterial = ratingColors[(int)Ratings.Perfect]; }
			else if ((notesGood == 0) && (notesMiss == 0)) { streakText.fontMaterial = ratingColors[(int)Ratings.Great]; }
			else if (notesMiss == 0) { streakText.fontMaterial = ratingColors[(int)Ratings.Good]; }
			else { streakText.fontMaterial = ratingColors[5]; }

			streakText.text = combo.ToString();
		}

		else
		{
			streakText.text = "";
		}
	}
}
