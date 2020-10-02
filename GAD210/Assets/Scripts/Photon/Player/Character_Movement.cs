using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Character_Movement : MonoBehaviourPun
{
    #region Variables
    public float moveSpeed;
    public float jumpSpeed;
    public float walkSpeed;
    private Rigidbody rb;
    public Vector3 movement;
    [SerializeField]
    private GameObject cam;

    #endregion

    #region Animator_Variables
    private Animator runningAnim;


    #endregion


    void Start()
    {
        if(photonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            runningAnim = GetComponentInChildren<Animator>();
            cam.SetActive(true);
        }
    }

    #region Movement
    void Update()
    {
        //Only move you own character
        if(photonView.IsMine)
        {
            //DOESNT MOVE BASED ON ROTATION FIX IT 
            //UPDATE: FIXED IT
            //movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            movement = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal")); //WORKING!

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = walkSpeed;
            }
            else
            {
                moveSpeed = 8f;
            }

            if (Input.GetKey(KeyCode.W))
            {
                runningAnim.SetBool("isRunning", true);
            }
            else
            {
                runningAnim.SetBool("isRunning", false);
            }
        }
        
    }

    void characterMovement(Vector3 direction)
    {
        rb.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    #endregion

    #region Jumping
    void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            characterMovement(movement);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
            }
        }
    }
    #endregion





}
