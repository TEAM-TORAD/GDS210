﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    public Animator anim;
    public bool isBlocking;

    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            anim.SetBool("Block", true);
            isBlocking = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            anim.SetBool("Block", false);
            isBlocking = false;
        }
    }
}
