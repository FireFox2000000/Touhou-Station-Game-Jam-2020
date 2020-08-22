using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic dialog display in pages with no text text animation
/// </summary>
public class StandardDialogManager : MonoBehaviour, IDialogueUI
{
    TMPro.TextMeshProUGUI text;

    string m_remainingSequenceText = string.Empty;

    [SerializeField]
    Texture2D[] m_backgroundRefs;
    [SerializeField]
    BackgroundBlending m_backgroundManager;

    bool m_blockingEnabled = false;

    void Awake()
    {
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
    }

    public bool AdvanceSequence()
    {
        if (m_blockingEnabled) return false;

        Debug.Log(string.Format("Dialogue advance sequence. Current page ({0}/{1})", text.pageToDisplay, text.textInfo.pageCount));

        if (text.pageToDisplay < text.textInfo.pageCount)
        {
            ++text.pageToDisplay;
            return false;
        }
        else
        {
            text.pageToDisplay = 1;     // Reset back to the first page, otherwise we might be on an invalid page for the next sequence
            return true;
        }
    }

    public void SetCurrentSequence(DialogueScript.Sequence sequence)
    {
        text.text = sequence.text;

        Debug.Log("Sequence text = " + sequence.text);

        Texture2D bgTex = null;
        if (!string.IsNullOrEmpty(sequence.background_image))
        {
            foreach(Texture2D tex in m_backgroundRefs)
            {
                if (string.Equals(tex.name, sequence.background_image))
                {
                    bgTex = tex;
                    break;
                }
            }

            Debug.Assert(bgTex, "Unable to find a background image called " + sequence.background_image + ". Texture may not be referenced in the scene.");
        }

        if (bgTex && !m_backgroundManager)
        {
            Debug.LogError("Dialogue sequence has requested a background image swap, however no background manager has been provided.");
        }

        if (m_backgroundManager && bgTex)
        {
            m_blockingEnabled = true;

            Close();
            m_backgroundManager.KickTransition(bgTex, OnBackgroundSwapComplete);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void OnBackgroundSwapComplete()
    {
        Open();
        m_blockingEnabled = false;
    }
}
