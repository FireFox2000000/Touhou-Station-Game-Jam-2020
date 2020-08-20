using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("MenuItem1 string = " + Localiser.Instance.GetLocalised("MenuItem1"));
        Debug.Log("MenuItem2 string = " + Localiser.Instance.GetLocalised("MenuItem2"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
