using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;

public class Health : MonoBehaviourPun
{
    public int health = 100;
    public int maxHealth = 100;
    public bool healthCriticalRunning = false;
    //public int lives;
    public GameObject healthImagePrefab;
    private Transform healthPanel;
    private Image healthImage, healthImageBG;
    private Color lerpedColor;
    public Color greyColor, redColor;

    void Start()
    {
        if(photonView.IsMine)
        {
            healthPanel = GameObject.FindGameObjectWithTag("HealthPanel").transform;
            GameObject healthImageObject = Instantiate(healthImagePrefab, healthPanel);
            healthImage = healthImageObject.transform.GetChild(1).GetComponent<Image>();
            healthImageBG = healthImageObject.transform.GetChild(0).GetComponent<Image>();
        }
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                //Take Damage (testing purposes)
                TakeDamage(10);
            }
            if (healthCriticalRunning)
            {
                lerpedColor = Color.Lerp(greyColor, redColor, Mathf.PingPong(Time.time, 1.25f));
                healthImageBG.color = lerpedColor;
            }
        }
    }
    public void ResetHealth(int _health, int _maxHealth)
    {
        health = _health;
        maxHealth = _maxHealth;
        healthImage.fillAmount = (float) health / maxHealth;
        if ((float)health / maxHealth < 0.2f) healthCriticalRunning = false;
        else healthCriticalRunning = true;
    }
    
    public void TakeDamage(int value)
    {
        health -= value;
        if(health <= 0)
        {
            //Player dies
            Debug.Log("You died my friend.");
            health = 0;
        }
        else
        {
            // Player takes damage
            Debug.Log("You took " + value + " in damage.");
            if((float) health / maxHealth < 0.2f)
            {
                healthCriticalRunning = true;
            }
        }
        healthImage.fillAmount = (float) health / maxHealth;
    }
}
