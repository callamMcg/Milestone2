using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //an array to store the targets
    private GameObject[] targets;

    //the target currently being focused on
    private int targetIndex = 0;

    /*
     * 1 - set targets to the targets game objects
     * 2 - if the index is out of range set it to 0
     * 3 - sort the game objects
     */
    void GetTarget()
    {
        //1
        targets = GameObject.FindGameObjectsWithTag("Target");

        //2
        if (targetIndex >= targets.Length)
        {
            targetIndex = 0;
        }

        //3
        Bubble();
    }

    /*
     * 1 - get the left or right keys if pressed
     * 2 - if left, go to the previous index
     * 3 - if right, go to the next index
     */
    void SwitchTarget()
    {
        //1
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        //2
        if (left)
        {
            Prev();
        }
        //3
        else if (right)
        {
            Next();
        }
    }

    /*
     * 1 - if the index is at its max, set to 0
     * 2 - otherwise, add 1
     */
    void Next()
    {
        //1
        if (targetIndex == targets.Length - 1)
        {
            targetIndex = 0;
        }
        //2
        else
        {
            targetIndex++;
        }
    }

    /*
     * 1 - if the index is at 0, set it to its max
     * 2 - otherwise, minus 1
     */
    void Prev()
    {
        //1
        if (targetIndex == 0)
        {
            targetIndex = targets.Length - 1;
        }
        //2
        else
        {
            targetIndex--;
        }
    }

    /*
     * 1 - get the length of the array
     * 2 - loop through the array 1 less than the length
     * 3 - itterate over the array
     * 4 - get the hp of the current object and the next
     * 5 - swap the current object and the next if the current hp is smaller
     */
    void Bubble()
    {
        //1
        int n = targets.Length;

        //2
        for (int i = 0; i < n - 1 ; i++)
        {
            //3
            for(int j = 0; j < n - i - 1 ; j++)
            {
                //4
                int currHP = targets[j].GetComponent<Target>().GetHP();
                int nextHP = targets[j].GetComponent<Target>().GetHP();

                //5
                if(nextHP > currHP)
                {
                    GameObject temp = targets[j + 1];
                    targets[j + 1] = targets[j];
                    targets[j] = temp;
                }
            }
        }
    }

    /*
     * 1 - when space is pressed, cast a ray from the main camera
     * 2 - if the ray hits a target, get the target
     * 3 - deal damage to the target
     * 4 - if the target has no hp, remove it from the array, move onto the next target and destory the object.
     * 5 - if the ray misses the target, look at the current target
     */
    void Shoot()
    {
        //1
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit))
            {
                if (hit.collider != null)
                {

                    Debug.Log("Bang");
                    //2
                    if (hit.collider.gameObject.CompareTag("Target"))
                    {
                        GameObject hitTarget = hit.collider.gameObject;

                        Debug.Log("Hit");
                        //3
                        hitTarget.GetComponent<Target>().Damage(1);

                        //4
                        if (hitTarget.GetComponent<Target>().GetHP() <= 1)
                        {
                            targets[targetIndex] = null;
                            Next();
                            Destroy(hitTarget);
                        }
                    }
                    //5
                    else
                    {
                        Look();
                    }
                }
            }
        }
    }

    /*
     * 1 - if the target is null, get the targets again
     * 2 - otherwise make the camera look at the target
     */
    void Look()
    {
        if (targets[targetIndex] == null)
        {
            GetTarget();
        }
        else
        {
            Camera.main.transform.LookAt(targets[targetIndex].transform);
        }
    }

    //at the start of the game get all the targets
    void Start()
    {
        GetTarget();
    }

    /*
     * 1 - if there are no targets load the win screen
     * 2 - otherwise, look at the target
     * 3 - Switch target and shoot on command
     */
    void Update()
    {
        //1
        if(targets.Length == 0)
        {
            SceneManager.LoadScene("Win");
        }
        //2
        else
        {
            GetTarget();
            if(targets.Length != 0)
            {
                Look();
            }
        }
        //3
        SwitchTarget();
        Shoot();
    }
}
