﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BackgroundBlending : MonoBehaviour
{
    Renderer m_ren;
    bool m_fadeRunning = false;
    Texture m_currentTexture;
    Texture m_initTexture;

    [SerializeField]
    float m_bleadSpeed = 0.5f;

    public delegate void OnCompleteFn();

    void InitRen()
    {
        if (!m_ren)
            m_ren = GetComponent<Renderer>();

        if (!m_currentTexture)
            m_currentTexture = m_ren.sharedMaterial.mainTexture;

        if (!m_initTexture)
            m_initTexture = m_ren.sharedMaterial.mainTexture;

        Debug.Log(m_currentTexture);
    }

    public void SetTexture(Texture2D tex)
    {
        InitRen();

        m_currentTexture = tex;
        m_ren.sharedMaterial.mainTexture = m_currentTexture;
    }

    public void KickTransition(Texture2D destText, OnCompleteFn onCompleteCallback, float onCompleteDelay = 0)
    {
        Debug.Log("Kicking background transition");

        InitRen();

        StartCoroutine(Fade(m_currentTexture, destText, onCompleteCallback, onCompleteDelay));
        m_currentTexture = destText;
    }

    IEnumerator Fade(Texture startTex, Texture2D endTex, OnCompleteFn onCompleteCallback, float endDelay)
    {
        InitRen();

        m_fadeRunning = true;

        float t = 0;

        m_ren.sharedMaterial.mainTexture = startTex;
        m_ren.sharedMaterial.SetTexture("_BlendTex", endTex);

        while (t <= 1)
        {
            t += Time.deltaTime * m_bleadSpeed;

            m_ren.sharedMaterial.SetFloat("_Blend", t);

            yield return null;
        }

        m_ren.sharedMaterial.SetFloat("_Blend", 0);
        m_ren.sharedMaterial.mainTexture = endTex;

        yield return new WaitForSeconds(endDelay);

        onCompleteCallback();

        m_fadeRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        const float bleedCompensation = 0.05f;
        // Adjust size for variable aspect ratios
        float quadHeight = Camera.main.orthographicSize * 2.0f;
        float quadWidth = quadHeight * Screen.width / Screen.height;
        transform.localScale = new Vector3(quadWidth + bleedCompensation, quadHeight + bleedCompensation, transform.localScale.z);
    }

    void OnApplicationQuit()
    {
        if (m_ren)
            m_ren.sharedMaterial.mainTexture = m_initTexture;
    }

    void OnDestroy()
    {
        if (m_ren)
            m_ren.sharedMaterial.mainTexture = m_initTexture;
    }
}
