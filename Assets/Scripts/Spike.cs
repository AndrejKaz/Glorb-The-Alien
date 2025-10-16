using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public Rigidbody2D rg;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float airTime = 1.2f;
    [SerializeField] bool isGrounded = true;

    void FixedUpdate()
    {
        if (isGrounded)
        {
            StartCoroutine(SpikeSeq());
        }
    }

    private IEnumerator SpikeSeq()
    {
        rg.linearVelocity = new Vector2(rg.linearVelocityX, jumpForce);
        isGrounded = false;
        yield return new WaitForSeconds(airTime);
        isGrounded = true;
    }
}
