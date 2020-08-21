using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIServices : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<UIFadeBlocker>(true).FadeOut(() => { });     // We assume we're coming from the frontend state
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
