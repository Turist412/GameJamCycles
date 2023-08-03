using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPushing : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] private float pushSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the direction of collision and apply force to the box in that direction.
            Vector2 pushDirection = collision.contacts[0].point - (Vector2)transform.position;
            rb.AddForce(pushDirection.normalized * pushSpeed, ForceMode2D.Impulse);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ChangeBoxBodyType();
        }
    }
    private void ChangeBoxBodyType()
    {
        rb.bodyType = rb.bodyType == RigidbodyType2D.Dynamic ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        Debug.Log("button R pressed");
    }

}
