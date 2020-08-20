using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Transform PlayerTrans;
    public int NumberOfSegments;
    public Transform[] Segments;
    public Vector3[] PositionData;
    public Vector3[] RotationData;
    public float Speed;
    public int SegmentDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Player Movement Control
        PlayerTrans.position = new Vector3(PlayerTrans.position.x + (Input.GetAxis("Horizontal") * Speed * Time.deltaTime), PlayerTrans.position.y + (Input.GetAxis("Vertical") * Speed * Time.deltaTime), PlayerTrans.position.z);

        // Saving previous player positions to create a path for the other segments to follow along
        bool SaveData = false;
        if (Input.GetAxis("Horizontal") != 0) SaveData = true; if (Input.GetAxis("Vertical") != 0) SaveData = true;

        if (SaveData == true)
        {
            for (int a= PositionData.Length-2; a>-1; a = a - 1)
            {
                
                    PositionData[a + 1] = PositionData[a];
                
                
            }
            PositionData[0] = PlayerTrans.position;
        }


        for (int a=1; a<Segments.Length; a = a + 1)
        {
            Segments[a].position = PositionData[a* SegmentDistance];
        }
        
        



    }
}
