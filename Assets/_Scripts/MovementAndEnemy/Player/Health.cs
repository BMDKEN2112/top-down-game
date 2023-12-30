using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 10;
    [SerializeField] public FloatValueSO currentHealth;

    [SerializeField] private GameObject bloodParticle;

    [SerializeField] private Renderer renderer;
    [SerializeField] private float flashTime = 0.2f;

    Animator animator;
    [SerializeField]
    private bool isDead = false;
    bool isAlive = true;

    private void Start()
    {
        currentHealth.Value = 1;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the object: " + gameObject.name);
        }
        else
        {
            Debug.Log("Animator component found on: " + gameObject.name);
            animator.SetBool("isAlive", isAlive);
        }
        isDead = false;
    }

    public void Reduce(int damage)
    {
        currentHealth.Value -= damage / maxHealth;
        CreateHitFeedback();
        if (currentHealth.Value <= 0)
        {
            isDead = true;
            Destroy(gameObject);
            SceneManager.LoadSceneAsync(0);
        }
    }

    public void AddHealth(int healthBoost)
    {
        int health = Mathf.RoundToInt(currentHealth.Value * maxHealth);
        int val = health + healthBoost;
        currentHealth.Value = (val > maxHealth ? maxHealth : val / maxHealth);
    }

    private void CreateHitFeedback()
    {
        Instantiate(bloodParticle, transform.position, Quaternion.identity);
        StartCoroutine(FlashFeedback());
    }

    private IEnumerator FlashFeedback()
    {
        renderer.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(flashTime);
        renderer.material.SetInt("_Flash", 0);
    }

    private void Die()
    {
        Debug.Log("Died");

        if (animator != null)
        {
            currentHealth.Value = 0;
            animator.SetBool("isAlive", false);
        }
        else
        {
            Debug.LogError("Animator is null in the Die method.");
        }
    }
}

