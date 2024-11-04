using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //track the current hp
    public int HP = 1;

    //share the current hp
    public int GetHP() { return HP; }

    //lower the current hp
    public void Damage(int damage = 1) 
    { 
        HP -= damage; 
    }

    //change size depending on hp
    void Scale()
    {
        transform.localScale = Vector3.one * HP;
        float y = 0.5f;
        if (HP != 1)
        {
            y = HP/1.5f;
        }
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    void Start() 
    {
        GetComponent<RandomMovement>().SetSpeed(HP);
    }

    //keep the size up to date
    void Update()
    {
        Scale();

        GetComponent<RandomMovement>().SetSpeed(HP);
    }
}
