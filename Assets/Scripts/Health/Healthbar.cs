﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    void Start()
    {
        UpdateHealthBar();
    }

    void Update()
    {
        if (playerHealth.currentHealth != currenthealthBar.fillAmount)
        {
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        float currentHealth = playerHealth.currentHealth;

        // Put to limited for fillheath
        currenthealthBar.fillAmount = currentHealth / 10;
        healthText.text = currentHealth.ToString();
    }
}
