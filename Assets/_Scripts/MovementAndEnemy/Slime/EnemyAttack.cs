using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 1;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Deal damage to the player (you might want to have a separate PlayerHealth script)
            collision.collider.GetComponent<Health>().Reduce(damage);
        }
    }
}
