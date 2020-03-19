using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VU : MonoBehaviour
{
    public bool invertDirection;
    public float maxScale;
    public GameObject sampleCubePrefab;
    GameObject[] sampleCube = new GameObject[AudioSpectrum.sampleRange];
    // Start is called before the first frame update
    void Start()
    {
        int inv = 1;
        if(invertDirection)
        {
            inv = -1;
        }
        Vector3 currPos = this.transform.position;
        for (int i = 0; i < AudioSpectrum.sampleRange; i++)
        {
            GameObject instanceSampleCube = (GameObject)Instantiate(sampleCubePrefab);
            instanceSampleCube.transform.position = currPos;
            instanceSampleCube.transform.parent = this.transform;
            instanceSampleCube.name = "SampleCube" + i;
            //this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            //instanceSampleCube.transform.position = Vector3.forward * 100;
            sampleCube[i] = instanceSampleCube;
            currPos += new Vector3(1 * inv, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < AudioSpectrum.sampleRange; i++)
        {
            if(sampleCube != null)
            {
                sampleCube[i].transform.localScale = new Vector3(1, (AudioSpectrum.samples[i] * maxScale) + 2, 1);
            }
        }
    }
}
