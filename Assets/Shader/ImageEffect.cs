﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Image Effect - Under Water 
/// Creates a distortion effect on the camera that can be seen in editor view,
/// This was created by Cameron Garchow with Inspiration from PeerPlay
/// Items are also designed for use in basically any other part.
/// This is also taken form the ideas from nick pattison's warp effect as seen in class
/// This effect is essential to feeling underwater
/// Inspiration Taken from : https://www.youtube.com/watch?v=SN_syWoNBYs by PeerPlay - Not a Direct Copy of how it is done.
/// TODO : Add a Sound matrix that gives the feeling of underwater (subtle not too much)
/// </summary>
public class ImageEffect : MonoBehaviour
{
    /// <summary>
    /// material is a reference to the shader material
    /// </summary>
    public Material material;

    /// <summary>
    /// is the offset of the pixel noise
    /// </summary>
    [Range(0.001f, 0.1f)]
    public float _pixelOffset = 0.0093f;

    /// <summary>
    /// Noise Scale is the scale of the noise volume
    /// </summary>
    [Range(0.001f, 20f)]
    public float _noiseScale = 0.71f;

    /// <summary>
    /// Frequency is the noise's frequency of the effect throughout camera
    /// </summary>
    [Range(0.001f, 20f)]
    public float _noiseFrequency = 1.77f;

    /// <summary>
    /// _NoiseSpeed sets the speed of the noise volume on the camera, the higher the faster the movement of the noise.
    /// </summary>
    [Range(0.001f, 20f)]
    public float _noiseSpeed = 0.64f;

    /// <summary>
    /// Sets Threshold Division to 4 (part of a formula)
    /// </summary>
    private float _ThresholdDivision = 4;

    /// <summary>
    /// Ref to spectrumaudio
    /// </summary>
    private float[] spectrumAudio;


    /// <summary>
    /// Ampilitude and effect of how much the spectrum audio will effect the object / shader
    /// </summary>
    public float amp = 20;

    /// <summary>
    /// Ref to the bands
    /// </summary>
    public int bands =1;

    /// <summary>
    /// Value of the Spectrum Data that is retrieved
    /// </summary>
    public static float Valuespectrum { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        spectrumAudio = new float[256];
    }

    /// <summary>
    /// Update gives a reference to unity of the effect
    /// is called once a frame
    /// This allows us to set the object within the editor's camera settings.
    /// Also allows us to set all parts of the shader from the editor. DOES NOT NEED TO BE DELETED!
    /// Code is very similar to tutorial because there is no other way to do it unless you want me to jump through hoola hoops!
    /// </summary>
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        AudioListener.GetSpectrumData(spectrumAudio, 0, FFTWindow.Hanning);
        if (spectrumAudio != null && spectrumAudio.Length > 0)
        {
            Valuespectrum = spectrumAudio[0];

        }
        material.SetFloat("_NoiseFrequency", _noiseFrequency * (MusicSpectrum.bufferAudiobnad[bands] * amp));
        material.SetFloat("_NoiseSpeed", _noiseSpeed * (MusicSpectrum.bufferAudiobnad[bands] * amp));
        material.SetFloat("_NoiseScale", _noiseScale * (MusicSpectrum.bufferAudiobnad[bands] * amp));
        material.SetFloat("_PixelOffset", _pixelOffset - (mousePos.y * .0005f));
        material.SetFloat("_ThresholdDivide", _ThresholdDivision);
    }
    /// <summary>
    /// Renders on image 
    /// </summary>
    /// <param name="src"> source of the camera </param>
    /// <param name="dst">distance of the material</param>
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, material);
    }
}
