using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision) {       
        Debug.Log(collision.tag);
        if(collision.CompareTag("Player")||collision.CompareTag("Enemy")){
            collision.GetComponent<Character>().OnHit(30f);
        }
    }
}