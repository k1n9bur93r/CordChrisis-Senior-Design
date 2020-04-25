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
	public static PlaytestSwitcher instance;

	[HideInInspector]
	public double currentBeat;

	private bool testing = false;

	void Awake()
	{
		// https://answers.unity.com/questions/1108634/dontdestroyonload-many-instances-of-one-object.html
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions
		}

		else if (instance != this)
		{
			Destroy(this);
		}		
	}

	void Update()
	{
		/*
		if (Input.GetKeyDown(KeyCode.T))
		{
			PlayTestToggle();
		}
		*/

		//Debug.Log(testing);
	}

	public void PlayTestToggle()
	{
		Scene currentScene = SceneManager.GetActiveScene();

		if (!testing && (currentScene.name == "NoteEditor")) // ISSUE/TEMP FIX: For some reason this conditional fires off when switching from game to editor
		{
			testing = true;

			GameObject editor = GameObject.Find("EditorNoteController");
			currentBeat = editor.GetComponent<EditorNoteController>().curBeat;
			Initiate.Fade("Main Game", Color.black, 5.0f);
			//SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
		}

		else
		{
			testing = false;

			Initiate.Fade("NoteEditor", Color.black, 5.0f);
			//SceneManager.LoadScene("NoteEditor", LoadSceneMode.Single);
		}
	}
}