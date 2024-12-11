using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private Transform rayCastPosition;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private bool onFloor = false;
    private bool onAir = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        float inputAxis = Input.GetAxis("Horizontal");
        transform.position += new Vector3(inputAxis * Time.deltaTime * speed,
                                          0, 0);

        spriteRenderer.flipX = inputAxis == .0f? Standing() : Walking(inputAxis);

        FloorDetection();

        if (onFloor && Input.GetButtonDown("Jump"))
        {
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            onFloor = false;
            animator.SetBool("isJumping", true);
            onAir = true;
            StartCoroutine(InitJump());
        }
    }

    private IEnumerator InitJump()
    {
        yield return new WaitForSeconds(0.1f);
        onAir = false;
    }

    private bool Walking(float inputAxis)
    {
        animator.SetBool("isWalking", true);
        return inputAxis > 0;
    }

    private bool Standing()
    {
        animator.SetBool("isWalking", false);
        return spriteRenderer.flipX;
    }

    private void FloorDetection()
    {
        var rayCast = Physics2D.Raycast(rayCastPosition.position, -transform.up, .1f);

        if (rayCast.collider != null)
        {
            if (!onAir)
            {
                onFloor = true;
                animator.SetBool("isJumping", false);
            }
        }
        else onFloor = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Floor")
        //    onFloor = true;
    }
}
