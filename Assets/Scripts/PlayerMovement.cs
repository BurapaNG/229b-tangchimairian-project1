
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    private void Awake()
    {
        //Grab references for rigibody and animetor from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2 (horizontalInput * speed, body.velocity.y); //เดินซ้ายขวา

        //flip player
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(7, 7, 7);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-7 , 7 , 7);

        if (Input.GetKey(KeyCode.Space) && grounded) //กระโดด เช็คพื้น
            jump();

        //set animator
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded",grounded);
            
    }
    private void jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) // เช็คการชน Enter
    {
        if(collision.gameObject.tag == "Ground")
            grounded = true;
    }

}
