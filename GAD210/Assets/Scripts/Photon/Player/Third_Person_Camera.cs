using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Third_Person_Camera : MonoBehaviourPun
{
    public float rotationSpeed;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform player;
    float mouseX;
    float mouseY;

    void Start()
    {
        if(photonView.IsMine)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void LateUpdate()
    {
        if(photonView.IsMine) CameraController();
    }

    void CameraController()
    {
        transform.LookAt(target);

        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -20, 80); //Perfect clamp

        //clamp at -35, 60 NOPE
        //clamp at -30,70 NOPE

        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        player.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
