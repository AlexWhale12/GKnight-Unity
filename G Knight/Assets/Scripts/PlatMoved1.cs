using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatMoved1 : Entity
{
    private float speed = 3.5f;
    private Vector3 dir;
    private SpriteRenderer sprite;

    [SerializeField] private int lives;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        dir = transform.up;
    }
    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.up * dir.x * 0.7f, 0.1f);

        if (colliders.Length > 0) dir *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
