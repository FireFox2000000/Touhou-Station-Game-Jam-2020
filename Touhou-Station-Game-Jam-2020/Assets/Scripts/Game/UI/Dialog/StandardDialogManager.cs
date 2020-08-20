using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic dialog display in pages with no text text animation
/// </summary>
[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class StandardDialogManager : MonoBehaviour, IDialogueUI
{
    TMPro.TextMeshProUGUI text;

    string m_remainingSequenceText = string.Empty;

    void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public bool AdvanceSequence()
    {
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
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
