using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// a very simple script to make a G-above-middle-C 'beep' on startup

public class SoundController : MonoBehaviour {
    int objno = 0;
    rtcmixmain RTcmix;
    private bool did_start = false;


    private void Awake()
    {
        // find the RTcmixmain object with the RTcmix function definitions
        RTcmix = GameObject.Find("RTcmixmain").GetComponent<rtcmixmain>();
    }


    // Use this for initialization
    void Start ()
    {
        // initialize RTcmix
        RTcmix.initRTcmix(objno);

        did_start = true;
    }
	

    public void PlaySound()
    {
        RTcmix.SendScore("WAVETABLE(0, 0.2, 20000, 8.07, 0.5)", objno);
    }

	// Update is called once per frame
	void Update ()
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
