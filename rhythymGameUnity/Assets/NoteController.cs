using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -1.0f * Time.deltaTime, 0);
        if (transform.position.y <= 0.375f && transform.position.y >= -0.375f && Input.GetKeyDown(KeyCode.Q))
        {
            Destroy(gameObject);
        }
    }
}
