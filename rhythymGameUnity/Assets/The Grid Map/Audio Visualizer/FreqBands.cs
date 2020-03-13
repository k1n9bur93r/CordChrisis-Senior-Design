using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqBands : MonoBehaviour
{
    // Start is called before the first frame update
    public int bandNum;
    public float startScale, scaleMultiplier;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, (AudioSpectrum.freqBand[bandNum] * scaleMultiplier) + startScale, transform.localScale.z);
    }
}
