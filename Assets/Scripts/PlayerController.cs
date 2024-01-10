using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float longIdleTime; //= 5f; // revisar, está colocado en el motor unity el valor tmb
    public float speed = 2.5f;
    public float jumpForce = 2.5f;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    // References
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    // Long Idle
    private float _longIdleTimer;

    // Movement
    private Vector2 _movement;
    private bool _facingRight = true;
    private bool _isGrounded;

    // Attack
    private bool _isAttacking;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAttacking == false)
        {
            //Movement
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            _movement = new Vector2(horizontalInput, 0f);

            //Flip character
            if (horizontalInput < 0f && _facingRight == true)
            {
                Flip();
            }
            else if (horizontalInput > 0f && _facingRight == false)
            {
                Flip();
            }
        }

        // is Grounded?
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // is Jumping?
        if (Input.GetButtonDown("Jump") && _isGrounded == true && _isAttacking == false)
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //Wanna Attack?
        if (Input.GetButtonDown("Fire1") && _isGrounded == true && _isAttacking == false) 
        {
            _movement = Vector2.zero;
            _rigidbody.velocity = Vector2.zero;
            _animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        if (_isAttacking == false)
        {
            float horizontalVelocity = _movement.normalized.x * speed;
            _rigidbody.velocity = new Vector2(horizontalVelocity, _rigidbody.velocity.y);
        }
    }

    private void LateUpdate()
    {
        _animator.SetBool("Idle", _movement == Vector2.zero);
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetFloat("VerticalVelocity", _rigidbody.velocity.y);

        // Animator
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _isAttacking = true;
        }
        else
        {
            _isAttacking = false;
        }

        Debug.Log("antes de entrar al long idle");
        // Long Idle - REVISAR,NO ANDA
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            Debug.Log("aantes de entrar el long idle con tiempo");
            _longIdleTimer += Time.deltaTime;
            Debug.Log(_longIdleTimer + "antes de ENTRAR al IF");

            if (_longIdleTimer >= longIdleTime)
            {
                Debug.Log("ENTRÓ!!!???");
                _animator.SetTrigger("LongIdle");
            }
            else // entra siempre al ELSE y reinicia el contador de tiempo, jamás llega a 5seg
            {
                Debug.Log("Si No - contador timer a CERO");
                _longIdleTimer = 0f;
            }
            Debug.Log(_longIdleTimer + "fuera del IF");
        }
    }

    private void Flip() // solucionado
    {
        _facingRight = !_facingRight;
        float localScaleX = transform.localScale.x; // error estaba transform.position.x fue cambiado al actual 
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
}
