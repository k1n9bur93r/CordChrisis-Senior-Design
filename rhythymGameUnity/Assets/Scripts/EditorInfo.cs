using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

/*
	> EditorInfo class

	Text controller for information that appears while testing a chart from the editor.
*/

public class EditorInfo : MonoBehaviour
{
	public Metronome clock;
	private GameObject files; // SiteHandler

	private string finalText;
	private string beatText;
	private string tempoText;

	void Awake()
	{
		files = GameObject.Find("SiteHandler");

		if (files.GetComponent<SiteHandler>().gameMode)
		{
			gameObject.SetActive(false);
		}
	}

	void Update()
	{
		TruncateAll();
		DrawText();
	}

	private void TruncateAll()
	{
		beatText = clock.beatsElapsed.ToString("0.0000"); //String.Format("{0:f2}", Math.Truncate(clock.beatsElapsed * 100) / 100);
		tempoText = clock.tempo.ToString("0.0000"); //String.Format("{0:f2}", Math.Truncate(clock.tempo * 100) / 100);
	}

	private void DrawText()
	{
		finalText = "beat\n"
			+ beatText + "\n\n"
			+ "tempo\n"
			+ tempoText;

		GetComponent<TextMeshPro>().text = finalText;
	}
}