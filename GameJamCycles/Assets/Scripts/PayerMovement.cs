using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; 
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    [SerializeField] private LayerMask jumpbleGround;
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private Rigidbody2D box;
    [SerializeField] private TilemapRenderer ground;
    private enum MovementState { idle, running, jumping, falling };

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if(Input.GetKeyDown(KeyCode.R)) { ChangeBoxBodyType(); }
        
        if(Input.GetKeyDown(KeyCode.Q)) { ChangeTilemapLayer(); }
        UpdateAnimationState();

    }

    private void UpdateAnimationState() 
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpbleGround);
    }
    private void ChangeBoxBodyType()
    {
        box.bodyType = box.bodyType == RigidbodyType2D.Dynamic ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        Debug.Log("button R pressed");
    }

    private void ChangeTilemapLayer()
    {
        ground.sortingOrder = ground.sortingOrder == -1 ? 0 : -1;
    }
}

