using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerManager
{
    public float speed;
    public float rotationSpeed;

    public Animator animator;

    [SerializeField] private LayerMask layerMask;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float vertical = 0;
        float horizontal = 0;

        if (Input.GetKey(KeyCode.W))
            vertical = 1;

        if (Input.GetKey(KeyCode.S))
            vertical = -1;

        if (Input.GetKey(KeyCode.D))
            horizontal = 1;

        if (Input.GetKey(KeyCode.A))
            horizontal = -1;

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction.Normalize();

        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Rotation
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            animator.SetBool("Run", true);
        }
        else
            animator.SetBool("Run", false);
    }
}
