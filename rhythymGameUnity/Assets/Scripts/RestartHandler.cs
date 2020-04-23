using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartHandler : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		CheckRestartKey();
	}

	public void RestartSong()
	{
		GameObject results = GameObject.Find("ResultsManager");
		Destroy(results); // This object persists into the options screen, causing null refs

		Initiate.Fade("Options", Color.black, 5.0f);
	}

	private void CheckRestartKey()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			RestartSong();
		}
	}
}
