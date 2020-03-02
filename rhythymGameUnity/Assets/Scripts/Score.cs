using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    private GameObject input;
    private int score;
    //NoteController s;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        input = GameObject.FindGameObjectWithTag("Note");
        //s = input.GetComponent<NoteController>();
    }

    // Update is called once per frame
    void Update()
    {
        //scoreText.text = "Score: " + s.score.ToString();
    }
}
