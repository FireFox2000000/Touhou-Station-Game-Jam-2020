using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueUI
{
    /// <summary>
    /// Called to open/make the dialogue ui visible
    /// </summary>
    void Open();

    /// <summary>
    /// Called upon when we want to remove the dialogue ui
    /// </summary>
    void Close();

    /// <summary>
    /// Call this whenever the user wants to advance the text
    /// </summary>
    /// <returns>Returns true if the sequence could not advance and has already been completed.</returns>
    bool AdvanceSequence();

    /// <summary>
    /// Called to set the next text block, images, etc
    /// </summary>
    /// <param name="sequence"></param>
    void SetCurrentSequence(DialogueScript.Sequence sequence);
}
