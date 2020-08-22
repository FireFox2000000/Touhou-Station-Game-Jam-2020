using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Transform BulletTrans;
    public float BulletSpeed;
    public float BulletDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BulletDuration -= Time.deltaTime;
        if (BulletDuration <= 0) Destroy(this.gameObject);
        BulletTrans.Translate(Vector3.forward * BulletSpeed, Space.Self);

    }
}
