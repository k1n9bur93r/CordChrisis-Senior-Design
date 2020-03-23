using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Public Function to use
//  public void UpdateScore(int acc, double te, double th)
//  This function can handle scoring for hold or tap notes
//  <tap => UpdateScore(x,0,0);>
//


public class Scoreboard : MonoBehaviour
{

    // UI Text Variables
    public TextMeshPro scoreText;
    public TextMeshPro streakText;
    public TextMeshPro multText;
    public TextMeshPro ratingText;

    // Score points / rating
    public int[] pointValue;
    public string[] rating;
    

    // Statistics for scoring
    public int globalScore = 0;

    public int accuracy = 0;
    public int noteCount = 0;
    public int hitCount = 0;
    public int combo = 0;
    public int missCount = 0;

    private Animator ratingAnim;

    // Start is called before the first frame update
    void Start()
    {
        pointValue = new int[5] { 0, 1, 2, 3, 4 };
        rating = new string[5] { "MISS", "BAD", "GOOD", "EXCELLENT", "MARVELOUS" };
        ratingAnim = GameObject.Find("RatingText").GetComponent<Animator>();
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshPro>();
        streakText = GameObject.Find("StreakText").GetComponent<TextMeshPro>();
        multText = GameObject.Find("MultText").GetComponent<TextMeshPro>();
        ratingText = GameObject.Find("RatingText").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    /*
    public void //Update()
    {
        // ...
    }
    */


    // UpdateScore is to score notes that are held
    // -int acc = acurracy rating (0-4)
    // -float te = time expected
    // -float th = time held
    public void UpdateScore(int acc, double te, double th)
    {

        if (acc == 0)
        {
            missCount++;
            combo = 0;
        }
        else
        {
            combo++;
            if (th >= te)
            {
                globalScore += ((combo / 10 + 1) * pointValue[acc] * 100) + Mathf.RoundToInt((float)te) * 10;
            }
            else
            {
                globalScore += ((combo / 10 + 1) * pointValue[acc] * 100) + Mathf.RoundToInt((float)th) * 10;
            }
            textUpdate(acc);
        }
    }

    // UpdateScoreTap is used for scoring the single tap notes 
    // - int acc = accuracy rating (0-4)
    public void UpdateScoreTap(int acc)
    {

        if (acc == 0)
        {
            missCount++;
            combo = 0;
        }
        else
        {
            hitCount++;

            if (acc > 1)
            {
                combo++;
                globalScore += (combo / 10 + 1) * pointValue[acc] * 100;
            }
            else
            {
                combo = 0;
                globalScore += pointValue[acc] * 100;
            }

        }
        textUpdate(acc);
    }



    // textUpdate updates the actual scoreboard
    // - int acc - accuracy rating
    // - Private function should be called by this class
    private void textUpdate(int acc)
    {
        ratingText.text = rating[acc];
        scoreText.text = "Score: " + globalScore.ToString();
        streakText.text = "Streak: " + combo.ToString();
        multText.text = "Mult: x" + (combo / 10 + 1).ToString();

    }
}
