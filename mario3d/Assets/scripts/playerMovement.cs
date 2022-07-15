using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    
    public CharacterController controller;
    public Animator marioAnimator;
    public Transform groundCheck;
    public Transform headBump;
    public LayerMask groundLayer;
    public LayerMask headBumpLayer;
    Vector3 velocity;

    public float moveSpeed = 5f;
    public float gravity= -9.18f;
    public float groundDistance;
    public float headHitDistance;
    public float jumpHeight = 3f;
    float x;
    public float acceleration = 3f;
    public bool isGrounded;
    public bool headJumpObstructed;
    float animSpeed;
    void Start()
    {
        
    }

    
    void Update()
    {
        handleJumpAnim();
        handleHeadBump();
        headJumpObstructed = Physics.CheckSphere(headBump.position, headHitDistance, headBumpLayer);
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundLayer);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        x = Input.GetAxis("Horizontal");
        adjustDirection();

        handleRunAnim();
        Vector3 move = Vector3.forward * x;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump")&&isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight* -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity*Time.deltaTime);
    }

    void adjustDirection()
    {
        if(x<0)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, - 1);
        }
        else if(x>0)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 1);
        }
    }
    void handleRunAnim()
    {
        if(x!=0 && animSpeed<1)
        {
            animSpeed += Time.deltaTime * acceleration;
        }
        else if(x==0&&animSpeed>0)
        {
            animSpeed -= Time.deltaTime * acceleration;
        }
        
        marioAnimator.SetFloat("velocity",animSpeed);
    }
    void handleJumpAnim()
    {
        if(isGrounded)
        {
            marioAnimator.SetBool("jump",false);
        }
        else
        {
            marioAnimator.SetBool("jump", true);
        }

    }
    void handleHeadBump()
    {
        if(headJumpObstructed)
        {
            velocity.y = -1f;
        }
            
    }
    
}
