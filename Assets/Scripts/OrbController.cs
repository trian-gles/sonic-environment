using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
