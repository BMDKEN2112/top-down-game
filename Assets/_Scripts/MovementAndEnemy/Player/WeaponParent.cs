using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }

    public Animator animator;

    public int damage;

    public float delay = 0.3f;

    private bool attackBlocked;

    public bool isAttacking { get; private set; }

    public void ResetIsAttacking()
    {
        isAttacking = false;
    }

    public Transform circleOrigin;
    public float radius;


    private void Update()
    {
        if (isAttacking)
            return;
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;
        Vector2 scale = transform.localScale;

        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;
    }

    public void Attack()
    {
        if (attackBlocked)
        {
            return;
        }
        animator.SetTrigger("Attack");
        isAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            EnemyHealth enemyHealth;
            if (enemyHealth = collider.GetComponent<EnemyHealth>())
            {
                enemyHealth.GetHit(damage, transform.parent.gameObject);
            }
            RedEnemyHealth redEnemyHealth;
            if (redEnemyHealth = collider.GetComponent<RedEnemyHealth>())
            {
                redEnemyHealth.GetHit(damage, transform.parent.gameObject);
            }
        }
    }
}
