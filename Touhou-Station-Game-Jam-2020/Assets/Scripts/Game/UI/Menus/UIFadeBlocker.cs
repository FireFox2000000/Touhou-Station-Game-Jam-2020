﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIFadeBlocker : MonoBehaviour
{
    Image[] m_images = null;

    [SerializeField]
    float m_fadeSpeed = 0.5f;

    public float fadeSpeed { get { return m_fadeSpeed; } }

    public delegate void OnCompleteFn();

    void Init()
    {
        if (m_images == null)
            m_images = GetComponentsInChildren<Image>();
    }

    public void FadeIn(OnCompleteFn onCompleteCallback)
    {
        gameObject.SetActive(true);
        StartCoroutine(Fade(1, onCompleteCallback));
    }

    public void FadeOut(OnCompleteFn onCompleteCallback)
    {
        gameObject.SetActive(true);
        StartCoroutine(Fade(-1, onCompleteCallback));
    }

    IEnumerator Fade(int fadeDir, OnCompleteFn onCompleteCallback)
    {
        Init();

        float alpha = fadeDir > 0 ? 0 : 1;

        do
        {
            alpha += Time.deltaTime * m_fadeSpeed * fadeDir;

            alpha = Mathf.Clamp01(alpha);

            foreach (var image in m_images)
            {
                Color colour = image.color;
                colour.a = alpha;
                image.color = colour;
            }

            yield return null;
        }
        while (alpha < 1 && alpha > 0);

        onCompleteCallback();

        if (alpha <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
