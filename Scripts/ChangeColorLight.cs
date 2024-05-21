using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorLight : MonoBehaviour
{
    [SerializeField] private float speedChange = 2f;
    private Light spotLight;
    private float timer;
    private Color targetColor;

    private void Start()
    {
        spotLight = GetComponent<Light>();
        targetColor = new Color(GetValue, GetValue, GetValue);
    }

    private void Update()
    {
        timer += Time.deltaTime * speedChange;
        spotLight.color = Color.Lerp(spotLight.color, targetColor, timer);
        if (timer > 1f)
        {
            targetColor = new Color(GetValue, GetValue, GetValue);
            timer = 0;
        }
    }

    private float GetValue
    {
        get
        {
            return Random.value * 0.25f + 0.75f;
        }
    }
}
