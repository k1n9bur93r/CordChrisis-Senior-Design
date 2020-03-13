using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VU_Behavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, (transform.localScale.y - 1) / 2, transform.localPosition.z);
    }
}
