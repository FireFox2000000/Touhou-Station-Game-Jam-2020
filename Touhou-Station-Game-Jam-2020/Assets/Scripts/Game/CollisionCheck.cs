using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public PlayerScript ThisPlayerScript;
    public AudioSource SFX_GrabClay;
        public AudioSource SFX_HitByFairy;
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision test!");
        if (collision.gameObject.tag == "Clay")
        {
            SFX_GrabClay.Play();
            ThisPlayerScript.IncrementHaniwa();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            SFX_HitByFairy.Play();
            ThisPlayerScript.DecrementHaniwa();
            collision.gameObject.GetComponent<EnemyFairyScript>().PlayerWasHit();
            }
        
    }
    
}
