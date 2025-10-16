using UnityEngine;

public class BIallofdeath : MonoBehaviour
{
    public Rigidbody2D rg;
    [SerializeField] int direction = 1;
    [SerializeField] float ballSpeed = 1f;
    [SerializeField] float ballH = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        rg.linearVelocity = new Vector2(direction * ballSpeed, rg.linearVelocityY += ballH);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WallB"))
        {
            direction = -1;
        }
        if (other.gameObject.CompareTag("WallA"))
        {
            direction = 1;
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            ballH *= -1;
        }
    }
}


