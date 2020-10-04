using Script.Movements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class PlayerAttacks : MonoBehaviour
{
    public WeaponManager wepManager;
    public GameObject block;
    public GameObject blockFX;
    public Animator anim;
    public bool movingLeft;
    public bool movingRight;
    // Start is called before the first frame update
    void Start()
    {
        wepManager = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(MovingLeft());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(MovingRight());
        }

        if (wepManager.primaryEquiped)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                blockAttack();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                swordAttack();

            }
        }

        IEnumerator MovingRight()
        {
            movingRight = true;
            movingLeft = false;
            anim.SetBool("movingRight", true);
            anim.SetBool("movingLeft", false);
            yield return new WaitForSeconds(1);
            anim.SetBool("movingRight", false);
            
        }
        IEnumerator MovingLeft()
        {
            movingLeft = true;
            movingRight = false;
            anim.SetBool("movingLeft", true);
            anim.SetBool("movingRight", false);
            yield return new WaitForSeconds(1);
            anim.SetBool("movingLeft", false);
           
        }
       
        void blockAttack()
        {
            StartCoroutine(EndBlock());
        }

        IEnumerator EndBlock()
        {
            block.SetActive(true);
            blockFX.SetActive(true);
            anim.SetBool("block", true);
            yield return new WaitForSeconds(2);
            anim.SetBool("block", false);
            block.SetActive(false);
            blockFX.SetActive(false);
        }
        void swordAttack()
        {           
            StartCoroutine(EndAttack());
        }

        IEnumerator EndAttack()
        {
            anim.SetBool("attack", true);
            yield return new WaitForSeconds(2);
            anim.SetBool("attack", false);
            
        }
    }
}

