using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpiritController : MonoBehaviour
{

    private int targetNum = 0;
    private GameObject[] targets = { };
    public float moveSize = .5f;
    private bool backwards;
    private int objno;
    rtcmixmain RTcmix;
    private bool did_start = false;
    private float[] pitches = { 9.00f, 9.02f, 9.04f, 9.06f, 9.07f };
    private float pitch;
    public TextAsset rtcScore;

    [SerializeField] private UnityEngine.Rendering.Volume volume;
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

        RTcmix.SendScore($"pitch = {pitch}" + rtcScore.text, objno);
        RTcmix.setpfieldRTcmix(1, 0, objno);



    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Length > 1)
        {
            Vector3 targetPos = targets[targetNum].transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSize * Time.deltaTime);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Spirit Collision");
        if (other.gameObject == targets[targetNum])
        {
            
        }
    }

    public void OnOrbCollision()
    {
        UpdateTarget();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Spirit")
        {
            SphereCollider selfColl = GetComponent<SphereCollider>();
            float dist = Vector3.Distance(transform.position, other.gameObject.transform.position);
            float amp = (selfColl.radius - dist) /selfColl.radius;
            RTcmix.setpfieldRTcmix(1, amp, objno);
            SpiritController otherSpirit = other.gameObject.GetComponent<SpiritController>();

            // Flip if the target is moving in the same direction
            if ((targetNum == otherSpirit.targetNum) && (backwards == otherSpirit.backwards))
            {
                backwards = !backwards;
            }

            volume.weight = (selfColl.radius - dist) / selfColl.radius;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Spirit")
        {
            RTcmix.setpfieldRTcmix(1, 0, objno);
            volume.weight = 0;
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
