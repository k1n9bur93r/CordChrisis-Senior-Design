using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
	> GameManager class

	Watchdog class to check what section of the game we're on and to run timing-sensitive classes.
	Wakes up relevant classes and plays relevant Update()-like functions in a set order.

	ISSUES:
		- Need to refactor how the public objects are found due to not having a reference to them during a scene change

	REFERENCE MATERIALS:
		- https://bemuse.ninja/project/docs/game-loop.html
		- https://forum.unity.com/threads/how-to-avoid-execution-order-nightmares.517578/
*/
public class GameManager : MonoBehaviour
{
	enum GameState { Play, Results, None };
	private const int MAX_RECEPTORS = 4;

	// GameObject versions of the classes <- need to find a way to activate things without doing stuff like this
	public GameObject metaGO; // Track
	public GameObject clockGO; // Metronome

	// Actual classes
	public Metronome clock; // Metronome
	public InputController[] receptor; // All receptors
	//public Scoreboard score; // Scoreboard
	public NoteSpawner spawner; // NoteSpawner

	private GameState mode;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions
		mode = GameState.None;
	}

	void Update()
	{
		switch (mode)
		{
			case GameState.Play:
				PlayModeLoop();
				break;

			case GameState.Results:
				break;

			case GameState.None:
				NoneModeLoop();
				break;

			default:
				Debug.Log("[GameManager] Game state fell through!");
				break;
		}
	}

	/*
		Play screen initialization and update loop
	*/

	private void PlayModeLoop()
	{
		// Timers and audio
		clock.Action();

		// Input
		for (int i = 0; i < MAX_RECEPTORS; i++)
		{
			receptor[i].Action();
		}

		// Visuals
		spawner.Action();
	}

	private void PlayModeInit()
	{
		//SceneManager.LoadScene("Main Game");
		metaGO.SetActive(true);
		clockGO.SetActive(true);
		clock.StartSong();
	}

	/*
		Results screen initialization and update loop
	*/

	private void ResultsModeLoop()
	{
		// ...
	}

	/*
		Inactive initialization and update loop
	*/

	private void NoneModeLoop()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			PlayModeInit();
			mode = GameState.Play;
		}
	}
}