using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBehaviorColor : MonoBehaviour
{
    /// <summary>
    /// Ref to spectrumaudio
    /// </summary>
    private float[] spectrumAudio;

    /// <summary>
    /// Ref to Material
    /// </summary>
    Material material;
    /// <summary>
    /// 
    /// </summary>
    private int scaleMulti = 5;
    /// <summary>
    /// 
    /// </summary>
    private float baseCol = 25;

    private float multi;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];


    }

    // Update is called once per frame
    void Update()
    {
        float[] samples = new float[64];

        AudioListener.GetSpectrumData(samples, 1, FFTWindow.Hanning);
        Vector3 mousePos = Input.mousePosition;
        Color col = new Color((baseCol * mousePos.x) / scaleMulti, (baseCol - mousePos.y) / scaleMulti, (samples[0] * mousePos.y) / scaleMulti);
        multi = (mousePos.x / samples[0]);
        material.SetColor("_Color", col);
        material.SetFloat("_Multiply", multi);
    }
}
