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
        Destroy(go, 100f);

        Vector3 initOff = new Vector3(0, 0, -600);
        GameObject go2 = Instantiate(Grid, StartPos.position + initOff, StartPos.rotation);
        go2.AddComponent<GridWave>();
        go2.transform.localScale = new Vector3(GScale, GScale, 100);
        Destroy(go, 100f);
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
            Destroy(go, 100f);
            Timer = 60f;
        }
    }
}
