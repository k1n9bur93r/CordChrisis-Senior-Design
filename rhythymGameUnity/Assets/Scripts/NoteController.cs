using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public bool pressable;
    public KeyCode key;
    private Vector3 originalPos;
    public int score;

    void Start()
    {
        originalPos = transform.position;
    }

    void Update()
    {
        transform.position += new Vector3(1.0f * Time.deltaTime, 0, 0);

        if (pressable && Input.GetKeyDown(key))
        {
            score++;
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        transform.position = originalPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            pressable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            pressable = false;
        }
    }

}
