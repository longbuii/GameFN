using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceCollectible : MonoBehaviour
{
    [SerializeField] private float experienceValue; 
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SoundManager.instance.Playsound(pickupSound);
            XpManager xpManager = col.GetComponent<XpManager>();

            // Add to  GainExperience on XpManager 
            xpManager.GainExperience(experienceValue);

            // Off to item, after player is interact to it.
            gameObject.SetActive(false);
        }
    }
}
