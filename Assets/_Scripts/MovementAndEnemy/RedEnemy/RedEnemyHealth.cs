using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RedEnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    public GameObject healthText;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int damage, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        currentHealth -= damage;
        if (currentHealth <= damage * 4)
        {
            RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
            textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            textTransform.SetParent(canvas.transform);
        }

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            Destroy(gameObject);
        }
    }

}
