using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerCollision : MonoBehaviour
{
    private CharacterController CC;
    // Start is called before the first frame update
    void Start()
    {
        CC = GetComponent<CharacterController>();
        CC.detectCollisions = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
