using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioVisual : MonoBehaviour
{


    AudioSource _audioSource;
    public static float[] _samples = new float[512];
    public static float[] _freqBand = new float[8];
    public static float[] _bandBuffer = new float[8];
    public static float[] _bufferDecrease = new float[8];

    float[] _freqBandHighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];



    // Start is called before the first frame update
    void Start()
    {

        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();

    }

    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if ( _freqBand [i] > _freqBandHighest [i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }

    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            if (_freqBand [g] > _bandBuffer [g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease [g]= 0.005f;
            }

            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer [g] -= _bufferDecrease [g];
                _bufferDecrease [g] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()
    {

        /* song has  x hrz / (we made 512) bands = 
         * 22050 / 512 = 43 herts per sammple 
         * 
         * 20 - 60
         * 60 - 250
         * 250 - 500
         * 500 - 2000
         * 2000 - 4000 
         * 4000 - 6000
         * 6000 - 20000 hertz
         * 
         * samples bands
         * 0 - 2 = 86 hertz
         * 1 - 4 = 172 hertz = range 87-258
         * 2 - 8 = 344 hertz = range 259-602
         * 3 - 16 = 688 hertz = range 603 -1290
         * 4 - 32 = 1376 hertz  = 1291-5418
         * 5 - 64 = 2752 hertz = 2667-5418
         * 6 - 128 = 5504 hertz = 5419-10922
         * 7 - 256 = 11008 hertz = 10923-21930
         */

        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            //check the equal sign
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }

            average /= count;

            _freqBand[i] = average * 10;
        }
    
    }
}
