using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	> GameManager class

	Watchdog class to check what section of the game we're on and to run timing-sensitive classes.
	Wakes up relevant classes and plays relevant Update()-like functions.

	REFERENCE MATERIALS
		- https://bemuse.ninja/project/docs/game-loop.html
		- https://forum.unity.com/threads/how-to-avoid-execution-order-nightmares.517578/
*/
public class GameManager : MonoBehaviour
{
	private const int MAX_RECEPTORS = 4;

	public GameObject meta; // Track

	public Metronome clock; // Metronome
	public InputController[] receptor;
	//public Scoreboard score; // Scoreboard
	public NoteSpawner spawner; // NoteSpawner

	void Start() // Track doesn't like Awake() for some reason, fix this
	{
		meta.SetActive(true);
	}

	// Timing-sensitive classes go here! They must be called in this order!
	void Update()
	{
		clock.Action();
		spawner.Action();

		for (int i = 0; i < MAX_RECEPTORS; i++)
		{
			receptor[i].Action();
		}
	}
}