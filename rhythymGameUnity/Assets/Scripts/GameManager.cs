using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
	> GameManager class

	wait a second no

	REFERENCE MATERIALS:
		- https://bemuse.ninja/project/docs/game-loop.html
		- https://forum.unity.com/threads/how-to-avoid-execution-order-nightmares.517578/
*/

public class GameManager : MonoBehaviour
{
	enum GameState { Play, Results, Zero };

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions
	}

	void Start()
	{
		// ...
	}
}