using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
	private const int FPS_LIMIT = 60; // Limit FPS for the sake of mobile battery life // Doesn't affect performance in the editor, verify on an actual mobile device

	void Start()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = FPS_LIMIT;
	}
}