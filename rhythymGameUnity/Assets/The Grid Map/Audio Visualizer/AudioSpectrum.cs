﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]

public class AudioSpectrum : MonoBehaviour
{
    AudioSource audioSource;
    public static int sampleRange = 512;
    public static float[] samples = new float[sampleRange];    // Stores the frequency sample
    public static float[] freqBand = new float[8];
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        int count = 0;
        float average;

        for(int i = 0; i < 8; i++)
        {
            average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            // 7 Frequency Bands
            if(i == 7)
            {
                sampleCount += 2;
            }

            for(int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;
            freqBand[i] = average * 10;
        }
    }
}
