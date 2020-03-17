using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimator : MonoBehaviour
{
	// used to indicate key presses
	private Renderer r;
	private Color pressedColor;
	private Color btnColor;

	// Start is called before the first frame update
	void Start()
	{
		r = GetComponent<Renderer>();
		btnColor = r.material.color;
		pressedColor = new Color(btnColor.r + 0.2f, btnColor.g + 0.2f, btnColor.b + 0.2f);        
	}

	// Update is called once per frame
	void Update()
	{
		// ...
	}

	public void SetPressedBtnColor()
	{
		//t1.text = "Key: " + keyPressed.ToString();
		r.material.color = pressedColor;
	}

	public void SetDefaultBtnColor()
	{
		r.material.color = btnColor;
	}
}