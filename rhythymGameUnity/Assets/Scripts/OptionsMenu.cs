using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
	> OptionsMenu class

	Handles the options menu.
	Passes options chosen in this menu to SiteHandler before moving to the game or editor.
*/

public class OptionsMenu : MonoBehaviour
{
	public TextMeshProUGUI speedText;
	public TextMeshProUGUI offsetText;

	private GameObject files; // SiteHandler

	float userSpeed; //= 1.0f;
	double userOffset; //= 0.0;

	void Start()
	{
		files = GameObject.Find("SiteHandler");

		userSpeed = files.GetComponent<SiteHandler>().userSpeed;
		userOffset = files.GetComponent<SiteHandler>().userOffset;

		// ---

		GameObject playtester = GameObject.Find("PlaytestSwitcher");
		Destroy(playtester); // Do not want the playtest switcher in the options menu
	}

	void Update()
	{
		PrintSpeed();
		PrintOffset();
	}

	private void PrintSpeed()
	{
		speedText.text = userSpeed.ToString("0.0x");
	}
	
	private void PrintOffset()
	{
		string temp;

		if (userOffset > 0.0) { temp = "+"; }
		else if (userOffset == 0.0) { temp = "±"; }
		else { temp = ""; }

		offsetText.text = temp + userOffset.ToString("0 ms");
	}

	// ---

	public void SpeedUp() { userSpeed += 0.1f; ValidateSpeed(); }
	public void SpeedUpExtra() { userSpeed += 1.0f; ValidateSpeed(); }
	public void SpeedDown() { userSpeed -= 0.1f; ValidateSpeed(); }
	public void SpeedDownExtra() { userSpeed -= 1.0f; ValidateSpeed(); }

	private void ValidateSpeed()
	{
		if (userSpeed < 1.0)
		{
			userSpeed = 1.0f;
		}

		if (userSpeed > 10.0)
		{
			userSpeed = 10.0f;
		}
	}

	// ---

	public void OffsetUp() { userOffset += 1; ValidateOffset(); }
	public void OffsetUpExtra() { userOffset += 10; ValidateOffset(); }
	public void OffsetDown() { userOffset -= 1; ValidateOffset(); }
	public void OffsetDownExtra() { userOffset -= 10; ValidateOffset(); }

	private void ValidateOffset()
	{
		if (userOffset < -100.0)
		{
			userOffset = -100.0;
		}
	}

	// ---

	public void StartGame()
	{
		files.GetComponent<SiteHandler>().SetOptionsIngame(userSpeed, userOffset);

		if (files.GetComponent<SiteHandler>().gameMode)
		{
			Initiate.Fade("Main Game", Color.black, 5.0f);
			//SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
		}

		else
		{
			Initiate.Fade("NoteEditor", Color.black, 5.0f);
			//SceneManager.LoadScene("NoteEditor", LoadSceneMode.Single);
		}
	}
}
