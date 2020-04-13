using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureSpawner : MonoBehaviour
{
    public List<GameObject> gestureObjects;
    // public List<GameObject>[] gestures = new List<GameObject>[4];

    public Dictionary<double, GameObject> gestureGameObjects = new Dictionary<double, GameObject>();
    public Dictionary<double, int> gestureNums = new Dictionary<double, int>();

    void Awake()
    {
        // for (int i = 0; i < 4; i++)
        // {
        //     gestures[i] = new List<GameObject>();
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void spawnGesture(int gestureNum, double beat)
    {
        GameObject curGesture = Instantiate(gestureObjects[gestureNum], new Vector3(0f, 2.0f, (float) beat), Quaternion.Euler(31.69f, 0, 0));

        removeGesture(beat);

        try
        {
            gestureGameObjects.Add(beat, curGesture);
            gestureNums.Add(beat, gestureNum);
        }
        catch (ArgumentException)
        {
            gestureGameObjects[beat] = curGesture;
            gestureNums[beat] = gestureNum;
        }

        // gestureObjects.Add(beat, curGesture);
        // gestureNums.Add(beat, int);
        
        // print(curGesture);

        // gestures[gestureNum].Add(curGesture.gameObject);

    }

    void removeGesture(double beat)
    {
        if (gestureGameObjects.ContainsKey(beat)) {
            GameObject gesture = gestureGameObjects[beat];
            gestureGameObjects.Remove(beat);
            Destroy(gesture);
        }
        if (gestureNums.ContainsKey(beat)) {
            gestureNums.Remove(beat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up")) {
            spawnGesture(0, 4);
        } else if (Input.GetKey("right")) {
            spawnGesture(1, 4);
        } else if (Input.GetKey("down")) {
            spawnGesture(2, 4);
        } else if (Input.GetKey("left")) {
            spawnGesture(3, 4);
        } else if (Input.GetKey("backspace")) {
            Debug.Log("delete");
            removeGesture(4);
        }
    }
}
