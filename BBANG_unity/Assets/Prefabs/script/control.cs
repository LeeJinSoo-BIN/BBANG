using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    public float walk_speed;
    public float run_speed;
    public float turnSpeed = 6f;   
    Vector3 moveVec;
    private float currentCamerXRotation = 0f;
    public Camera m_Camera;
    public GameObject weapon;
    public float gunFireDelay;
    private float currentFireDelay;
    public ParticleSystem muzzleFlash;
    public int leftAmmo;
    public int currentAmmo;
    public TextMeshProUGUI leftAmmoText;
    public TextMeshProUGUI currentAmmoText;
    public GameObject hitEffect;
    public float weaponRange;
    private RaycastHit hitInfo;
    private void Awake()
    {
    	rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>(); 
    }
    
    void Update()
    {
        GetInput();
        Move();
        View();
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
        //
        transform.Translate(moveVec * speed * Time.deltaTime);
        
        anim.SetBool("walk", moveVec != Vector3.zero); // Walk 애니메이션 true
        
        if(run){
            anim.SetBool("run",true);
            speed = run_speed;
        }
        else{
            anim.SetBool("run",false);
            speed = walk_speed;
        }
    }

    void View()
    {
       
            float mouse_horizontal = Input.GetAxisRaw("Mouse Y");
            float mouse_vertical = Input.GetAxisRaw("Mouse X");

            transform.localRotation *= Quaternion.Euler(0, mouse_vertical * turnSpeed, 0);

            currentCamerXRotation += -mouse_horizontal * turnSpeed;
            currentCamerXRotation = Mathf.Clamp(currentCamerXRotation, -90, 90);

            //m_Camera.transform.localRotation *= Quaternion.Euler(-mouse_horizontal * turnSpeed * Time.deltaTime, 0, 0);
            m_Camera.transform.localEulerAngles = new Vector3(currentCamerXRotation, 0f, 0f);

        
    }


    void Pick()
    {
        
        if (rifle) {
            pick_rifle = !pick_rifle;
            anim.SetBool("rifle", pick_rifle);
            anim.SetBool("aiming", pick_rifle);
            weapon.SetActive(pick_rifle);
        }
        
    }
    void Fire(){
        if(pick_rifle && fire && currentFireDelay <= 0 && currentAmmo > 0){
            currentFireDelay = gunFireDelay;
            currentAmmo -= 1;
            anim.SetTrigger("fire");
            muzzleFlash.Play();
            currentAmmoText.text = currentAmmo.ToString();


            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hitInfo, weaponRange)){
                
                var clone = Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(clone, 2f);
                
            }
        }
        if (currentFireDelay > 0){
            currentFireDelay -= Time.deltaTime;
        }
    }

}