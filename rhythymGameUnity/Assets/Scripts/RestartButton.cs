using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	> RestartButton

	Handler for the restart UI element.
*/

public class RestartButton : MonoBehaviour
{
	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(delegate { RestartSong(); });
	}

	void Update()
	{
		CheckRestartKey();
	}

	public void RestartSong()
	{
		GetComponent<Button>().onClick.RemoveListener(delegate { RestartSong(); });
		Initiate.Fade("Main Game", Color.black, 2.5f);
	}

	private void CheckRestartKey()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			RestartSong();
		}
	}
}
