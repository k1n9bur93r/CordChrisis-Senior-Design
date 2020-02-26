using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float noteSpeed;
    public Metronome metronome;
    public double beat;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //rb.velocity = new Vector3 (0f,0f,-noteSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
