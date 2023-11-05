using UnityEngine;

public class Healthcollectible : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float healthvalue;
    [SerializeField] private bool isHealthRefill;
    [SerializeField] private bool increaseMaxHealth;
    [SerializeField] private float maxHealthIncreaseValue;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Health playerHealth = col.GetComponent<Health>();

            // Refill to Health
            if (isHealthRefill)
            {
                playerHealth.Addheal(healthvalue); 
            }
            // Increase to MaxHealth
            else if (increaseMaxHealth)
            {
                playerHealth.IncreaseMaxHealth(maxHealthIncreaseValue); 
            }

            playerHealth.Addheal(healthvalue); 

            gameObject.SetActive(false);
        }
    }



}
