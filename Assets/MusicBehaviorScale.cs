using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBehaviorScale : MonoBehaviour
{

    /// <summary>
    /// Band is the ranges at which can be given to the bars.
    /// Bands is EQUAL to music spectrum
    /// </summary>
   [Range(1,32)] public int band;
    /// <summary>
    /// starting scale of the object
    /// </summary>
    public float startScale;
    /// <summary>
    /// starting scale of the object
    /// </summary>
    public float scaleMulti;
    /// <summary>
    /// Reference to material
    /// </summary>
    Material mat;
    /// <summary>
    /// Reference to the Base Color
    /// </summary>
    public float baseCol = 155;
    /// <summary>
    /// Sets mouse controlled for Y transform
    /// ENABLE DISABLE
    /// </summary>
    public bool isMouseControlled = true;
    /// <summary>
    /// START
    /// Get Material REFERENCE AND SET FROM OBJECT
    /// </summary>
    private void Start()
    {
        mat = GetComponent<MeshRenderer>().materials[0];
    }
    /// <summary>
    /// Update
    /// Gets local transform and times it by the new freq bands.
    /// If mouse button is held down changes frequency to use freqbands instead!
    /// </summary>
    void Update()
    {
        if (band <= MusicSpectrum.mBytes)
        {
            Vector3 mousePos = Input.mousePosition;
            if (isMouseControlled == true)
            {
                scaledMousePos(mousePos);
            }
            else if (isMouseControlled == false)
            {
                notScaledPos(mousePos);
            }
        }
    }
    /// <summary>
    /// scaledMousePos
    /// Is scaled by mouse position in Y coordinate
    /// Clamped to prevent issues with warning
    /// </summary>
    /// <param name="mousePos"> is the return value of mouse position xyz </param>
    void scaledMousePos(Vector3 mousePos)
    {
       
            if (Input.GetMouseButton(0))
            {
            float tempValue = Mathf.Clamp(((MusicSpectrum.bufferAudiobnad[band]) + startScale) - mousePos.y * scaleMulti, Random.Range(1, 1 * mousePos.y + (MusicSpectrum.bufferAudiobnad[band])), 0); // prevent bug and warning of going below 0 
            transform.localScale = new Vector3(startScale, tempValue, startScale); ;
                Color col = new Color(baseCol + MusicSpectrum.bufferAudiobnad[band] * mousePos.x, 0 + MusicSpectrum.bufferAudiobnad[band], baseCol + MusicSpectrum.bufferAudiobnad[band] * mousePos.z);

                mat.SetColor("_Color", col);
            }
            if (!Input.GetMouseButton(0))
            {
            float tempValue = Mathf.Clamp(((MusicSpectrum.bufferAudiobnad[band]) + startScale) - mousePos.y * scaleMulti, 150, 0); // prevent bug and warning of going below 0
            transform.localScale = new Vector3(startScale, tempValue, startScale);
                Color col = new Color((baseCol * mousePos.x) * scaleMulti, 0 + MusicSpectrum.bufferAudiobnad[band] * mousePos.y, ((baseCol * mousePos.x) * scaleMulti) * MusicSpectrum.bufferAudiobnad[band]);

                mat.SetColor("_Color", col);
            }
     
    }
    /// <summary>
    /// NoTscalePos
    /// Is not scaled by mouse position in Y coordinate
    /// </summary>
    /// <param name="mousePos"> is the return value of mouse position </param>
    void notScaledPos(Vector3 mousePos)
    {

        if (Input.GetMouseButton(0))
        {
            float tempValue = ((MusicSpectrum.bufferAudiobnad[band]) + startScale); //refactor
            transform.localScale = new Vector3(startScale, tempValue, startScale); ;
            Color col = new Color(baseCol + MusicSpectrum.bufferAudiobnad[band] * mousePos.x, 0 + MusicSpectrum.bufferAudiobnad[band], baseCol + MusicSpectrum.bufferAudiobnad[band] * mousePos.z);
            mat.SetColor("_Color", col);

        }
        if (!Input.GetMouseButton(0))
        {
            float tempValue = (((MusicSpectrum.bufferAudiobnad[band]) * scaleMulti) + startScale);
            transform.localScale = new Vector3(startScale, tempValue, startScale);
            Color col = new Color((baseCol * mousePos.x) * scaleMulti, 0 + MusicSpectrum.bufferAudiobnad[band] * mousePos.y, ((baseCol * mousePos.x) * scaleMulti) * MusicSpectrum.bufferAudiobnad[band]);
            mat.SetColor("_Color", col);
        }

    }
}
