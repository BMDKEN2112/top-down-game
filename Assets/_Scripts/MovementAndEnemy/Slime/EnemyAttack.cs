using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 1;

    public DetectionZone detectionZone;

    public float moveSpeed = 500f;

    Rigidbody2D rb;

    Animator animator;

    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isMoving", isMoving);
    }

    private void FixedUpdate()
    {
        if (detectionZone.detectedObjs.Count > 0)
        {
            Vector2 direction = (detectionZone.detectedObjs[0].transform.position - transform.position).normalized;
            animator.SetBool("isMoving", true);
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Deal damage to the player (you might want to have a separate PlayerHealth script)
            collision.collider.GetComponent<Health>().Reduce(damage);
        }
    }
}
