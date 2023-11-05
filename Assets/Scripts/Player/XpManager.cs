using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XpManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_1; // Reference to Text component for current level
    [SerializeField] private TextMeshProUGUI text_2; // Reference to Text component for next level
    [SerializeField] private Image barFill; // Reference to the fill image of the XP bar
    [SerializeField] private Image barOutline; // Reference to the outline image of the XP bar
    [SerializeField] private Image circle_1; // Reference to the first circle image
    [SerializeField] private Image circle_2; // Reference to the second circle image
    [SerializeField] private GameObject xpObject; // Reference to the XP object

    // References
    private PlayerStatus playerStatus;
    private PlayerUpgradeManager playerUpgrade;

    private float currentAmount = 0;
    private int currentLevel = 1;
    private int nextLevel = 2;
    private float experienceRequired = 100f;
    private static bool hasExitedUnity = false;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerUpgrade = GetComponent<PlayerUpgradeManager>();

        if (PlayerPrefs.HasKey("UpgradePoints"))
        {
            playerStatus.upgradePoints = PlayerPrefs.GetInt("UpgradePoints");
        }
        if (hasExitedUnity)
        {
            ResetExperience();
            hasExitedUnity = false;
        }
        else
        {
            LoadExperience();
        }
    }

    private void Update()
    {
        // Check if the 'G' key has been pressed (you can change the key or condition)
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Call the GainExperience function with a fixed XP value, for example: 50 XP
            GainExperience(50);
        }
    }

    void OnApplicationQuit()
    {
        hasExitedUnity = true;
        ResetUpgradePoints();
        ResetExperience();
    }

    private void ResetExperience()
    {
        currentAmount = 0;
        currentLevel = 1;
        nextLevel = 2;
        experienceRequired = 100;
        SaveExperience();
    }

    private void ResetUpgradePoints()
    {
        playerStatus.upgradePoints = 0;
        PlayerPrefs.SetInt("UpgradePoints", 0);
        PlayerPrefs.Save();
    }

    // Call this function when the player gains experience points
    public void GainExperience(float experience)
    {
        float experienceRatio = experience / experienceRequired;

        if (currentAmount + experienceRatio >= 1f)
        {
            LevelUp();
        }
        else
        {
            UpdateProgressBar(experienceRatio);
        }
    }

    // Function to update the progress bar
    private void UpdateProgressBar(float progress)
    {
        currentAmount += progress;

        // Update the fill amount of the progress bar
        barFill.fillAmount = currentAmount;

        // Update the Text displaying the current level and the next level
        text_1.text = currentLevel.ToString();
        text_2.text = nextLevel.ToString();
    }

    // Function to perform a level up
    // The LevelUp function updates
    private void LevelUp()
    {
        currentLevel++;
        nextLevel++;

        playerStatus.upgradePoints += playerUpgrade.upgradePointsOnLevelUp; // Increase upgrade points

        text_1.text = currentLevel.ToString();
        text_2.text = nextLevel.ToString();

        currentAmount = 0f;
        barFill.fillAmount = currentAmount;

        // Call the UpdateUI function in PlayerStatus to display the upgrade points
        playerStatus.UpdateUI();
        playerUpgrade.UpdateUI();

        // Update the required experience value for the new level
        experienceRequired = CalculateExperienceRequired(currentLevel);
        SaveExperience();
        // Save the experience and the new level in PlayerPrefs
        PlayerPrefs.SetInt("UpgradePoints", playerStatus.upgradePoints);
        PlayerPrefs.Save();
    }

    private float CalculateExperienceRequired(int level)
    {
        // Write code here to calculate the required experience based on the level
        return 100f * level; // For example: 100 XP for level 1, 200 XP for level 2, and so on.
    }

    public void SaveExperience()
    {
        PlayerPrefs.SetFloat("PlayerExperience", currentAmount);
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerNextLevel", nextLevel);
        PlayerPrefs.SetFloat("ExperienceRequired", experienceRequired);
        PlayerPrefs.Save();
    }

    public void LoadExperience()
    {
        if (PlayerPrefs.HasKey("PlayerExperience"))
        {
            currentAmount = PlayerPrefs.GetFloat("PlayerExperience");
            currentLevel = PlayerPrefs.GetInt("PlayerLevel");
            nextLevel = PlayerPrefs.GetInt("PlayerNextLevel");
            experienceRequired = PlayerPrefs.GetFloat("ExperienceRequired");
            barFill.fillAmount = currentAmount;
            text_1.text = currentLevel.ToString();
            text_2.text = nextLevel.ToString();
        }
    }
}
