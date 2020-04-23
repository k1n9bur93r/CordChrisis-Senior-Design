using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/*
	> ResultsManager class

	Watches over the state of a game in play mode.
	Detects song endings and changes scenes as appropriate.
*/

public class ResultsManager : MonoBehaviour
{
	private const int MAX_KEYS = 4;
	private const int MAX_GESTURES = 4;
	
	private GameObject metronome;
	private GameObject noteSpawner;
	private GameObject scoreboard;
	
	private JudgeContainer stats;
	private bool songFinished = false;

	private double score;
	private int notesMarvelous, notesPerfect, notesGood, notesMiss;
	private int notesEarly, notesLate;
	private int comboMax;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions
	}

	void Start()
	{
		GameObject files = GameObject.Find("SiteHandler");

		if (!files.GetComponent<SiteHandler>().gameMode)
		{
			gameObject.SetActive(false);
		}

		metronome = GameObject.Find("Metronome");
		noteSpawner = GameObject.Find("NoteSpawner");
	}

	void Update()
	{
		if (!songFinished)
		{
			CheckRemainingNotes();
		}
	}

	private void CheckRemainingNotes()
	{
		bool tapsEmpty = true;
		bool holdsEmpty = true;
		bool gesturesEmpty = true;

		for (int i = 0; i < MAX_KEYS; i++)
		{
			if (noteSpawner.GetComponent<NoteSpawner>().notes[i].Count > 0) { tapsEmpty = false; break; }
			if (noteSpawner.GetComponent<NoteSpawner>().gestures[i].Count > 0) { holdsEmpty = false; break; }
		}

		if (noteSpawner.GetComponent<NoteSpawner>().holds.Count > 0) { gesturesEmpty = false; }

		// ---

		if (tapsEmpty && holdsEmpty && gesturesEmpty)
		{
			//Debug.Log("Song finished!");
			songFinished = true;
			StartCoroutine(TransitionResults());
		}
	}

	private IEnumerator TransitionResults()
	{
		// Display clear graphic
			// ...

		// Get stats
		scoreboard = GameObject.Find("MyCanvas");
		stats = scoreboard.GetComponent<Scoreboard>().stats;

		/*
		score = scoreboard.GetComponent<Scoreboard>().score;
		notesMarvelous = scoreboard.GetComponent<Scoreboard>().notesMarvelous;
		notesPerfect = scoreboard.GetComponent<Scoreboard>().notesPerfect;
		notesGood = scoreboard.GetComponent<Scoreboard>().notesGood;
		notesMiss = scoreboard.GetComponent<Scoreboard>().notesMiss;
		comboMax = scoreboard.GetComponent<Scoreboard>().comboMax;
		notesEarly = scoreboard.GetComponent<Scoreboard>().notesEarly;
		notesLate = scoreboard.GetComponent<Scoreboard>().notesLate;
		*/

		// Wait before changing scenes
		yield return new WaitForSecondsRealtime(3.0f);

		// Change scenes
		//SceneManager.LoadScene("ResultsScreen", LoadSceneMode.Single);
		//Initiate.Fade("ResultsScreen", Color.black, 1.75f);
		StartCoroutine(LoadResultsScreen());

		//Initiate.Fade("ResultsScreen", Color.black, 5.0f);
		//DrawResults();
	}

	// ISSUE: Need a way to asynchronously load the next scene AND do the screen fade

	private IEnumerator LoadResultsScreen()
	{
		AsyncOperation nextScene = SceneManager.LoadSceneAsync("ResultsScreen", LoadSceneMode.Single);

		while (!nextScene.isDone)
		{
			yield return null;
		}

		DrawResults();

		yield return new WaitForEndOfFrame();
	}

	private void DrawResults()
	{
		GameObject letterGrade = GameObject.Find("LetterGrade");
		GameObject clearType = GameObject.Find("ClearType");
		GameObject scoreText = GameObject.Find("ScoreText");
		GameObject judgeType = GameObject.Find("JudgeType");
		GameObject judgeCount = GameObject.Find("JudgeCount");

		// ---

		string finalLetterGrade;

		if (stats.score >= 1000000) { finalLetterGrade = "SSS"; }
		else if (stats.score >= 990000) { finalLetterGrade = "SS"; }
		else if (stats.score >= 950000) { finalLetterGrade = "S"; }
		else if (stats.score >= 900000) { finalLetterGrade = "A"; }
		else if (stats.score >= 800000) { finalLetterGrade = "B"; }
		else if (stats.score >= 700000) { finalLetterGrade = "C"; }
		else { finalLetterGrade = "D"; }

		letterGrade.GetComponent<TextMeshProUGUI>().text = finalLetterGrade;

		// ---

		string finalClearType;

		if (stats.notesMiss == 0)
		{
			if (stats.notesGood == 0)
			{
				if (stats.notesPerfect == 0)
				{
					finalClearType = "Perfect Full Combo!";
				}

				else
				{
					finalClearType = "Full Combo!";
				}
			}

			else
			{
				finalClearType = "Full Combo!";
			}
		}

		else
		{
			if (stats.score >= 700000)
			{
				finalClearType = "Clear!";
			}

			else
			{
				finalClearType = "Failed...";
			}
		}

		clearType.GetComponent<TextMeshProUGUI>().text = finalClearType;

		// ---

		scoreText.GetComponent<TextMeshProUGUI>().text = stats.score.ToString("000,000");
		
		// ---

		judgeType.GetComponent<TextMeshProUGUI>().text = "Perfect\nGreat\nGood\nMiss\nMax Combo";

		judgeCount.GetComponent<TextMeshProUGUI>().text =
			stats.notesMarvelous + "\n"
			+ stats.notesPerfect + "\n"
			+ stats.notesGood + "\n"
			+ stats.notesMiss + "\n"
			+ stats.comboMax;
	}
}