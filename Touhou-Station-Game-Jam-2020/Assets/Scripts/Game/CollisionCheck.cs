using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public PlayerScript ThisPlayerScript;

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision test!");
        if (collision.gameObject.tag == "Clay")
        {

            ThisPlayerScript.IncrementHaniwa();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            ThisPlayerScript.DecrementHaniwa();
            //    collision.gameObject.GetComponent<EnemyFairyScript>().PlayerWasHit(); yeah i'm just gonna take this one out it's not needed i think lol
        }
        if (collision.gameObject.tag == "Akyuu")
        {
            // Function To Call Ending Cutscene
        }
    }
    
}
