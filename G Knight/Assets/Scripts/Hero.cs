using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
    private PlayerController control;
    [SerializeField] private float speed;
    [SerializeField] private int lives;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveInput;
    private bool isGrounded = false;

    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public static Hero Instance { get; set; }

    private States State

    {
      get { return (States)anim.GetInteger("State"); }
      set { anim.SetInteger("State", (int)value); }
    }

private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged = true;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    void Update()
    {
        if (isGrounded)State = States.Idle;

        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButtonDown("Fire1"))
            Attack();
    }

    private void Run()
    {
        if (isGrounded)State = States.Run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;

        if (!isGrounded) State = States.Jump;
    }

    public override void GetDamage()
    {
        lives -= 1;
        Debug.Log(lives);

        if (lives < 1)
            Die();
    }

    private void Attack()
    {
        if (isGrounded && isRecharged)
        {
            State = States.Idle;
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }

    }
    
    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }    
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    public enum States
    {
      Idle,
      Run,
      Jump,
      Attack
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            this.transform.parent = collision.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            this.transform.parent = null;
        }
    }
}
