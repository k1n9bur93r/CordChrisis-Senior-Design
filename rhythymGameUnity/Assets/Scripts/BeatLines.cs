using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatLines : MonoBehaviour
{
    int beats = 100;
    int beat_length = 2;
    int track_width = 4;
    float line_width = 0.2f;
    List<GameObject> lines;

    // Start is called before the first frame update
    void Start()
    {
        lines = new List<GameObject>();
        addBeats();
    }

    void addBeats()
    {
        for (int i = 0; i <= beats * beat_length; i += beat_length)
        {
            addBeatLine(i);
        }
    }

    void addBeatLine(int z_position)
    {
        GameObject go = new GameObject();
        lines.Add(go);
        LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.material.color = Color.grey;
        lineRenderer.SetPosition(0, new Vector3( track_width, 0, z_position));
        lineRenderer.SetPosition(1, new Vector3(-track_width, 0, z_position));
        lineRenderer.widthMultiplier = 0.2f;
    }

    void destroyLines() {
        foreach (GameObject go in lines)
        {
            Destroy(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
