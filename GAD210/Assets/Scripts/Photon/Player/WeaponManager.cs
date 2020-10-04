using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject ninjato;
    public KeyCode primary;

    public bool primaryEquiped;
    // Start is called before the first frame update
    void Start()
    {
        primaryEquiped = false;
        ninjato.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(primary))
        {
            if (primaryEquiped)
            {
                PrimaryDeactivated();
            }

            else
            {
                PrimaryActive();
            }

        }

    }

    void PrimaryActive()
    {
        ninjato.SetActive(true);
        primaryEquiped = true;
    }
    void PrimaryDeactivated()
    {
        ninjato.SetActive(false);
        primaryEquiped = false;    
    }
}
