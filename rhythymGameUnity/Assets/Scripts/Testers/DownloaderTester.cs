﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	> DownloaderTester class

	Uses downloaded items.
	NOTICE: Do not use this in the actual game!
*/

public class DownloaderTester : MonoBehaviour
{
	void Awake()
	{
		GameObject target = GameObject.Find("Downloader");

		if (target == null) { Debug.Log("Scene load failed!"); }

		// ---

		SetAudio(target.GetComponent<Downloader>().audioFile);
		SetChart(target.GetComponent<Downloader>().jsonContent);
	}

	public void SetAudio(AudioClip audio)
	{
		GetComponent<AudioSource>().clip = audio;
		GetComponent<AudioSource>().Play();
	}

	public void SetChart(string chart)
	{
		Debug.Log(chart);
	}
}
