using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_SunLight : MonoBehaviour
{
    public float height = 2;
    public float speed = 1;
    private float min;
    private float max;
    // Start is called before the first frame update
    void Start()
    {
        min = transform.position.z;
        max = transform.position.z + height;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * speed, max - min) + min);
    }
}
