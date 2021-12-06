using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public GameObject dropPos;
    public GameObject[] dropObs;
    private List<GameObject> existingObs;
    private int selectedOb = 0;

    public float speed = 12;
    Vector3 velocity;
    bool isGrounded;
    public float gravity = -9.81f;

    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    // Start is called before the first frame update
    void Start()
    {
        UpdateMaterial();
        existingObs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            var newObj = Instantiate(dropObs[selectedOb], dropPos.transform.position, Quaternion.identity);
            existingObs.Add(newObj);
        }

        if (Input.GetKeyDown("q"))
        {
            selectedOb = (selectedOb - 1);
            if (selectedOb == -1)
            {
                selectedOb = dropObs.Length - 1;
            }
            UpdateMaterial();
        }

        if (Input.GetKeyDown("e"))
        {
            selectedOb = (selectedOb + 1) % dropObs.Length;
            UpdateMaterial();
        }

        if (Input.GetKeyDown("backspace"))
        {
            Debug.Log("backspace");
            DeleteLastObj();
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void DeleteLastObj()
    {
        int i = existingObs.Count - 1;
        GameObject lastObj = existingObs[i];
        existingObs.RemoveAt(i);
        Destroy(lastObj);
        foreach (GameObject spirit in GameObject.FindGameObjectsWithTag("Spirit"))
        {
            SpiritController spCon = spirit.GetComponent<SpiritController>();
            spCon.UpdateTarget();
        }
    }

    void UpdateMaterial()
    {
        dropPos.GetComponent<MeshRenderer>().material = dropObs[selectedOb].GetComponent<MeshRenderer>().sharedMaterial;
        dropPos.transform.localScale = dropObs[selectedOb].transform.localScale;
    }
}
