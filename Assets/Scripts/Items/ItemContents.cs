using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContents : MonoBehaviour
{
    public float experienceValue;
    public float healthValue;
    public float defenseValue;

    public XpManager xpManager;
    public Health health;

    private void Start()
    {
        xpManager = FindObjectOfType<XpManager>();
        health = FindObjectOfType<Health>();
    }

    public void UseExperienceItem()
    {
        if (xpManager != null)
        {
            xpManager.GainExperience(experienceValue);
            Debug.Log("Using Experience Item");
        }
    }
    public void UseHealthItem()
    {
        if (health != null)
        {
            health.Addheal(healthValue);
            Debug.Log("Using Health Item");

        }
    }
    public void UseDefenseItem()
    {
        PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.AddDefense(defenseValue);
            Debug.Log("Used Defense Item and gained " + defenseValue + " defense.");
        }
    }
}

