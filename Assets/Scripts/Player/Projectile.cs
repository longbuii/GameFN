using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectie : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private BoxCollider2D boxCollider;
    private Animator aim;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        aim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
            return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed,0,0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) 
            gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        hit = true;
        boxCollider.enabled = false;
        aim.SetTrigger("explode");

        if (col.tag == "Enemy")
            col.GetComponent<Health>().TakeDamage(1);
    }
    public void setDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactive()
    {
        gameObject.SetActive(false);
    }
}
