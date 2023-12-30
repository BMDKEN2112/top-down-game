using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public float timeToLive = 0.5f;

    public float floatSpeed = 500f;

    public Vector3 direction = new Vector3 (0, 1, 0);

    RectTransform rectTransform;

    public TextMeshProUGUI textMesh;

    Color startingColor;

    public float timeElapsed = 0.0f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startingColor = textMesh.color;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        rectTransform.position += direction * floatSpeed * Time.deltaTime;

        textMesh.color = new Color(startingColor.r, startingColor.g, startingColor.b, 1 - (timeElapsed / timeToLive));

        if (timeElapsed > timeToLive)
        {
            Destroy(gameObject);
        }
    }
}
