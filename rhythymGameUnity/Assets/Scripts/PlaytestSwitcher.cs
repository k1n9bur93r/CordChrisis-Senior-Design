using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
	> PlaytestSwitcher class

	When the game is launched in editor mode, allows for switching between play and editor mode.
*/

public class PlaytestSwitcher : MonoBehaviour
{
	[HideInInspector]
	public double currentBeat;

	private bool testing = false;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions
	}

	void Update()
	{
		PlayTestButton();
	}

	private void PlayTestButton()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (!testing)
			{
				GameObject editor = GameObject.Find("EditorNoteController");
				currentBeat = editor.GetComponent<EditorNoteController>().curBeat;

				//SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
				Initiate.Fade("Main Game", Color.black, 5.0f);
				testing = true;
			}

			else
			{
				//SceneManager.LoadScene("NoteEditor", LoadSceneMode.Single);
				Initiate.Fade("NoteEditor", Color.black, 5.0f);
				testing = false;

				// TO DO: EditorNoteController forgets curBeat upon reloading scene, do something about this
			}
		}
	}
}