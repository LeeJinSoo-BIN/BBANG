using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;
    
    float hAxis;
    float vAxis;
    bool rifle;
    bool pick_rifle = false;
    bool run;
    bool fire;
    public float speed;
    Vector3 moveVec;
        
    private void Awake()
    {
    	rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>(); 
    }
    
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Pick();
        Fire();
    }

    void GetInput() // 키보드 값 받기
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        fire = Input.GetMouseButton(0);
        
        run = Input.GetKey(KeyCode.LeftShift);
        rifle = Input.GetKeyDown("1");        
        
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * speed * Time.deltaTime;
        
        anim.SetBool("walk", moveVec != Vector3.zero); // Walk 애니메이션 true
        
        if(run){
            anim.SetBool("run",true);
            speed = 2.5f;
        }
        else{
            anim.SetBool("run",false);
            speed = 1f;
        }
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec); // 자연스럽게 회전
    }

    void Pick()
    {
        
        if (rifle) {
            pick_rifle = !pick_rifle;
            anim.SetBool("rifle", pick_rifle);
            anim.SetBool("shoulder_rifle", pick_rifle);
        }
        
    }
    void Fire(){
        if(fire){
            anim.SetTrigger("fire_rifle");
        }
    }

}
