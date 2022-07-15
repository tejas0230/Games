using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovementRB : MonoBehaviour
{
    public Rigidbody playerRB;
    public Animator mario;
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    public float moveSpeed = 5f;
    public float groundDistance;
    public bool isGrounded;
    public float acceleration = 3f;
    float x;
    public float jumpForce =5f;
    float animSpeed;

    float OriginalMoveSpeed;
    
    private void Start()
    {
        OriginalMoveSpeed = moveSpeed;
    }
    private void Update()
    {
        handleJumpAnim();
        handleRunAnim();
        adjustDirection();
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        isGrounded = Physics.CheckBox(groundCheck.position,new Vector3(0.5f, 0.05f, 0.5f),Quaternion.identity,groundLayer);
        x = Input.GetAxis("Horizontal");
        /*if(!isGrounded)
        {
            moveSpeed = OriginalMoveSpeed / 2;
        }
        else
        {
            moveSpeed = OriginalMoveSpeed;
        }*/
    }

    private void FixedUpdate()
    {

            playerRB.velocity = new Vector3(playerRB.velocity.x, playerRB.velocity.y, x * moveSpeed);
   
        if(isGrounded && Input.GetKey(KeyCode.Space))
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            audioManager.Instance.PlaySound("jump");
        }
    }

    void adjustDirection()
    {
        if (x < 0)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, -1);
        }
        else if (x > 0)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 1);
        }
    }
    void handleRunAnim()
    {
        if (x != 0 && animSpeed < 1)
        {
            animSpeed += Time.deltaTime * acceleration;
        }
        else if (x == 0 && animSpeed > 0)
        {
            animSpeed -= Time.deltaTime * acceleration;
        }

        mario.SetFloat("velocity", animSpeed);
    }
    void handleJumpAnim()
    {
        if (isGrounded)
        {
            mario.SetBool("jump", false);
        }
        else
        {
            mario.SetBool("jump", true);
        }

    }
}
