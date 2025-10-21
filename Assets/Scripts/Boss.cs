using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody2D rg;

    [SerializeField] float speed = 5f;

    private int direction = 1;



    //Arm
    GameObject arm;
    GameObject bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bullet = GameObject.FindWithTag("Bullet");

        arm = GameObject.FindWithTag("Arm");

    }

    // Update is called once per frame
    void Update()
    {
        rg.linearVelocity = new Vector2(direction * speed, rg.linearVelocityY);
        shoot();
    }

    void shoot()
    {
        if(arm != null)
        {
            Instantiate(bullet, arm.transform.position, Quaternion.identity);
        }
    }


}
