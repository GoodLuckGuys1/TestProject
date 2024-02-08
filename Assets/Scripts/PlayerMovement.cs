using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool isJump;
    private bool _isDoubleJump;
    private float _dirX;
    private float _currentSpeed;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [FormerlySerializedAs("jimpForce")] [SerializeField] private float jumpForce = 14f;
    [SerializeField] private AudioSource jumpSoundEffect; 
    
    private enum MovementState
    {
        Idle,
        Running,
        Jumping,
        Falling,
        DoubleJump
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
        _dirX = Input.GetAxisRaw("Horizontal");
        Debug.Log(_dirX);
        _currentSpeed = _dirX == 0 ? _currentSpeed : _dirX * moveSpeed;
        rb.velocity = new Vector2(_currentSpeed, rb.velocity.y);
        if (Input.GetButtonDown("Jump") )
        {
            _isDoubleJump = false;
            
            if (IsGrounded())
            {
                isJump = true;
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else
            {
                if (isJump && rb.velocity.y > .1f)
                {
                    jumpSoundEffect.Play();
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    isJump = false;
                    _isDoubleJump = true;
                }
            }
        }

        UpdateAnimationState();
        
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        switch (_currentSpeed)
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
            state = _isDoubleJump? MovementState.DoubleJump: MovementState.Jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.Falling;
        }
        
        anim.SetInteger("state", (int)state);
    }

    public void OnLeft()
    {
        Debug.Log("Left");
        _currentSpeed = -moveSpeed;
    }

    public void OnRight()
    {
        Debug.Log("Right");
        _currentSpeed = moveSpeed;
    }

    public void Jump()
    {
        Debug.Log("Jump click");
        _isDoubleJump = false;
            
        if (IsGrounded())
        {
            Debug.Log("Jump");
            isJump = true;
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else
        {
            if (isJump && rb.velocity.y > .1f)
            {
                Debug.Log("2Jump");
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJump = false;
                _isDoubleJump = true;
            }
        }
    }

    public void Stop()
    {
        _currentSpeed = 0;
    }
    
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .3f, jumpableGround);
    }
}
