using System.Collections;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class movement : MonoBehaviour
{
    Animator animator;
    public CharacterController controller;
    public float speed = 7f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;
    public float jumpSpeed = 2.0f;
    public float gravity = 10.0f;
    private Vector3 movingDirection = Vector3.zero;
    bool isGrounded;
    public LayerMask ground;
    public GameObject playergameobject;
    healthtest Healthtest;

    
    /// <attackthings>
    public Transform attackPosition;
    public float AttackRadius;
    public LayerMask enemylayer;


    
    bool isIdle;
    bool isAttacking;
    bool isJumping;
    bool inAttackRadius;
 
   

    public void Awake()
    {
       
        animator = playergameobject.GetComponent<Animator>();
        Healthtest = FindAnyObjectByType<healthtest>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        idle();
        
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 14f;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 10f;
        }

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 movedirection = Quaternion.Euler(0f, targetAngle,0f) * Vector3.forward;
            controller.Move(movedirection.normalized * speed * Time.deltaTime);       
        }
        
        
        isGrounded = Physics.Raycast(controller.transform.position - Vector3.up * controller.height * 0.5f, Vector3.down, controller.stepOffset, LayerMask.GetMask("Ground"));

        Debug.DrawRay(controller.transform.position - Vector3.up * controller.height * 0.5f, Vector3.down * controller.stepOffset, Color.red);

        if  (isGrounded && controller.velocity == Vector3.zero)
        {
            idle();
            isIdle = true;
        }
        else if(isGrounded)
        {
            walk();
            isIdle = false;
        }
       
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && isIdle)
        {
            movingDirection.y = jumpSpeed;
            idlejump();
        }
        else if(isGrounded && Input.GetKeyDown(KeyCode.Space) && !isIdle)
        {
            movingDirection.y = jumpSpeed;
            walkjump();
        }

        if(Input.GetMouseButtonDown(0) && !isAttacking && isGrounded)
        {
            StartCoroutine(Attack());
        }
        
     

        movingDirection.y -= gravity * Time.deltaTime;
        controller.Move(movingDirection * Time.deltaTime);

        
    }

        public void idlejump()
        {
            animator.SetBool("idlejump",true);
            isJumping = true;
        }

        public void idle()
        {
            
             animator.SetBool("idle",true);
             animator.SetBool("idlejump", false);
             animator.SetBool("walk", false);
            
        }
        
        public void walk()
        {   
            
            animator.SetBool("walk", true);
            animator.SetBool("idle", false);
            animator.SetBool("jump", false);
        }
       
        public void walkjump()
        {
            animator.SetBool("jump",true);
            isJumping = true;
        }

        public IEnumerator Attack()
        {
            animator.SetBool("Attack1", true);
            isAttacking = true;
            Attackreal();
            speed = 0;
            yield return new WaitForSeconds(1);
            animator.SetBool("Attack1", false);
            isAttacking = false;
            speed = 10;
        }

        public IEnumerator GetHit()
        {
            animator.SetTrigger("Gethit");
            yield return new WaitForSeconds(1);
            animator.ResetTrigger("Gethit");
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                GetHit();
            }
        }

        public void Attackreal()
        {
            Collider[] hitenemy = Physics.OverlapSphere(attackPosition.position ,AttackRadius ,enemylayer);

            foreach(Collider Enemy in hitenemy)
            {
                Debug.Log("Enemy Hit");
                Healthtest.takeDamage(20);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition.position ,AttackRadius);
        }

}

