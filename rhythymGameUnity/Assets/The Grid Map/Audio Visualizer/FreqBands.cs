using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqBands : MonoBehaviour
{
    public bool useBuffer;
    public int bandNum;
    public float startScale, scaleMultiplier;
    // Update is called once per frame
    void Update()
    {
        if(useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioSpectrum.bandBuffer[bandNum] * scaleMultiplier) + startScale, transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x, (transform.localScale.y - 1) / 2, transform.localPosition.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioSpectrum.freqBand[bandNum] * scaleMultiplier) + startScale, transform.localScale.z);
        }
    }
}
