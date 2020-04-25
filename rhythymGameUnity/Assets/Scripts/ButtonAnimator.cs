using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ButtonAnimator : MonoBehaviour
{
	// used to indicate key presses
	private Renderer r;
	private Color pressedColor;
	private Color btnColor;
    public KeyCode btnKey;

	void Start()
	{
		r = GetComponent<Renderer>();
		btnColor = r.material.color;
		pressedColor = new Color(btnColor.r + 0.2f, btnColor.g + 0.2f, btnColor.b + 0.2f);
		
	}

	void Update()
	{		
		if (Input.GetKey(btnKey))
		{
			SetPressedBtnColor();
		}
		else
		{
			SetDefaultBtnColor();
		}
	}

	public void SetPressedBtnColor()
	{
		//r.material.color = pressedColor;
		GetComponent<PostProcessVolume>().enabled = true;
	}

	public void SetDefaultBtnColor()
	{
		//r.material.color = btnColor;
		GetComponent<PostProcessVolume>().enabled = false;
	}
}