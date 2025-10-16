using System;
using System.Numerics;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemyRg;
    public float enemySpeed = 1.0f;
    private int startDirection = -1;
    private int currDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currDirection = startDirection;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PointA") || collision.gameObject.CompareTag("PointB"))
        {
            currDirection *= -1;
        }
    }

    void FixedUpdate()
    {
        enemyRg.linearVelocity = new Vector2(enemySpeed * currDirection, enemyRg.linearVelocityY);
    }
}
