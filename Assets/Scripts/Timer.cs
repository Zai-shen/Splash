using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    public float maxHeight = 143f;
    private bool finishReached = false;
    
    void Start()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!finishReached)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
            _textMeshPro.SetText($"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
            float interpolation = transform.parent.position.y / maxHeight;
            _textMeshPro.fontMaterial.SetFloat("_GlowPower", interpolation);
        }

        if (transform.parent.position.y >= (maxHeight - 1) && !finishReached)
        {
            _textMeshPro.color = new Color(0, 1, 1, 0.75f);
            finishReached = true;
        }
    }
}
