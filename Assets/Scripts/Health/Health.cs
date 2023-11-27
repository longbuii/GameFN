using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class Health : MonoBehaviour
{
  
    [Header("Health")]
    public float startingHealth = 10;
    public float maxHealth;

    public float MaxHealth => maxHealth;
    public float currentHealth { get; private set; }
    private bool dead;
    private Rigidbody2D rb;
    [SerializeField] private float damage;
    public event Action<float> OnHealthChanged;

    // Reference
    private Animator anim;
    private PlayerStatus playerStatus;


    [Header("Health Status")]
    public TextMeshProUGUI healthtxt;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int iFrameCount;
    private SpriteRenderer spriteRend;

    [Header("Component")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        playerStatus = GetComponent<PlayerStatus>();

        currentHealth = startingHealth;

        playerStatus.UpdateUI();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(5f); 
        }
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;

        if (playerStatus.Defense > 0)
        {
            float actualDamage = CalculateActualDamage(_damage);

            playerStatus.Defense -= actualDamage;

            playerStatus.Defense = Mathf.Max(playerStatus.Defense, 0);

            playerStatus.UpdateUI();
        }
        else
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Inuvunerability());
            SoundManager.instance.Playsound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                rb.bodyType = RigidbodyType2D.Static;

                foreach (Behaviour component in components)
                    component.enabled = false;
                anim.SetBool("Jump", false);
                anim.SetTrigger("die");
                dead = true;
                SoundManager.instance.Playsound(deathSound);
            }
        }
        OnHealthChanged?.Invoke(currentHealth);
    }

    float CalculateActualDamage(float rawDamage)
    {
        if (playerStatus != null)
        {
            float actualDamage = rawDamage;
            return actualDamage;
        }
        else
        {
            return rawDamage;
        }
    }

    public void Addheal(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        if (OnHealthChanged != null)
        {
            OnHealthChanged(currentHealth);
        }
    }

    public void IncreaseMaxHealth(float _value)
    {
        float previousMaxHealth = currentHealth - startingHealth;
        startingHealth += _value;
        currentHealth = startingHealth + previousMaxHealth;
    }

    public void Setheal(float _value)
    {
        startingHealth = _value;

    }

    public void Respawn()
    {
        Setheal(startingHealth); 
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.ResetTrigger("die");
        anim.Play("Moving");
        StartCoroutine(Inuvunerability());

        foreach (Behaviour component in components)
            component.enabled = true;
        TakeDamage(damage);
    }
    private IEnumerator Inuvunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < iFrameCount; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (iFrameCount * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (iFrameCount * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

}


