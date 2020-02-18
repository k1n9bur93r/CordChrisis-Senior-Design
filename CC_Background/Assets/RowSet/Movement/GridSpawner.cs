using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public Transform StartPos;
    public GameObject Grid;
    public float Timer = 60f;
    private float GScale = 30000f;
    void Start()
    {
        GameObject go = Instantiate(Grid, StartPos.position, StartPos.rotation);
        go.AddComponent<GridWave>();
        go.transform.localScale = new Vector3(GScale, GScale, 100);
    }
    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            GameObject go = Instantiate(Grid, StartPos.position, StartPos.rotation);
            go.AddComponent<GridWave>();
            go.transform.localScale = new Vector3(GScale, GScale, 100);
            Timer = 60f;
        }
    }
}
