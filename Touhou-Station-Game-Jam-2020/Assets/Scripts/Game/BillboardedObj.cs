using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardedObj : MonoBehaviour
{
    public Transform CameraTrans;
    private Transform thisTrans;
    // Start is called before the first frame update
    void Start()
    {
        thisTrans = this.gameObject.transform;
        if (CameraTrans == null) CameraTrans = GameObject.Find("Player/Player/Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        thisTrans.rotation = CameraTrans.rotation;
    }
}
