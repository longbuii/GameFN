using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemyBoss : MonoBehaviour
{
    [SerializeField] private float health, maxHealth, healthEnrage;
    [SerializeField] BarEmeny barEnemy;
    private Health playerHealth;

    [Header("Others")]
    public int coinValue = 10;
    public ScoreSystem scoreSystem; 
    public XpBossManager xpBossManager; 
    public PlayerStatus playerStatus;
    public GameObject chest;
    public Animator anim;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    public bool isInvulnerable = false;
    public bool isDead = false;
    public Transform player;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        barEnemy = GetComponentInChildren<BarEmeny>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
        barEnemy.UpdateHealthBar(health, maxHealth);

    }
    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;
        SoundManager.instance.Playsound(hurtSound);
        barEnemy.UpdateHealthBar(health, maxHealth);
        anim.SetTrigger("Hurt");

        if (health <= healthEnrage)
        {

            anim.SetBool("IsEnraged", true);
        }

        if (health <= 0)

        {
            if (!isDead)
            {
                Die();
            }
        }
    }


    void Die()
    {                
        SoundManager.instance.Playsound(deathSound);
        Debug.Log("Boss is dead");
        anim.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        enabled = false;

        if (xpBossManager != null)
        {
            xpBossManager.GiveExperienceToPlayer();
            Debug.Log("Gained " + xpBossManager.xpValue + " experience.");
        }

        if (scoreSystem != null)
        {
            int luckyCoinValue = scoreSystem.CalculateLuckyCoin(coinValue, playerStatus.Luck / 100f);
            scoreSystem.AddScore(luckyCoinValue);
            Debug.Log("Added " + luckyCoinValue + " to score.");
        }
        isDead = true;
        chest.gameObject.SetActive(true);

    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }


}
