﻿
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    
    

    private void Awake()
    {
        //Grab references for rigibody and animetor from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
         horizontalInput = Input.GetAxis("Horizontal");

        //flip player
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(7, 7, 7);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-7, 7, 7);

        //set animator
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //wall jump
        if (wallJumpCooldown < 0.2f)
        {
            

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y); //เดินซ้ายขวา

            if (onWall() && isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space)) //กระโดด เช็คพื้น
                jump();

        }
        else
            wallJumpCooldown += Time.deltaTime;
            
    }
    private void jump()
    {
        if (isGrounded()) 
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");

        }
        else if (onWall() && isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10,0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y , transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            
            wallJumpCooldown = 0;
        }
        
    }

    private bool isGrounded()
    {
        RaycastHit2D raycasthit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size, 0,Vector2.down,0.1f, groundLayer);
        return raycasthit.collider  != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycasthit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.1f, wallLayer);
        return raycasthit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Win") 
        {
            SceneManager.LoadScene(2);
        }
    }
}
