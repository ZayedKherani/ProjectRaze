using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float movementSpeed = 2f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    private Vector2 movementDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            Camera.main.orthographicSize--;
        };

        if (Input.GetKeyDown(KeyCode.Period))
        {
            Camera.main.orthographicSize++;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movementDirection * movementSpeed;    
    }
}
