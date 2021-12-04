using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritController : MonoBehaviour
{

    private int targetNum = 0;
    private GameObject[] targets = { };
    public float moveSize = .5f;
    private bool backwards;
    private int objno;
    rtcmixmain RTcmix;
    private bool did_start = false;
    private float[] pitches = { 8.00f, 8.02f, 8.04f, 8.06f, 8.07f };
    private float pitch;
    // Start is called before the first frame update
    void Start()
    {
        backwards = (Random.value > 0.5f);
    }

    private void Awake()
    {
        UpdateTarget();
        objno = GameObject.FindGameObjectsWithTag("Spirit").Length;
        RTcmix = GameObject.Find("RTcmixmain").GetComponent<rtcmixmain>();
        RTcmix.initRTcmix(objno);

        did_start = true;
        pitch = pitches[objno % pitches.Length];


    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Length > 1)
        {
            Vector3 targetPos = targets[targetNum].transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSize * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < moveSize * Time.deltaTime)
            {
                UpdateTarget();
                RTcmix.SendScore($"WAVETABLE(0, 0.2, 20000, {pitch}, 0.5)", objno);
            }
        }
    }

    public void UpdateTarget()
    {
        

        targets = GameObject.FindGameObjectsWithTag("Orb");
        if (targets.Length < 2)
        {
            return;
        }
        if (!backwards)
        {
            targetNum = (targetNum + 1) % targets.Length;
        }
        else
        {
            targetNum -= 1;
            if (targetNum == -1)
            {
                targetNum = targets.Length - 1;
            }
        }
        
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
