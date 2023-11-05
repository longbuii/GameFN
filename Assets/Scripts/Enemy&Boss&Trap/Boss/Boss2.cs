using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private int takedamage;
    [SerializeField] private int enraged;
    public float attackRange = 1.5f;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Enemy Move")]
    [SerializeField] private float agroRange;
    [SerializeField] private float moveSpeed;
    private bool movementEnabled = true; 

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float knockbackForce;

    public Animator anim;
    private Health playerHealth;   

    public bool isAttacking;
    public bool isInvulnerable = false;
    public bool isFlipped = false;

    public Transform player;
    public Rigidbody2D rb2d;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= agroRange)
        {
         
                if (PlayerInSight())
                {   
                    if (cooldownTimer >= attackCooldown)
                    {
                        cooldownTimer = 0;
                        anim.SetTrigger("Attack"); 
                        isAttacking = true;
                        rb2d.velocity = Vector2.zero;
                        anim.SetBool("Moving", false);
                        StartCoroutine(EnableMovementAfterAttack());
                    }
                }
    
                ChasePlayer();
            
                                 
        }
        else
        {
            StopChasePlayer();
        }
    }

    void ChasePlayer()
    {
        if (movementEnabled && !isAttacking)
        {
            if (transform.position.x < player.position.x)
            {
                rb2d.velocity = new Vector2(moveSpeed, 0);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (transform.position.x > player.position.x)
            {
                rb2d.velocity = new Vector2(-moveSpeed, 0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            anim.SetBool("Moving", true);
        }

    }

    void StopChasePlayer()
    {
        if (movementEnabled)
        {
            rb2d.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }
    }


    public void DisableMovement()
    {
        movementEnabled = false;
        isAttacking = true;
        Debug.Log("Movement Disabled");

    }

    public void EnableMovement()
    {
        isAttacking = false;
        movementEnabled = true;
        Debug.Log("Movement EnableMovement");

    }


    private bool PlayerInSight()
    {

            RaycastHit2D hit =
                Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
                0, Vector2.left, 0, playerLayer);

            if (hit.collider != null)
                playerHealth = hit.transform.GetComponent<Health>();

            return hit.collider != null;

    }
    private IEnumerator EnableMovementAfterAttack()
    {

        // Wait for a certain amount of time (you can adjust this as needed).
        yield return new WaitForSeconds(1.0f); // Wait for 1 second as an example.

        // Enable movement or perform other actions here.
        EnableMovement();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, agroRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);

            Vector2 knockbackDirection = (player.position - transform.position).normalized;
            rb2d.AddForce(-knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }


    private void EnragedAttack()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(enraged);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(takedamage);
            }
        }
    }

}