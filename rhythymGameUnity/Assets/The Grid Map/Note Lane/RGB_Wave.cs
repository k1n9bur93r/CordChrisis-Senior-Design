using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGB_Wave : MonoBehaviour
{
    // Fade the color from red to green
    // back and forth over the defined duration

    Color colorStart = Color.cyan;
    Color colorEnd = Color.white;
    float duration = 2.0f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        rend.material.color = Color.Lerp(colorStart, colorEnd, lerp);
    }
}
