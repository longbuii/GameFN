using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class DefenseBar : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Image totalDefenseBar;
    [SerializeField] private Image currentDefenseBar;
    [SerializeField] private TextMeshProUGUI defenseText;

    void Start()
    {

    }

    void Update()
    {
        float currentDefense = playerStatus.Defense;
        float maxDefense = playerStatus.MaxDefense;

        if (maxDefense > 0)
        {
            currentDefenseBar.fillAmount = currentDefense / maxDefense;
        }
        else
        {
            currentDefenseBar.fillAmount = 0f; 
        }

        defenseText.text = currentDefense.ToString();
    }
}