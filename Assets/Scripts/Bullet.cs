using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float moveSpeed;
    Transform target;
    Rigidbody2D rb;
    string targetTag;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Attack(Transform transform,string tag)
    {
        moveSpeed = 1;
        target = transform;
        targetTag = tag;    
    }
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == targetTag)
        {
            transform.localPosition = new Vector2(0, 0);
            gameObject.SetActive(false);
        }
    }
}
