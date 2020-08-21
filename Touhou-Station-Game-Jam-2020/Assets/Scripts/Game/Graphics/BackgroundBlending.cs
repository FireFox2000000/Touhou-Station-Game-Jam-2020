using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BackgroundBlending : MonoBehaviour
{
    Renderer m_ren;
    bool m_fadeRunning = false;
    Texture2D m_currentTexture;

    [SerializeField]
    float m_bleadSpeed = 0.5f;

    public delegate void OnCompleteFn();

    void InitRen()
    {
        if (!m_ren)
            m_ren = GetComponent<Renderer>();
    }

    public void SetTexture(Texture2D tex)
    {
        InitRen();

        m_currentTexture = tex;
        m_ren.sharedMaterial.mainTexture = m_currentTexture;
    }

    public void KickTransition(Texture2D destText, OnCompleteFn onCompleteCallback)
    {
        Debug.Log("Kicking background transition");

        StartCoroutine(Fade(m_currentTexture, destText, onCompleteCallback));
        m_currentTexture = destText;
    }

    IEnumerator Fade(Texture2D startTex, Texture2D endTex, OnCompleteFn onCompleteCallback)
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

        onCompleteCallback();

        m_fadeRunning = false;
    }
}
