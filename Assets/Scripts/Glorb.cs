using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

public class Glorb : MonoBehaviour
{
    //Variables
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpForce = 25f;
    [SerializeField] bool isGrounded = false;
    [SerializeField] float movementX;
    [SerializeField] int coinCount;

    //Player references
    public Rigidbody2D glorbBody;
    GameObject glorb;
    private bool isAlive = true;

    //Dash
    private bool isDashing = false;
    private bool canDash = true;
    [SerializeField] float dashPower = 50f;
    private float dashTime = 0.5f;
    private float dashCooldown = 1f;

    //Scene management
    private int currScene;
    private int nextScene;           

    void Start()
    {
        currScene = SceneManager.GetActiveScene().buildIndex;

        glorb = GameObject.FindWithTag("Glorb");

        //Each time you load a new level get the number of coins
        foreach(GameObject coinObj in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coinCount++;
        }
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (!isAlive)
        {
            glorb.SetActive(false);
            return;
        }

        //Game Loop
        Movement();
        Jump();
     }

    //Function for the movement
    private void Movement()
    {
        // Horizontal input
        movementX = Input.GetAxisRaw("Horizontal");

        // Apply horizontal movement
        glorbBody.linearVelocity = new Vector2(movementX * speed, glorbBody.linearVelocityY);

        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    //Using an IEnumerator function so it dosent stop the already running code
    private  IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        glorbBody.linearVelocityX = movementX * dashPower;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    //Jump function
    private void Jump()
    {
        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            glorbBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

    }

    //Wall jump
    private void WallJump()
    {
        glorbBody.linearVelocityY += 6;
        glorbBody.linearVelocityX += 3;
    }

    //!-- Collision --!//
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Coin pick up
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coinCount--;
        }

        //Var so you can progress to the next level
        if (collision.CompareTag("Door") && coinCount == 0)
        {
            nextScene = currScene + 1;
            SceneManager.LoadScene(nextScene);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        //Is player grounded
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        //Wall jumping
        if (other.gameObject.CompareTag("Wall"))
        {
            glorbBody.linearVelocityY -= 2f;
            glorbBody.gravityScale = 2;
            isGrounded = true;
        
        }

        //Enemy collision
        if (other.gameObject.CompareTag("Enemy"))
        {
            isAlive = false;
        }

        //Jump platform
        if (other.gameObject.CompareTag("Jump platform"))
        {
            glorbBody.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
        }

        //Push platform
        if(other.gameObject.CompareTag("Push platform"))
        {
            glorbBody.AddForce(Vector2.right * 30, ForceMode2D.Impulse);
        }

        //Out of world
        if (other.gameObject.CompareTag("Pit platform"))
        {
            isAlive = false;
        }

        //Spike trap
        if (other.gameObject.CompareTag("Spike"))
        {
            isAlive = false;
        }

        //Ball trap 
        if (other.gameObject.CompareTag("Ball"))
        {
            isAlive = false;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        //Calling the wall jump function
        if (other.gameObject.CompareTag("Wall"))
        {
            WallJump();
        }
    }
    
}

