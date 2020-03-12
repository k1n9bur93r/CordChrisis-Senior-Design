using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
	> GameManager class

	Master game loop and state handler.

	Wakes up objects and/or executes them in the correct order as needed.
	Objects that have an Update() method and rely on execution order should be transitioned to work with this class.
	Set those objects up as follows:
		1.) Set the object as a public class of itself.
		x.) If the object must be activated (not updated!) at a certain time, set the object up as a public GameObject.
		2.) Rename Update() or FixedUpdate() to Action().
		3.) Call YourObject.Action() in the appropriate looping function.

	This class also handles which "state" of the game we are currently in.
	The states are as follows:
		- Play: The main game screen.
		- Results: The results screen.
		- Zero: An empty screen that waits for the Play screen to start.
	These have their own initialization and update/loop functions.

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

	/*
		Game loop selector. Changes depending on which state of the game we are currently in.
	*/

	void Update()
	{
		switch (mode)
		{
			case GameState.Play:
				PlayModeLoop();
				break;

			case GameState.Results:
				ResultsModeLoop();
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
		Play screen initialization and update loop.

		Does the following in this order:
			1.) Timer
			2.) Audio
			3.) Input
			4.) Note movement
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
		//clock.StartSong();
		clock.StartSongAnywhere(); // DEBUG ONLY

		mode = GameState.Play;
	}

	/*
		Results screen initialization and update loop.
	*/

	private void ResultsModeLoop()
	{
		// ...
	}

	/*
		Inactive screen initialization and update loop.
	*/

	private void ZeroModeLoop()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			PlayModeInit();
		}
	}
}