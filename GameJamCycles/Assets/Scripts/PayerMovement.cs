using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    [SerializeField] private LayerMask jumpableGround;
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private Transform rock;
    [SerializeField] private Transform pedestalTransform;
    [SerializeField] private SpriteRenderer pedestal;
    [SerializeField] private TilemapRenderer ground;
    [SerializeField] private TilemapRenderer groundPast;
    [SerializeField] private TilemapRenderer groundFuture;
    [SerializeField] private Sprite pedestalNew;
    private int currentTime = 0;    //-1 = past, 0 = present, 1 = future
    private bool canChangeTime = false;
    private bool pickUpAllowed = false;
    private enum MovementState { idle, running, jumping, falling }; // idle = 0, running = 1, jumping = 2, falling = 3
    [SerializeField] private float TimeLapsTimeDelay = 1.5f;
    [SerializeField] private AudioSource jumpingSoundEffect;
    [SerializeField] private AudioSource runningSoundEffect;
    private bool isMoving = false;
    private bool timelaps = false;
    private Coroutine timelapsCoroutine = null;
    [SerializeField] private Transform playerLight;
    [SerializeField] private Transform finish;
    private bool isPushing = false;

    


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

        // Check if the player is moving
        isMoving = dirX != 0;

        // Play/Stop running sound based on the player's movement
        if (isMoving && IsGrounded())
        {
            if (!runningSoundEffect.isPlaying)
            {
                runningSoundEffect.Play();
            }
        }
        else
        {
            if (runningSoundEffect.isPlaying)
            {
                runningSoundEffect.Stop();
            }
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if(Input.GetKeyDown(KeyCode.R) && pickUpAllowed) 
        {
            canChangeTime = true;
            pedestal.sprite = pedestalNew;
        }

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

         if (Input.GetKeyDown(KeyCode.E) && IsGrounded() && canChangeTime == true)
        {
            StartCoroutine(DelayedMoveFuture());
            timelaps = true;
            ResetTimelapsAfterDelay();
        }

        if (Input.GetKeyDown(KeyCode.Q) && IsGrounded() && canChangeTime == true)
        {
            StartCoroutine(DelayedMovePast());
            timelaps = true;
            ResetTimelapsAfterDelay();
        }

        anim.SetInteger("state", (int)state);
        anim.SetBool("timelaps", timelaps);
    }

    private IEnumerator DelayedMoveFuture()
    {
        yield return new WaitForSeconds(TimeLapsTimeDelay); // Wait for 2 seconds
        MoveFuture();
    }

    private IEnumerator DelayedMovePast()
    {
        yield return new WaitForSeconds(TimeLapsTimeDelay); // Wait for 2 seconds
        MovePast();
    }

    private void MoveFuture()
    {
        if (currentTime == 0)
        {
            groundPast.sortingOrder = -1;
            ground.sortingOrder = -1;
            groundFuture.sortingOrder = 0;

            currentTime = 1;
            Vector3 lightPosition = playerLight.position;
            lightPosition.z = 0;
            playerLight.position = lightPosition;

            Vector3 rockPosition = rock.position;
            rockPosition.z = 1;
            rock.position = rockPosition;

            Vector3 finishPosition = finish.position;
            finishPosition.z = 1;
            finish.position = finishPosition;

            Vector3 pedestalPosition = pedestalTransform.position;
            pedestalPosition.z = 1;
            pedestalTransform.position = pedestalPosition;

        }
        if (currentTime == -1)
        {
            groundPast.sortingOrder = -1;
            ground.sortingOrder = 0;
            groundFuture.sortingOrder = -1;

            currentTime = 0;
            Vector3 lightPosition = playerLight.position;
            lightPosition.z = -1;
            playerLight.position = lightPosition;

            Vector3 rockPosition = rock.position;
            rockPosition.z = 0;
            rock.position = rockPosition;

            Vector3 finishPosition = finish.position;
            finishPosition.z = 0;
            finish.position = finishPosition;

            Vector3 pedestalPosition = pedestalTransform.position;
            pedestalPosition.z = 0;
            pedestalTransform.position = pedestalPosition;
        }
    }

    private void MovePast()
    {
        if (currentTime == 0)
        {
            groundPast.sortingOrder = 0;
            ground.sortingOrder = -1;
            groundFuture.sortingOrder = -1;
            currentTime = -1;

            Vector3 lightPosition = playerLight.position;
            lightPosition.z = 0;
            playerLight.position = lightPosition;

            Vector3 rockPosition = rock.position;
            rockPosition.z = 1;
            rock.position = rockPosition;

            Vector3 finishPosition = finish.position;
            finishPosition.z = 1;
            finish.position = finishPosition;

            Vector3 pedestalPosition = pedestalTransform.position;
            pedestalPosition.z = 1;
            pedestalTransform.position = pedestalPosition;

        }
        if (currentTime == 1)
        {
            groundPast.sortingOrder = -1;
            ground.sortingOrder = 0;
            groundFuture.sortingOrder = -1;
            currentTime = 0;

            Vector3 lightPosition = playerLight.position;
            lightPosition.z = -1;
            playerLight.position = lightPosition;

            Vector3 rockPosition = rock.position;
            rockPosition.z = 0;
            rock.position = rockPosition;

            Vector3 finishPosition = finish.position;
            finishPosition.z = 0;
            finish.position = finishPosition;

            Vector3 pedestalPosition = pedestalTransform.position;
            pedestalPosition.z = 0;
            pedestalTransform.position = pedestalPosition;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    //rock.bodyType = rock.bodyType == RigidbodyType2D.Dynamic ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;

    private void ResetTimelapsAfterDelay()
    {
        if (timelapsCoroutine != null)
        {
            StopCoroutine(timelapsCoroutine);
        }

        timelapsCoroutine = StartCoroutine(ResetTimelaps());
    }

    private IEnumerator ResetTimelaps()
    {
        yield return new WaitForSeconds(TimeLapsTimeDelay); // Wait for 2 seconds
        timelaps = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            if (transform.position.y > collision.transform.position.y + 0.9f)
            {
            jumpingSoundEffect.Play();
            }
            else
            {
                if(IsGrounded())
                { 
                    isPushing = true;
                    Debug.Log("Pushing");

                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pedestal"))
        {
            pickUpAllowed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pedestal"))
        {
            pickUpAllowed = false;
        }
    }

    

}
