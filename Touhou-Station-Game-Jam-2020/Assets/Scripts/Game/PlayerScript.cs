using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerScript : MonoBehaviour
{
    public Transform PlayerTrans;
    public int NumberOfSegments;
    public Transform[] Segments;
    public Vector3[] PositionData;
    public Quaternion[] RotationData;
    public float Speed;
    public float HoriSpeed;
    public int SegmentDistance;


    [System.Serializable]
    public class MovementStamps {
        public int MovementLength;
        public float RotationSpeed;
    }
    public MovementStamps[] Stamps;
    public int CurrentStamp;
    public int StampStatus;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void IncrementHaniwa()
    {
        NumberOfSegments += 1;
    }
    public void DecrementHaniwa()
    {
        NumberOfSegments -= 1;
        if (NumberOfSegments < 0)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        SceneManager.LoadScene("MainGame");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // Player Movement Control
        PlayerTrans.position = new Vector3(PlayerTrans.position.x, PlayerTrans.position.y + (Input.GetAxis("Vertical") * Speed * Time.deltaTime), PlayerTrans.position.z);
        if (PlayerTrans.position.y < -8.5) PlayerTrans.position = new Vector3(PlayerTrans.position.x, -8.5f, PlayerTrans.position.z);
        if (PlayerTrans.position.y > -1.5) PlayerTrans.position = new Vector3(PlayerTrans.position.x, -1.5f, PlayerTrans.position.z);
        if (Input.GetAxis("Horizontal") > 0)
        {

            // if (StampStatus < Stamps[CurrentStamp].MovementLength && CurrentStamp== Stamps.Length-1)
            // {



                StampStatus += 1;
                transform.Translate(Vector3.right * HoriSpeed, Space.Self);
                PlayerTrans.Rotate(0, Stamps[CurrentStamp].RotationSpeed, 0, Space.World);
                if (StampStatus > Stamps[CurrentStamp].MovementLength)
                {
                    CurrentStamp += 1;
                    StampStatus = 0;
                }
            // }
            

        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            // if (StampStatus > -1 && CurrentStamp == 0)
            // {
                StampStatus -= 1;
                transform.Translate(Vector3.right * -HoriSpeed, Space.Self);
                PlayerTrans.Rotate(0, -Stamps[CurrentStamp].RotationSpeed, 0, Space.World);
                if (StampStatus < 0)
                {
                    CurrentStamp -= 1;
                    StampStatus = Stamps[CurrentStamp].MovementLength;
                }
            // }
            
        }
        




        // Saving previous player positions to create a path for the other segments to follow along
        bool SaveData = false;
        if (Input.GetAxis("Horizontal") != 0) SaveData = true; if (Input.GetAxis("Vertical") != 0) SaveData = true;

        if (SaveData == true)
        {
            for (int a= PositionData.Length-2; a>-1; a = a - 1)
            {
                
                    PositionData[a + 1] = PositionData[a];
                    RotationData[a + 1] = RotationData[a];

            }
            PositionData[0] = PlayerTrans.position;
            RotationData[0] = PlayerTrans.rotation;
        }


        for (int a=1; a<Segments.Length; a = a + 1)
        {
            if (a < NumberOfSegments+1)
            {
                Segments[a].gameObject.SetActive(true);
                Segments[a].position = PositionData[a * SegmentDistance];
                Segments[a].rotation = RotationData[a * SegmentDistance];
            }
            else
            {
                Segments[a].gameObject.SetActive(false);
            }
        }
        
        



    }
}
