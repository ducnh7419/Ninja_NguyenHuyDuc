using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image imageFill;
    [SerializeField] Vector3 offset;
    private Transform target;
    float maxHp;
    float hp;

    void Update()
    {
        imageFill.fillAmount=Mathf.Lerp(imageFill.fillAmount,hp/maxHp,Time.deltaTime*5f);
        transform.position=target.position + offset;
    }

    public void OnInit(float maxHp,Transform target){
        this.target=target;
        this.maxHp = maxHp;
        hp=maxHp;
        imageFill.fillAmount = 1;
    }

    public void setNewHp(float newHp){
        this.hp=newHp;

    }
   
}
