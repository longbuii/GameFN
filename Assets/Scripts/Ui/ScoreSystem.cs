using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI myScore;
    public int scoreNum;
    [SerializeField] private GameObject scoreObject;
    void Start()
    {
        // Take to Coin form the PlayerPrefs
        if (!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin", 0);
        }

        scoreNum = PlayerPrefs.GetInt("Coin");
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddScore(10);
        }
    }
    void OnApplicationQuit()
    {
        // When App is close, It's update to ResetScore
        ResetScore();
    }
    public void AddScore(int amount)
    {

        scoreNum += amount;
        UpdateScoreUI();

        PlayerPrefs.SetInt("Coin", scoreNum);
        PlayerPrefs.Save(); 
    }

    public void ResetScore()
    {
        scoreNum = 100; // Put to value of Coin
        PlayerPrefs.SetInt("Coin", scoreNum);
        PlayerPrefs.Save(); // Save to change of PlayerPrefs
        UpdateScoreUI();
    }

    public int CalculateLuckyCoin(int baseCoinValue, float luckModifier)
    {
        if (Random.Range(0f, 1f) <= luckModifier)
        {
            return baseCoinValue * 2; // Double coin if lucky is high
        }
        return baseCoinValue; // 
    }

    public void UpdateScoreUI()
    {
        myScore.text = " " + scoreNum.ToString();
    }
    public bool TryPurchaseItem(int itemCost)
    {
        if (scoreNum >= itemCost)
        {
            // Enough to Scose, player can buy something to Shop.
            scoreNum -= itemCost;

            // Update UI after minus money of player
            UpdateScoreUI();

            PlayerPrefs.SetInt("Coin", scoreNum);
            PlayerPrefs.Save();

            return true;
        }
        else
        {
            return false;
        }
    }

}

