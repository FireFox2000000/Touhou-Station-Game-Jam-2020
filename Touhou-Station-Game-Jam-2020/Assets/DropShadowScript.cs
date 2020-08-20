using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadowScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform ShadowTrans;
    public float ShadowTransHeight;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShadowTrans.position = new Vector3(ShadowTrans.position.x, ShadowTransHeight, ShadowTrans.position.z);
    }
}
