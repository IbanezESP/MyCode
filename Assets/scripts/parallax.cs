using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public Transform cam;
    public float moveRate;
    private float starPoingX, starPoingY;
    public bool lockY; // false

    void Start()
    {
        starPoingX = transform.position.x;
        starPoingY = transform.position.y;
    }

    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(starPoingX + cam.position.x * moveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(starPoingX + cam.position.x * moveRate, starPoingY + cam.position.y * moveRate);
        }
    }
}
