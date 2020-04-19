using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	> IntroManager class

	Handles intro sequence and song delays during play mode.
*/

public class IntroManager : MonoBehaviour
{
	private GameObject metronome;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions
	}

	void Start()
	{
		GameObject files = GameObject.Find("SiteHandler");

		if (!files.GetComponent<SiteHandler>().gameMode)
		{
			gameObject.SetActive(false);
		}

		metronome = GameObject.Find("Metronome");
	}
}
