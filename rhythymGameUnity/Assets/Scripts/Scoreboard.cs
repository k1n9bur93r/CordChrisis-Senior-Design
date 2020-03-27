using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Ratings { Miss, Good, Great, Perfect, Marvelous };
public enum Leanings { None, Early, Late };


// Public Function to use
//  public void UpdateScore(int acc, double te, double th)
//  This function can handle scoring for hold or tap notes
//  <tap => UpdateScore(x,0,0);>
//


public class Scoreboard : MonoBehaviour
{
	private const double ACC_SCORE_MAX = 900000;
	private const double COMBO_SCORE_MAX = 100000;
	//private const int maxNotesTemp = 178; // Placeholder until max notes can be determined!

	// Other classes
	public Track meta;

	// UI Text Variables
	public TextMeshPro scoreText;
	public TextMeshPro streakText;
	public TextMeshPro multText;
	public TextMeshPro ratingText;
	public TextMeshPro leanText;
	private Animator ratingAnim;

	// Score points / rating
	//public int[] pointValue;
	public string[] rating;	

	// Statistics for scoring
	private int notesMarvelous, notesPerfect, notesGreat, notesGood, notesMiss;
	private int notesEarly, notesLate;
	private int combo, negativeCombo, comboMax;
	private double score; // Cast this to an int when displaying it

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
		rating = new string[5] { "Miss", "Good", "Great!", "Excellent!!", "Marvelous!!!" };
		ratingAnim = GameObject.Find("RatingText").GetComponent<Animator>();
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
		scoreText.text = ((int)score).ToString("000,000");

		// ---

		// DEBUG
		//streakText.text = negativeCombo.ToString() + " Combo";

		if (combo > 0)
		{
			streakText.text = combo.ToString() + " Combo";
		}

		else
		{
			streakText.text = "";
		}
	}

	public void UpdateScore(Ratings rate, Leanings lean)
	{
		double baseAccValue = ACC_SCORE_MAX / (double)meta.noteTotal;
		double baseComboValue = COMBO_SCORE_MAX / (double)meta.noteTotal; //COMBO_SCORE_MAX * (1.0 / ((double)meta.noteTotal - 1.0));

		// Accuracy
		switch (rate)
		{
			case Ratings.Marvelous:
				ratingText.text = "Marvelous!!!";
				score += baseAccValue;
				notesMarvelous++;
				combo++;
				break;

			case Ratings.Perfect:
				ratingText.text = "Perfect!!";
				score += baseAccValue - 10.0;
				notesPerfect++;
				combo++;
				break;

			case Ratings.Great:
				ratingText.text = "Great!";
				score += baseAccValue * 0.7;
				notesGreat++;
				combo++;
				break;
		
			case Ratings.Good:
				ratingText.text = "Good";
				score += baseAccValue * 0.3;
				notesGood++;
				combo = 0;
				break;

			case Ratings.Miss:
				ratingText.text = "Miss...";
				notesMiss++;
				combo = 0;
				break;

			default:
				Debug.Log("[Scoreboard] UpdateScore() accuracy fell through!");
				break;
		}

		/*
		// Combo
		if (combo > 10)
		{
			score += baseComboValue;
		}
		*/

		// Negative combo
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

	// textUpdate updates the actual scoreboard
	// - int acc - accuracy rating
	// - Private function should be called by this class
	private void textUpdate(int acc)
	{
		/*
		ratingText.text = rating[acc];
		scoreText.text = "Score: " + globalScore.ToString();
		streakText.text = "Streak: " + combo.ToString();
		multText.text = "Mult: x" + (combo / 10 + 1).ToString();
		*/
	}
}
