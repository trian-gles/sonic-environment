using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{

    private int objno;
    private float[] pitches = { 8.00f, 8.02f, 8.04f, 8.06f, 8.07f };
    private float pitch;
    private Vector3 targetScale;
    public Vector3 burstScale = new Vector3(2, 2, 2);
    public float burstDecay = 0.01f;

    public float maxIntensity = 5f;
    public TextAsset rtcScore;

    rtcmixmain RTcmix;
    private bool did_start = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        targetScale = transform.localScale;
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
        RTcmix.SendScore($"pitch = {pitch}" + rtcScore.text, objno);
    }

    // Update is called once per frame
    void Update()
    {
        float stepSize = burstDecay * Time.deltaTime;

        if (Vector3.Distance(transform.localScale, targetScale) > stepSize)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, burstDecay * stepSize);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Spirit Collision");
        if (other.GetType() == typeof(CapsuleCollider))
        {
            other.gameObject.GetComponent<SpiritController>().OnOrbCollision();
            PlaySound();
            Burst();
        }
    }

    private void Burst()
    {
        Vector3 newScale = new Vector3();
        newScale.x = burstScale.x;
        newScale.y = burstScale.y;
        newScale.z = burstScale.z;
        transform.localScale = newScale;
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
