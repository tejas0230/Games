using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    //Config
    public float PlayerSpeed;
    public float JumpSpeed;
    public float climbSpeed;
    public float LevelLoadDelay = 2f;


    //State
    bool isAlive = true;

    //Cached component reference
    Rigidbody2D rb;
    Animator myAnimation;
    CapsuleCollider2D myBodyCollider;
    CircleCollider2D myFeetCollider;

    //Message then methods
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        myAnimation = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<CircleCollider2D>();
    }


    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Jump();
        }
        climbLadder();
        Death();

    }
    void Run()
    {
        float InputX = Input.GetAxis("Horizontal");
        Vector2 PlayerVelocity = new Vector2(InputX * PlayerSpeed, rb.velocity.y);
        rb.velocity = PlayerVelocity;

        bool playerIsRunning = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        myAnimation.SetBool("Running", playerIsRunning);
    }
    void climbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimation.SetBool("Climbing", false);
            rb.gravityScale = 1;
            return;
        }
        float Climbing = Input.GetAxisRaw("Vertical");
        Vector2 ClimbVelocity = new Vector2(rb.velocity.x, Climbing * climbSpeed);
        rb.gravityScale = 0;
        rb.velocity = ClimbVelocity;
        bool playerIsClimbing = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        myAnimation.SetBool("Climbing", playerIsClimbing);
    }
    private void FlipSprite()
    {
        bool playerIsLookingRight = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (playerIsLookingRight)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1);
        }
    }

    void Jump()
    {

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 JumpVelocity = new Vector2(0f, JumpSpeed);
            rb.velocity += JumpVelocity;
        }

    }

    private void Death()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Spikes")))
        {
            isAlive = false;
            rb.velocity = new Vector2(80f, 80f);
            myAnimation.SetTrigger("Death");
            StartCoroutine(LoadNextLevel());
           
            
        }

        

    }
    IEnumerator LoadNextLevel()
    {

        yield return new WaitForSecondsRealtime(LevelLoadDelay);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }


}
