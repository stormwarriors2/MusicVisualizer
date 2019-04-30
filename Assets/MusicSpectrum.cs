using UnityEngine;

/// <summary>
/// Followed tutorial base code so i don't have to declare audiolistener in each
/// Based this on the code from my music visualizer i did the first time. 
/// This time using unity. 
/// Inspiration from: https://www.youtube.com/watch?v=mHk3ZiKNH48&t=3s Peerplay
/// Inspiration also from the original assignment
/// Refs : https://docs.unity3d.com/ScriptReference/AudioSource.GetSpectrumData.html
/// Usage : FFt.Hamming : https://docs.unity3d.com/ScriptReference/FFTWindow.Hamming.html //better frequency domain for sound waves
/// Hamming : https://www.mathworks.com/help/signal/ref/hamming.html
/// uses up 40 bytes, anything up 64 bytes is too much for the system to handle
/// Must be multiples of 8
/// </summary>
[RequireComponent (typeof(AudioSource))]
public class MusicSpectrum : MonoBehaviour
{
    /// <summary>
    /// Spectrum audio is a float / ref to the audio (For Left)
    /// </summary>
    public float[] spectrumAudioLeft = new float [1024];
    /// <summary>
    /// Spectrum audio is a float / ref to the audio (For Right)
    /// </summary>
    public float[] spectrumAudioRight = new float[1024];
    /// <summary>
    /// Reference to Bits Multiples 0f 8
    /// Bytes of storage for Frequency bands 
    /// </summary>
    public static float mBytes = 64;

    /// <summary>
    /// Audio Band Float for seperate functionality
    /// </summary>
    public static float[] bandAd = new float[64];
    /// <summary>
    /// Buffer and Audio Bands combined
    /// </summary>
    public static float[] bufferAudiobnad = new float[64];


    /// <summary>
    /// Amplitude multiply to increase
    /// </summary>
    public float amp = 20;
    /// <summary>
    /// FreqBands = FFT.gETBANDS
    /// </summary>
    float[] freqBands = new float[64];
    /// <summary>
    /// Buffer variable to control the buffer so it is less extreme and less fast, FFT GET AVG
    /// </summary>
    float[] buffer = new float[64];
    /// <summary>
    /// Cannnot be changed,
    /// Is the difference between FreqBands and Buffer
    /// </summary>
    float[] bufferComp = new float[64];

    /// <summary>
    /// Averages of the channel bands / audio waves
    /// </summary>
    private float averages;

    /// <summary>
    /// Audio Source is a reference
    /// </summary>
    AudioSource sourceaudio;
    /// <summary>
    /// Height of the frequency / sound wave
    /// </summary>
    float[] FREQUENCYHEIGHT = new float[64];
    /// <summary>
    /// Reference to the channels that could be heard,
    /// Stereo is both left and right hearing
    /// Self Explanatory
    /// </summary>
    public enum channels { Stereo};
    /// <summary>
    /// Reference to the channels of the stereo.
    /// </summary>
    public channels channel = new channels();
    
    // Start is called before the first frame update
    void Start()
    {
        sourceaudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Update
    /// Goes through spectrum and frequency bands and buffer bands
    /// Get Specturm gets the audiolistener for FFTwindow.HAMMING
    /// 
    /// </summary>
    void Update()
    {

        GetSpectrum();
        FrequencyBands();
        bufferband();
        createaudioBand();
    }
    /// <summary>
    /// Create Audio Bands
    /// Creates Different Audio bands that can be used for each cube.
    /// </summary>
    void createaudioBand()
    {
        for (int b = 0; b < mBytes; b++)
        {
            if (freqBands[b] > FREQUENCYHEIGHT[b])
            {
                FREQUENCYHEIGHT[b] = freqBands[b];
            }
            bandAd[b] = (freqBands[b] / FREQUENCYHEIGHT[b]);
            bufferAudiobnad[b] = (freqBands[b] / FREQUENCYHEIGHT[b]);
        }
    }
    /// <summary>
    /// Get Spectrum data for audio listener
    /// On both left and right sound spectrums
    /// HAMMING wave form has less higher peaks compared to blackbird and hanning
    /// </summary>
    void GetSpectrum()
    {
        AudioListener.GetSpectrumData(spectrumAudioLeft, 1, FFTWindow.Hamming);
        AudioListener.GetSpectrumData(spectrumAudioRight, 1, FFTWindow.Hamming);
    }
    /// <summary>
    /// Bufferband is the buffer to prevent instant closing of the object scales.
    /// Setting the buffer to the frequency band will 'normalize' the value allowing for smoother transitions similar to the processing example in class
    /// 
    /// </summary>
    void bufferband()
    {
        for (int t = 0; t < mBytes; t++)
        {
            if (freqBands == null) return; //check for out of bounds if so stops script!
            
            if (freqBands [t] >= buffer[t])
            {
                buffer[t] = freqBands[t];
                bufferComp[t] = .007f;
            }
            if (freqBands [t] <= buffer[t])
            {
                buffer[t] -= bufferComp[t];
                bufferComp[t] *= 1.5f;
            }
        }
    }
    /// <summary>
    /// Divide audio into different bands.
    /// From Left, Right, or Stereo
    /// Cannot go past 7 objects tied to audio data!
    /// This is because of the multiplication of the threads / audio data. 
    /// There is a maxium amount of threads that can be given
    /// </summary>
    void FrequencyBands()
    {
        ///Divide into different frequencies
        int counter = 0; //counter for timer 
        int samples = 1;
        int power = 0;
        for (int i = 0; i < mBytes; i++)
        {
            averages = 0;
            //i == 16|| i == 24 || 
            if (i == 16 || i == 24 || i == 32 || i == 40 || i == 56 || i == 64) // each seperates it into different "band's" or number orders for bytes
            {
                power++;
                samples = (int)Mathf.Pow(2, power);
                if (power == 3)
                {
                    samples -= 2;
                }
            }
            for (int v = 0; v < samples;  v++)
            {
                if (channel == channels.Stereo)
                {
                    averages += spectrumAudioLeft[counter] + spectrumAudioRight[counter] * (counter + 1);
                }
                counter++;
            }
            averages /= counter;
            freqBands[i] = averages * 80;
        }
    }

}
