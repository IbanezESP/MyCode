using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemiesEagle : enemies
{
    private Rigidbody2D rb;
    //private Animator anim;
    public Transform topPoint, endPoint;

    private float topY, endY;
    private bool touch = true;

    public float fly;


    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        transform.DetachChildren();
        topY = topPoint.position.y;
        endY = endPoint.position.y;
        Destroy(topPoint.gameObject);
        Destroy(endPoint.gameObject);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (touch)
        {
            rb.velocity = new Vector2(rb.velocity.x, fly);
            if (transform.position.y > topY)
            {
                touch = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -fly);
            if (transform.position.y < endY)
            {
                touch = true;
            }
        }
    }


}
