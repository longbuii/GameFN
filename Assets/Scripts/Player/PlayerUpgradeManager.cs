using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int upgradePointsOnLevelUp; 
    public int damage;
    private PlayerStatus playerStatus; 
    private PlayerMove PlayerMove;
    private PlayerAttack playerAttack;
    public Image defenseUpgradeButton;
    public Image luckUpgradeButton;
    public Image critRateUpgradeButton;
    public Image speedUpgradeButton;
    public Image damageUpgradeButton;



    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>(); 
        PlayerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        damage = playerAttack.damage;

        if (PlayerPrefs.HasKey("UpgradePoints"))
        {
            playerStatus.upgradePoints = PlayerPrefs.GetInt("UpgradePoints");
        }
        UpdateUI();

    }

    // Update to Defense
    public void UpgradeDefense()
    {
        if (playerStatus.upgradePoints > 0)
        {
            playerStatus.Defense += 0.5f;
            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }

    // Update to CriRate
    public void UpgradeCritRate()
    {
        if (playerStatus.upgradePoints > 0)
        {
            playerStatus.CritRate += 0.5f; 
            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }

    // Update to Lucky
    public void UpgradeLuck()
    {
        if (playerStatus.upgradePoints > 0)
        {
            playerStatus.Luck += 0.5f; 
            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }


    // Update to Damage

    public void UpgradeDamage()
    {
        if (playerStatus.upgradePoints > 0)
        {
            int damageIncrease = 1;
            playerAttack.damage += damageIncrease;

            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }


    // Update to Speed
    public void UpgradeSpeed()
    {
        if (playerStatus.upgradePoints > 0)
        {
            PlayerMove.speed += 0.1f; 
            PlayerPrefs.SetFloat("PlayerSpeed", PlayerMove.speed);

            playerStatus.upgradePoints--;
            UpdateUI();
            playerStatus.UpdateUI();
            PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
            PlayerPrefs.Save();
        }
    }

    public void UpdateUI()
    {
        // Update to UI when it have change to something of Status
        if (defenseUpgradeButton != null)
        {
            defenseUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        if (critRateUpgradeButton != null)
        {
            critRateUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        if (luckUpgradeButton != null)
        {
            luckUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        if (damageUpgradeButton != null)
        {
            damageUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        if (speedUpgradeButton != null)
        {
            speedUpgradeButton.enabled = playerStatus.upgradePoints > 0;
        }

        playerStatus.UpdateUI();
    }
}
