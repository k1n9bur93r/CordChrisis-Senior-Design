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
