using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	> SiteHandler class

	Passes information to and from the game and site.
*/

public class SiteHandler : MonoBehaviour
{
	// Make these private at some point?

	[Header("Check this while testing, uncheck before building")]
	public bool bypassWait = false;
	[Space]

	private bool initDone = false;

	// Track vars
	public Track track;
	public string track_file;

	// Metronome vars
	public Metronome metronome;
	public double userOffset;

	// NoteSpawner vars
	public NoteSpawner noteSpawner;
	public float speedMod;

	// InputController vars
		// BINDINGS GO HERE

	void InitTrack(string user)
	{
		track_file = user;
	}

	void InitMetronome(float user)
	{
		userOffset = user;
	}

	void InitNoteSpawner(float user)
	{
		speedMod = user;
	}

	// Check if these ^^^^^ can be called during the Awake() loop

	void Awake()
	{
		if (bypassWait)
		{
			initDone = true;
		}

		/*
		while (!initDone)
		{
			// this won't work, need a co-routine or something to wait for this i guess
		}
		*/

		Debug.Log("[SiteHandler] Intializing other objects...");

		// Track
		track.track_file = track_file;

		// Metronome
		metronome.userOffset = userOffset;

		// NoteSpawner
		noteSpawner.speedMod = speedMod;

		// InputController
			// ...

		Debug.Log("[SiteHandler] Done!");
	}
}
