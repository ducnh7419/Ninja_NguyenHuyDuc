using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : IState
{
    float timer;
    public void OnEnter(Enemy enemy)
    {
        if (enemy.Target!=null){
            // doi huong enemy toi huong player
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
            enemy.StopMoving();
            enemy.Attack();
        }
        timer=0;
    }

    public void OnExecute(Enemy enemy)
    {
        if(enemy.Target==null){
            timer+=Time.deltaTime;
        }
        
        if (timer>=1.5f){
            Debug.Log("A");
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
