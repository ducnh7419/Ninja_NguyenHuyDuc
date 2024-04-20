using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected float hp;
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private CombatText combatTextPrefab;
    protected bool isDeath=>hp <= 0;
    private string currAnimName;
    
    public virtual void OnInit(){
        hp=100;
        healthBar.OnInit(100,transform);
    }

    public virtual void OnDespawn(){
        
    }

    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn),1.5f);
    }

    protected void ChangeAnim(string animName){
        if(animName!=currAnimName){
            anim.ResetTrigger(animName);
            currAnimName=animName;
            anim.SetTrigger(currAnimName);          
        }
    }

    private void Start() {
        OnInit();
    }
    

    public void OnHit(float damage){
        if(!isDeath){
            hp-=damage;
            if(isDeath){
                hp=0;
                OnDeath();
            }
            healthBar.setNewHp(hp);
            Instantiate(combatTextPrefab,transform.position+Vector3.up,Quaternion.identity).OnInit(damage);
        }
    }

    
}
