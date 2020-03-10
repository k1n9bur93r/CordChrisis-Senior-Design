using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public float height = 2;
    public float speed = 1;
    private float min;
    private float max;
    // Start is called before the first frame update
    void Start()
    {
        min = transform.position.y;
        max = transform.position.y + height;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * speed, max - min) + min, transform.position.z);
    }
}
