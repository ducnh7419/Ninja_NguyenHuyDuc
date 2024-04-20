using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField]private float attackRange;
    [SerializeField]private float moveSpeed;
    [SerializeField]private Rigidbody2D rb;
    private Character target;
    public Character Target => target;

    private IState currState;

    private bool isRight=true;

    [SerializeField] private GameObject attackArea;

    private void Update() {
        if (currState != null&&!isDeath){
            currState.OnExecute(this);
        }
    }
 

    public override void OnInit(){
        base.OnInit();
        ChangeState(new IdleState());
    }

    public override void OnDespawn(){
        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

    protected override void OnDeath(){
        ChangeState(null);
        base.OnDeath();
    }

    internal void SetTarget(Character character)
    {
        this.target=character;
        if(IsTargetInRange()){
            ChangeState(new AttackState());
        }else{
            if(Target!=null){
                Debug.Log("C");
                ChangeState(new PatrolState());
            }else{
                ChangeState(new IdleState());
            }
        }
    }

    public void Moving(){
        ChangeAnim("run");
        rb.velocity=transform.right*moveSpeed;
    }

    public void StopMoving(){
        ChangeAnim("idle");
        rb.velocity=Vector2.zero;
    }

    public void Attack(){
        ChangeAnim("attack");   
        ActiveAttack();
        Invoke(nameof(DeActiveAttack),.5f);
    }

    private void ActiveAttack(){
        attackArea.SetActive(true);
    }

    private void DeActiveAttack(){
        attackArea.SetActive(false);
    }

    public bool IsTargetInRange(){
        return target!=null && Vector2.Distance(target.transform.position,transform.position)<=attackRange;
    }

    public void ChangeState(IState newState){
        if(currState !=null){
            currState.OnExit(this);
        }
        currState = newState;
        if(currState!=null){
            currState.OnEnter(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag=="EnemyWall"){
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight=isRight;
        transform.rotation=isRight?Quaternion.Euler(Vector3.zero):Quaternion.Euler(Vector3.up*180);
    }

    
}
