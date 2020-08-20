using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class DialogueScript
{
    [Serializable]
    public class Sequence
    {
        public string text;

        // Image? Character name?

        public Sequence(string text)
        {
            this.text = text;
        }
    }

    public List<Sequence> sequences = new List<Sequence>();
}
