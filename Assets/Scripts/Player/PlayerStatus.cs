using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;
    [SerializeField] private float defense;
    [SerializeField] private float luck;
    [SerializeField] private float critRate;
    [SerializeField] public float MaxDefense; 
    [SerializeField] private GameObject status; 

    public TextMeshProUGUI defensetxt;
    public TextMeshProUGUI lucktxt;
    public TextMeshProUGUI critratetxt;
    public TextMeshProUGUI speedtxt;
    public TextMeshProUGUI damagetxt;
    public TextMeshProUGUI healthtxt;

    // References
    private PlayerMove playerMove; 
    private PlayerAttack playerAttack;
    private Health health; 

    public int upgradePoints; 
    public TextMeshProUGUI upgradePointstxt; 
    private bool isStatusOpen = false; 
    public bool canlevelup = false;


    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        health = GetComponent<Health>();
        UpdateUI();
        health.OnHealthChanged += UpdateHealthInStatus;
        status.SetActive(false);
        canlevelup = false;

        if (instance == null)
        {
            instance = this;
        }

        if (PlayerPrefs.HasKey("PlayerDefense"))
        {
            defense = PlayerPrefs.GetFloat("PlayerDefense");
        }
        if (PlayerPrefs.HasKey("PlayerLuck"))
        {
            luck = PlayerPrefs.GetFloat("PlayerLuck");
        }
        if (PlayerPrefs.HasKey("PlayerCrit"))
        {
            critRate = PlayerPrefs.GetFloat("PlayerCrit");
        }
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            health.startingHealth = PlayerPrefs.GetFloat("PlayerHealth");
        }
        if (PlayerPrefs.HasKey("UpgradePoints"))
        {
            upgradePoints = PlayerPrefs.GetInt("UpgradePoints");
        }

        UpdateUI();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleStatus(); 
            UpdateUI(); 
        }
    }

    private void ToggleStatus()
    {
        isStatusOpen = !isStatusOpen;

        // If Status is Open, PlayerAttack isn't active to Attack.
        if (isStatusOpen)
        {
            if (playerAttack != null)
            {
                playerAttack.enabled = false;
            }

            // Active to "Infor" when status is open.
            status.SetActive(true);
        }
        else
        {
            if (playerAttack != null)
            {
                playerAttack.enabled = true;
            }

            // Don't active to "Infor" when status is close.
            status.SetActive(false);
        }

        // Update to status when status is change something.
        UpdateUI();
    }

    void OnApplicationQuit()
    {
                
        Reset();

    }

    public void Reset()
    {
        defense = 10;
        PlayerPrefs.SetFloat("PlayerDefense", defense);
        luck = 10;
        PlayerPrefs.SetFloat("PlayerLuck", luck);
        critRate = 10;
        PlayerPrefs.SetFloat("PlayerCrit", critRate);
        PlayerPrefs.SetInt("UpgradePoints", upgradePoints);
        PlayerPrefs.Save(); 
        UpdateUI();
    }



    public void SetCanLevelUp(bool value)
    {
        canlevelup = value;
    }
    public void UpdateUI()
    {
        defensetxt.text = "Defense: " + defense.ToString();
        lucktxt.text = "Lucky: " + luck.ToString("F0") + "%";
        critratetxt.text = "Crit Rate: " + critRate.ToString("F0") + "%";

        // Display to value of  Point on Status
        if (upgradePointstxt != null)
        {
            upgradePointstxt.text = "Your Points: " + upgradePoints.ToString();

        }
        // Display to value of Speed 
        if (playerMove != null && speedtxt != null)
        {
            speedtxt.text = "Speed: " + playerMove.speed.ToString(); // Sử dụng playerMove.speed để lấy giá trị speed từ PlayerMove.cs
        }
        // Display to value of Damage 

        if (playerAttack != null && damagetxt != null)
        {
            damagetxt.text = "Damage: " + playerAttack.damage.ToString();
        }
        // Display to value of Health 

        if (health != null && healthtxt != null)
        {
            healthtxt.text = "Health: " + health.currentHealth.ToString();
        }
    }

    void UpdateHealthInStatus(float newHealth)
    {
        healthtxt.text = "Health: " + newHealth.ToString();
    }

    // Create to value of Defense
    public float Defense
    {
        get { return defense; }
        set
        {
            defense = value;
            PlayerPrefs.SetFloat("PlayerDefense", defense);
            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    // Create to value of Luck
    public float Luck
    {
        get { return luck; }
        set
        {
            luck = value;
            PlayerPrefs.SetFloat("PlayerLuck", luck);
            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    // Create to value of CriRate
    public float CritRate
    {
        get { return critRate; }
        set
        {
            critRate = value;
            PlayerPrefs.SetFloat("PlayerCrit", critRate);
            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    // Update to value of Defense
    public void AddDefense(float value)
    {
        defense += value;
        if (defense > MaxDefense)
        {
            defense = MaxDefense;
        }

        PlayerPrefs.SetFloat("PlayerDefense", defense);
        PlayerPrefs.Save();

        UpdateUI();
    }
}
