using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaytestButton : MonoBehaviour
{
	private GameObject switcher;

	void Start()
	{
		GameObject files = GameObject.Find("SiteHandler");

		if (files.GetComponent<SiteHandler>().gameMode)
		{
			gameObject.SetActive(false);
		}

		else
		{
			switcher = GameObject.Find("PlaytestSwitcher");

			GetComponent<Button>().onClick.AddListener(delegate { switcher.GetComponent<PlaytestSwitcher>().PlayTestToggle(); });
		}
	}
}