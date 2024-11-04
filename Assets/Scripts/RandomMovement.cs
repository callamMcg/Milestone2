using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    private float rotation = 0;
    private static float speed;
    private Rigidbody rb;
    private float ticker = 0;
    private float collisionTicker = 0;

    public void SetSpeed(int HP)
    {
        speed = 50/HP;
    }

    private void NewRotation()
    {
        rotation = Random.Range(-180, 180);
    }

    void Rotate() 
    {
        Debug.Log(rotation);
        Quaternion target = Quaternion.Euler(0, rotation, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);

    }

    private void Move()
    {
        rb.velocity = transform.TransformDirection(Vector3.forward * speed);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(ticker <= 0)
        {
            ticker = 1;
            NewRotation();
        }
        else
        {
            ticker -= Time.deltaTime;
        }
        Rotate();
        Move();
    }

    void OnCollisionStay()
    {
        if(collisionTicker <= 0)
        {
            ticker = 1;
            collisionTicker = 1;
            NewRotation();
        }
        else
        {
            collisionTicker -= Time.deltaTime;
        }
    }

    void OnCollisionLeave()
    {
        collisionTicker = 0;
    }
}
