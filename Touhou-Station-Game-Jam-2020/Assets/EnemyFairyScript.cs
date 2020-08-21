using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFairyScript : MonoBehaviour
{
    public Transform TargetTrans;
    public Transform thisTrans;
    public float Speed;
    public float MinDistanceToChase;
    public bool HitPlayer;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void PlayerWasHit()
    {
        HitPlayer = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (HitPlayer == false)
        {


            // thisTrans.LookAt(TargetTrans);
            if (Vector3.Distance(TargetTrans.position, thisTrans.position) <= MinDistanceToChase)
            {

                thisTrans.position = Vector3.MoveTowards(thisTrans.position, TargetTrans.position, Time.deltaTime * Speed);
            }
        }
        else
        {
            thisTrans.position = Vector3.MoveTowards(thisTrans.position, new Vector3(0f,1000f,0f), Time.deltaTime * 40f);
        }
    }
}
