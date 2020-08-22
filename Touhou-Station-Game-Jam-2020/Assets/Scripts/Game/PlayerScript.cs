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
    [SerializeField]
    PathMovement m_movementPath;
    Vector3[] m_path;
    float m_currentPathDistance = 0;
    [SerializeField]
    public float rotationSpeed = 6.0f;

    [System.Serializable]
    public class MovementStamps {
        public int MovementLength;
        public float RotationSpeed;
    }
    public MovementStamps[] Stamps;
    public int CurrentStamp;
    public int StampStatus;
    public float CameraVerticalPosition;
    public Transform CameraTrans;
    public float CameraSpeed;
    public SpriteRenderer PlayerSprite;
    public float HitRedValue;
    public float StunTimer;
    public HaniwaAnimation HaniwaAnimScript;
    public AudioSource SFX_GrabClay;
    public AudioSource SFX_HitByFairy;
    // Start is called before the first frame update
    void Start()
    {
        m_path = m_movementPath.GetPath();
    }
    public void IncrementHaniwa()
    {
        NumberOfSegments += 1;
        SFX_GrabClay.Play();
    }
    public void DecrementHaniwa()
    {
        if (StunTimer == 0f)
        {

            SFX_HitByFairy.Play();
            NumberOfSegments -= 1;
            HitRedValue = 0f;
            StunTimer = 4f;
            if (NumberOfSegments < 0)
            {
                GameOver();
            }
        }
    }
    public void GameOver()
    {
        SceneManager.LoadScene("MainGame");
    }


    public void CameraMovement()
    {
        CameraVerticalPosition = Mathf.Lerp(CameraVerticalPosition, PlayerTrans.position.y + 0.5f, Time.deltaTime* CameraSpeed);
        CameraTrans.position = new Vector3(CameraTrans.position.x, CameraVerticalPosition, CameraTrans.position.z);
        // CameraTrans.LookAt(PlayerTrans);
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        

        CameraMovement();
        if (m_movementPath)
        {

            float yPos = transform.position.y + Input.GetAxis("Vertical") * Speed * Time.deltaTime;
            float movementInput = Input.GetAxis("Horizontal");
            //int inputDirection = movementInput != 0 ? (int)Mathf.Sign(movementInput) : 0;


            if(StunTimer > 2f && StunTimer <= 4f)
            {
                movementInput = 0;
                StunTimer -= Time.deltaTime;
                yPos = transform.position.y - 2f * Speed * Time.deltaTime;
                PlayerSprite.color = new Color(PlayerSprite.color.r, 0f, 0f);
                HaniwaAnimScript.enabled = false;
                PlayerSprite.flipY = true;
            }

            if(StunTimer > 0f && StunTimer <= 2f)
            {
                StunTimer -= Time.deltaTime;
                if (HitRedValue < 1f)
                {
                    HitRedValue += Time.deltaTime * 0.5f;
                }
                else HitRedValue = 1f;
                PlayerSprite.color = new Color(PlayerSprite.color.r, HitRedValue, HitRedValue);

                PlayerSprite.flipY = false;
                HaniwaAnimScript.enabled = true;
            }


            if (StunTimer < 0f)
            {
                PlayerSprite.color = new Color(1f, 1f, 1f, 1f);
                StunTimer = 0f;
                PlayerSprite.flipY = false;
            }
            
            m_currentPathDistance += movementInput * HoriSpeed;

            // Brute forced, can be optimised by preprocessing the path lengths but meh
            float pathDistanceSqr = 0;
            float desiredPathDistanceSqr = m_currentPathDistance;// * m_currentPathDistance;
            Vector3 playerPosition = Vector3.zero;
            Quaternion optimalPlayerRotation = Quaternion.identity;

            // Search to find where we should be along the path
            for (int i = 1; i < m_path.Length; ++i)
            {
                float newPathDistanceSqr = pathDistanceSqr + (m_path[i] - m_path[i - 1]).magnitude;     // todo, use sqrMag cause much faster than regular mag.
                if (newPathDistanceSqr > desiredPathDistanceSqr)
                {
                    float t = (desiredPathDistanceSqr - pathDistanceSqr) / (newPathDistanceSqr - pathDistanceSqr);
                    playerPosition = Vector3.Lerp(m_path[i - 1], m_path[i], t);

                    Vector3 direction = (m_path[i] - m_path[i - 1]).normalized;
                    optimalPlayerRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-90, Vector3.up) * direction);    // Rotate the sprites so they actually face the camera

                    break;
                }

                pathDistanceSqr = newPathDistanceSqr;
            }

            
            transform.position = new Vector3(playerPosition.x, yPos, playerPosition.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, optimalPlayerRotation, rotationSpeed * Time.fixedDeltaTime);
            if (PlayerTrans.position.y < -8.5) PlayerTrans.position = new Vector3(PlayerTrans.position.x, -8.5f, PlayerTrans.position.z);
            if (PlayerTrans.position.y > -4f) PlayerTrans.position = new Vector3(PlayerTrans.position.x, -4f, PlayerTrans.position.z);
            if (Input.GetAxis("Horizontal") > 0) PlayerSprite.flipX = false;
            if (Input.GetAxis("Horizontal") < 0) PlayerSprite.flipX = true;
        }
        else  // Older movement system
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
