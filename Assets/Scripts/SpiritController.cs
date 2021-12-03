using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritController : MonoBehaviour
{

    private int targetNum = 0;
    private GameObject[] targets = { };
    public float moveSize = .5f;
    private bool backwards;
    // Start is called before the first frame update
    void Start()
    {
        backwards = (Random.value > 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Length > 1)
        {
            Vector3 targetPos = targets[targetNum].transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSize);

            if (Vector3.Distance(transform.position, targetPos) < moveSize)
            {
                UpdateTarget();
            }
        }
    }

    public void UpdateTarget()
    {
        targets = GameObject.FindGameObjectsWithTag("Orb");
        if (!backwards)
        {
            targetNum = (targetNum + 1) % targets.Length;
        }
        else
        {
            targetNum -= 1;
            if (targetNum == 0)
            {
                targetNum = targets.Length - 1;
            }
        }
        
    }
}
