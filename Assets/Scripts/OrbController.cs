using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{

    private int objno;
    private float[] pitches = { 8.00f, 8.02f, 8.04f, 8.06f, 8.07f };
    private float pitch;

    rtcmixmain RTcmix;
    private bool did_start = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        foreach (GameObject spirit in GameObject.FindGameObjectsWithTag("Spirit"))
        {
            SpiritController spCon = spirit.GetComponent<SpiritController>();
            spCon.UpdateTarget();
        }

        objno = GameObject.FindGameObjectsWithTag("Orb").Length + 100;

        RTcmix = GameObject.Find("RTcmixmain").GetComponent<rtcmixmain>();
        RTcmix.initRTcmix(objno);

        did_start = true;
        pitch = pitches[objno % pitches.Length];
    }

    public void PlaySound()
    {
        RTcmix.SendScore($"WAVETABLE(0, 0.2, 10000, {pitch}, 0.5)", objno);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!did_start) return;

        // compute sound samples
        RTcmix.runRTcmix(data, objno, 0);
    }


    void OnApplicationQuit()
    {
        did_start = false;
        RTcmix = null;
    }
}
