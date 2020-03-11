using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour
{

    // UI Text Variables
    public TextMeshPro score3d;
    public TextMeshPro streak3d;
    public TextMeshPro mult3d;
    public TextMeshPro status3d;

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

    // Start is called before the first frame update
    void Start()
    {
        pointValue = new int[5] { 0, 1, 2, 3, 4 };
        rating = new string[5] { "MISS", "BAD", "GOOD", "EXCELLENT", "MARVELOUS" };
        score3d = GameObject.Find("Score3dp").GetComponent<TextMeshPro>();
        streak3d = GameObject.Find("Streak3dp").GetComponent<TextMeshPro>();
        mult3d = GameObject.Find("Mult3dp").GetComponent<TextMeshPro>();
        status3d = GameObject.Find("Status3dp").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    public void Action() //Update()
    {
        // ...
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


    // UpdateScoreHold is to score notes that are held
    // -int acc = acurracy rating (0-4)
    // -float te = time expected
    // -float th = time held
    public void UpdateScoreHold(int acc, double te, double th)
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

    // textUpdate updates the actual scoreboard
    // - int acc - accuracy rating
    // - Private function should be called by this class
    private void textUpdate(int acc)
    {
        status3d.text = rating[acc];
        score3d.text = "Score: " + globalScore.ToString();
        streak3d.text = "Streak: " + combo.ToString();
        mult3d.text = "Mult: x" + (combo / 10 + 1).ToString();
    }
}
