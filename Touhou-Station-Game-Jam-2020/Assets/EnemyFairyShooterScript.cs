using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFairyShooterScript : MonoBehaviour
{
    public int Counter;
    public Transform[] Sprays;
    public Transform BulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Counter += 1;

        switch(Counter){
            case 0:
                for (int a=0; a<Sprays.Length; a = a + 1)
                {
                    Instantiate(BulletPrefab, Sprays[a].position, Sprays[a].rotation);
                }
                
                break;
            case 6:
                for (int a = 0; a < Sprays.Length; a = a + 1)
                {
                    Instantiate(BulletPrefab, Sprays[a].position, Sprays[a].rotation);
                }
                break;
            case 12:
                for (int a = 0; a < Sprays.Length; a = a + 1)
                {
                    Instantiate(BulletPrefab, Sprays[a].position, Sprays[a].rotation);
                }
                break;
            case 18:
                for (int a = 0; a < Sprays.Length; a = a + 1)
                {
                    Instantiate(BulletPrefab, Sprays[a].position, Sprays[a].rotation);
                }
                break;
            case 24:
                for (int a = 0; a < Sprays.Length; a = a + 1)
                {
                    Instantiate(BulletPrefab, Sprays[a].position, Sprays[a].rotation);
                }
                break;
            case 50:
                Counter = 0;
                break;
        }
    }
}
