using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaniwaAnimation : MonoBehaviour
{
    public Sprite[] AnimationFrames;
    public int CurrentFrame;
    public SpriteRenderer AnimationRenderer;
    public float Speed;
    private float Timer;
    // Start is called before the first frame update
    void Start()
    {
        AnimationRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Timer += Speed * Time.deltaTime;
        if (Timer >= 1)
        {
            Timer = 0;
            CurrentFrame += 1;
            if (CurrentFrame >= AnimationFrames.Length)
            {
                CurrentFrame = 0;
            }
        
        }
        AnimationRenderer.sprite = AnimationFrames[CurrentFrame];
    }
}
