using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded=false;
    private bool isJumping;
    private bool isAttack;
    private bool isRunning;
    private float horizontal;
    [SerializeField] private float speed=5;
    [SerializeField]private float jumpForce;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;
    private int coin=0;
    private Vector3 savePoint;  

    private void Awake(){
        coin=PlayerPrefs.GetInt("coin", 0);
    }
    
    

    // Update is called once per frame
    void Update()
    {       
        if(isDeath){
            return;
        }
        isGrounded=CheckGrounded();
        horizontal=Input.GetAxisRaw("Horizontal");
        if(isGrounded){
            Debug.Log("G");
            if(isJumping){
                return;
            }
            //jump
            if(Input.GetKeyDown(KeyCode.Space)){
                Jump();       
            }
            
            //change anim run
            if(Mathf.Abs(horizontal)>0.1f){
                ChangeAnim("run");
            }

            //attack
            if(Input.GetKeyDown(KeyCode.C)){
                Attack();
            }
            //throw
            if(Input.GetKeyDown(KeyCode.V)){
                Throw();
            }
        }
        // Check Falling
        if(!isGrounded&&rb.velocity.y<0){
                ChangeAnim("fall");
                isJumping=false;
        }

        if(isRunning&&isGrounded){
            if(Input.GetKeyDown(KeyCode.DownArrow)){
                Slide();
            }
        }

        //Moving
        if(Mathf.Abs(horizontal)>0.1f){
            rb.velocity=new Vector2(horizontal*Time.deltaTime*speed,rb.velocity.y);
            //horizontal>0=> tra ve 0, con khong thi tra ve 180
            transform.rotation=Quaternion.Euler(new Vector3(0,horizontal>0 ? 0 : 180,0));
            // transform.localScale=new Vector3(horizontal,1,1);
            isRunning=true;
        }else if(isGrounded){
            ChangeAnim("idle");
            rb.velocity=Vector2.zero;
            isRunning=false;
        }
    }

    override public void OnInit(){
        base.OnInit();
        ChangeAnim("idle");
        isRunning=false;
        attackArea.SetActive(false);
        isAttack=false;             
        transform.position=savePoint;
        if(isGrounded)
            SavePoint();
        UIManager.instance.SetCoin(0);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Coin"){
            coin++;
            UIManager.instance.SetCoin(coin);
            Destroy(other.gameObject);
            PlayerPrefs.SetInt("coin",coin);        
        }
        if(other.tag=="DeathZone"){
            OnDeath();         
        }
    }

    public override void OnDespawn(){
        base.OnDespawn();
        // OnInit();
    }
    protected override void OnDeath(){
        base.OnDeath();
        Invoke(nameof(OnInit),0.3f);
    }

    private bool CheckGrounded(){
        Debug.DrawLine(transform.position,transform.position+Vector3.down*0.85f,Color.red);
        RaycastHit2D hit= Physics2D.Raycast(transform.position,Vector3.down,0.85f,groundLayer);
        return hit.collider!=null;
    }

    public void Jump(){
        isJumping=true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce*Vector2.up);
    }

    public void Slide(){
        ChangeAnim("slide");
        transform.rotation=Quaternion.Euler(new Vector3(0,0,90));
        rb.velocity=new Vector2(horizontal*Time.deltaTime*speed,rb.velocity.y);
        Invoke(nameof(ResetSlide),1f);
    }

    private void ResetSlide(){
        ChangeAnim("idle");
        transform.rotation=Quaternion.Euler(Vector3.zero);
    }

    public void Attack(){
        ChangeAnim("attack");
        isAttack=true;        
        Invoke(nameof(ResetAttack),0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack),.5f);
    }

    private void ActiveAttack(){
        attackArea.SetActive(true);
    }

    private void DeActiveAttack(){
        attackArea.SetActive(false);
    }

    private void ResetAttack(){
        isAttack=false;
        ChangeAnim("idle");
    }

    public void Throw(){
        ChangeAnim("throw");
        isAttack=true;
        Invoke(nameof(ResetAttack),0.5f);
        Instantiate(kunaiPrefab,throwPoint.position,throwPoint.rotation);
    }

    public void SetMove(float horizontal){
        this.horizontal=horizontal;
        isRunning=true;
    }


    internal void SavePoint()
    {
        savePoint=transform.position;
    }
}
