using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    //====================//
    //Movement of the boss//
    //====================//
    public Rigidbody2D rg;
    [SerializeField] float speed = 5f;
    [SerializeField] float floatSpeed = 5f;
    private int direction = 1;

    //==============//
    //Float movement//
    //==============//
    private bool canFloat = false;
    private bool canFall = false;
    private bool floatStart = false;
    private float floatTime = 1f;
    private float fallTime = 1f;

    //==========================//
    //Spike creation of the boss//
    //==========================//
    private float bulletCreation = 0.5f;
    private float bulletCooldown = 0.8f;
    private bool canCreate = false;
    private bool canStart = true;

    //============//
    //Game objects//
    //============//
    GameObject arm;
    GameObject bulletSpike;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletSpike = GameObject.FindWithTag("Bullet");
        arm = GameObject.FindWithTag("Arm");
    }

    // Update is called once per frame
    void Update()
    {
        rg.linearVelocity = new Vector2(direction * speed, rg.linearVelocityY);

    }

    void FixedUpdate()
    {
        //Call the routine that lets you create spikes
        if (canStart)
        {
            canCreate = true;
            StartCoroutine(spikeCreation());
        }

        //Check if the boss can float
        if (rg.transform.position.x >= 10f && !floatStart)
        {
            floatStart = true;
            StartCoroutine(floatCycle());
        }

    }

    //Float cycle
    private IEnumerator floatCycle()
    {
        while(true)
        {
            //Float phase
            canFloat = true;
            rg.linearVelocity = new Vector2(rg.linearVelocityX, floatSpeed);
            yield return new WaitForSeconds(floatTime);
            canFloat = false;

            //Fall phase
            canFall = true;
            rg.linearVelocity = new Vector2(rg.linearVelocityX, -floatSpeed);
            yield return new WaitForSeconds(fallTime);
            canFall = false;
        }
    }

    //Spike creation
    private IEnumerator spikeCreation()
    {
        //Check if the boss can create
        if (canCreate)
        {
            Vector2 startPos = arm.transform.position;

            if (arm != null)
            {
                Vector2 newPos = new Vector2(startPos.x - 2, 0.5f);
                Instantiate(bulletSpike, newPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(bulletCreation);
            canCreate = false;
            canStart = false;
            yield return new WaitForSeconds(bulletCooldown);
            canStart = true;
        }
        //Disable the start and create components
        if (rg.transform.position.x >= 10f)
        {
            canStart = false;
            canCreate = false;
        }
    }
}