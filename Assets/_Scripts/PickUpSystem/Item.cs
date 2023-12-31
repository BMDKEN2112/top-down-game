using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]

    public ItemSO inventoryItem { get; private set; }

    [field: SerializeField]
    public int quantity { get; set; } = 1;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float duration = 0.3f;

    private void Start()
    {
        if (inventoryItem != null && inventoryItem.ItemImage != null)
        {
            GetComponent<SpriteRenderer>().sprite = inventoryItem.ItemImage;
        }
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimatePickUp());
    }

    private IEnumerator AnimatePickUp()
    {
        audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
