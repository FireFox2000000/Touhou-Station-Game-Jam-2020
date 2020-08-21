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
    public bool FadeOnAppear;
    public float FadeVar;
    // Start is called before the first frame update
    void Start()
    {
        if (FadeOnAppear == true)
            FadeVar = 0;
        else FadeVar = 1f;

        AnimationRenderer.color = new Color(AnimationRenderer.color.r, AnimationRenderer.color.g, AnimationRenderer.color.b, FadeVar);
        if (AnimationRenderer == null) AnimationRenderer = this.GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {

        if (FadeOnAppear == true)
            FadeVar = 0;
        else FadeVar = 1f;

        AnimationRenderer.color = new Color(AnimationRenderer.color.r, AnimationRenderer.color.g, AnimationRenderer.color.b, FadeVar);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FadeVar < 1f)
        {
            FadeVar += Time.deltaTime;
        }
        else FadeVar = 1f;
        AnimationRenderer.color = new Color(AnimationRenderer.color.r, AnimationRenderer.color.g, AnimationRenderer.color.b, FadeVar);
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
