using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
	> GameManager class

	Master game state handler.
	Changes scenes, wakes up objects, and executes them in the correct order as needed.

	ISSUES:
		- Need to refactor how the public objects are found due to not having a reference to them during a scene change

	REFERENCE MATERIALS:
		- https://bemuse.ninja/project/docs/game-loop.html
		- https://forum.unity.com/threads/how-to-avoid-execution-order-nightmares.517578/
*/
public class GameManager : MonoBehaviour
{
	enum GameState { Play, Results, Zero };
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
		mode = GameState.Zero;
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

			case GameState.Zero:
				ZeroModeLoop();
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

		mode = GameState.Play;
	}

	/*
		Results screen initialization and update loop
	*/

	private void ResultsModeLoop()
	{
		// ...
	}

	/*
		Inactive screen initialization and update loop
	*/

	private void ZeroModeLoop()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			PlayModeInit();
		}
	}
}