using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private float dirX = 0f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jimpForce = 14f;
    [SerializeField] private AudioSource jumpSoundEffect; 
    
    private enum MovementState
    {
        Idle,
        Running,
        Jumping,
        Falling
    }

    
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
   
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jimpForce);
        }

        UpdateAnimationState();
        
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        switch (dirX)
        {
            case > 0f:
                state = MovementState.Running;
                sprite.flipX = false;
                break;
            case < 0f:
                state = MovementState.Running;
                sprite.flipX = true;
                break;
            default:
                state = MovementState.Idle;
                break;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.Jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.Falling;
        }
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .3f, jumpableGround);
    }
}
