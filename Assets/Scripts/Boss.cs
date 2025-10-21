using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    //Movement of the boss
    public Rigidbody2D rg;
    [SerializeField] float speed = 5f;
    private int direction = 1;

    //Spike creation of the boss
    private float bulletCreation = 0.5f;
    private float bulletCooldown = 0.8f;
    private bool canCreate = false;
    private bool canStart = true;

    //Game objects
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
        moveUp();
    }

    //Spike creation
    private IEnumerator spikeCreation()
    {
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

        if(rg.transform.position.x >= 10f)
        {
            canStart = false;
            canCreate = false;
        }
    }



    private void moveUp()
    {
        if (rg.transform.position.x >= 10f)
        {
            rg.linearVelocity = new Vector2(direction * speed, 5f);
            print("GO UP");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            direction *= -1;

        }

        if (collision.gameObject.CompareTag("PointA"))
        {
            direction *= -1;
        }

        if (collision.gameObject.CompareTag("PointB"))
        {
            direction *= -1;
            print("BOOM");
        }

    
    }

}
