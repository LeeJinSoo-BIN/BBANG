using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;
    
    float hAxis;
    float vAxis;
    bool ak47;
    bool handfull = false;
    bool hand;
    bool run;
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

    }

    void GetInput() // 키보드 값 받기
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        ak47 = Input.GetKey("1");
        hand = Input.GetKey("3");
        run = Input.GetKey(KeyCode.LeftShift);
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * speed * Time.deltaTime;
        if(handfull){
            anim.SetBool("walkHandFull", moveVec != Vector3.zero); // Walk 애니메이션 true
        }
        else{
            anim.SetBool("walkNoHand", moveVec != Vector3.zero); // Walk 애니메이션 true
        }
        if(run){
            anim.SetBool("run",true);
            speed = 1.2f;
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
        // Jump하고 있는 상황에서 Jump 하지 않도록 방지
        if (ak47) {
            //rigid.AddForce(Vector3.up * 5, ForceMode.Impulse);
            anim.SetTrigger("ak47"); // Jump Trigger true 설정
            handfull = true;
        }
        else if (hand){
            handfull = false;
        }
    }
    
}
