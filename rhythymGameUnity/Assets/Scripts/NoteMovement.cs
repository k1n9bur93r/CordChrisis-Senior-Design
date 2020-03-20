using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float speedMod;
    public Metronome metronome;
    public double beat;
    public double length;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //rb.velocity = new Vector3 (0f,0f,-speedMod);
    }

    // Update is called once per frame
    void Update()
    {

    }
}