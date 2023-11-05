using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour
{
    public enum ChestType
    {
        InstantOpen, // The chest opens immediately when encountered and can give coins
        BossDefeatOpen, // The chest only opens after defeating a boss
        EmptyChest
    }
    public ChestType chestType;

    public int baseCoinValue = 10; // Basic coin value
    public float luckModifier = 0.1f; // A measure of luck
    public GameObject openedChestPrefab; // Prefab of the opened chest
    private bool isOpened = false; // Variable to determine if the chest is opened
    private bool isChestOpened = false;
    public bool hasBoss = false;
    private float openedTime;

    // References
    public HealthEnemyBoss healthEnemyBoss;
    private ScoreSystem scoreSystem;
    private Animator anim;

    public AudioClip chestSound;

    private void Start()
    {
        // Find the ScoreSystem in the same GameObject that contains the Chest script
        scoreSystem = GetComponent<ScoreSystem>();
        anim = GetComponent<Animator>();

        // Check the initial state of the chest and perform the corresponding behavior
        switch (chestType)
        {
            case ChestType.InstantOpen:
                // The chest opens immediately when the game starts
                if (isOpened)
                {
                    gameObject.SetActive(false);
                    if (openedChestPrefab != null)
                    {
                        Instantiate(openedChestPrefab, transform.position, transform.rotation);
                    }
                }
                break;

            case ChestType.BossDefeatOpen:
                // The chest only opens after defeating a boss, not visible initially
                gameObject.SetActive(false);
                break;
        }
    }

    private void Update()
    {
        if (isChestOpened && Time.time - openedTime > 5f)
        {
            // If the chest is opened and 5 seconds have passed, hide it
            gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (isChestOpened)
        {
            return;
        }
        switch (chestType)
        {
            case ChestType.InstantOpen:
                OpenInstantChest();
                break;

            case ChestType.BossDefeatOpen:
                OpenAfterBossDefeat();
                break;

            case ChestType.EmptyChest:
                // This is an empty chest, no action needed.
                // Close the empty chest
                isOpened = true;
                StartCoroutine(DisappearAfterDelay(5f));
                break;
        }
    }

    private IEnumerator DisappearAfterDelay(float delay)
    {
        // Wait for the specified delay before triggering the animation
        yield return new WaitForSeconds(delay);

        // Activate the open animation
        if (anim != null)
        {
            anim.SetBool("open", true); // "OpenChest" is the name of the Animation Clip
        }
        isChestOpened = true;

        // Record the time when the chest was opened
        openedTime = Time.time;
    }

    private void OpenInstantChest()
    {
        PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
        float chance = playerStatus.Luck / 100f;

        int receivedCoins;

        // Generate a random value in the range of 0-1 and compare it with the chance
        if (Random.Range(0f, 1f) <= chance)
        {
            receivedCoins = baseCoinValue * 2; // Double the coin value if luck is successful
        }
        else
        {
            receivedCoins = baseCoinValue; // Basic coin value if luck is not successful
        }

        // Add this number of coins to the player's score
        scoreSystem.AddScore(receivedCoins);
        Debug.Log("Received " + receivedCoins + " coins!");

        if (openedChestPrefab != null)
        {
            Instantiate(openedChestPrefab, transform.position, transform.rotation);
        }

        // Close the chest and display the opened chest
        isOpened = true;
        if (anim != null)
        {
            SoundManager.instance.Playsound(chestSound);
            anim.SetBool("open", true); // "OpenChest" is the name of the Animation Clip
        }
        isChestOpened = true;
    }

    public void OpenAfterBossDefeat()
    {
        Debug.Log("Is boss dead? " + healthEnemyBoss.isDead);
        if (healthEnemyBoss.isDead)
        {
            gameObject.SetActive(true);

            // Add code to handle chest opening after defeating the boss here
            int receivedCoins = baseCoinValue; // Basic coin value, no need to check chance
            Debug.Log("Received " + receivedCoins + " coins!");

            // Add this number of coins to the player's score
            scoreSystem.AddScore(receivedCoins);

            // Close the chest and display the opened chest (if there is an animation)
            isOpened = true;
            if (anim != null)
            {
                anim.SetBool("open", true); // "OpenChest" is the name of the Animation Clip
            }
            isChestOpened = true;
        }
    }
}
