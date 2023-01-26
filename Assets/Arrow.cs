using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [HideInInspector]
    public float initialRotation;
    float angle;
    public Rigidbody2D rb;
    public float initialSpeed;
    Vector3 Velocity;
    Vector3 oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, initialRotation);
        rb.velocity = transform.up * Time.fixedDeltaTime * initialSpeed;
        oldPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Velocity = oldPosition - transform.position;
        oldPosition = transform.position;

        angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

    }
}
