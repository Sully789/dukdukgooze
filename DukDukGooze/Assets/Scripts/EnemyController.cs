/*
 * Sean O'Sullivan, K00180620, Programming Digital Games Engines, CA1
 * EnemyController.cs handles the AI and collison detection for the Enemy
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints; //Public Transform array to hold the waypoints enemy travels to
    private int currentWaypoint = 0; //Int to hold the current waypoint enemy is located

    private float speed = 6.0f; //Speed enemy moves
    private float zRange = 9;//Range on z axis that detects if enemy has left game screen
    private float xRange = 9; //Range on x axis that detects if enemy has left game screen

    private bool reachedPlayer = false;//Boolean to detect if enemy has collided with player

    public Animator anim; //Animator object used for animation of enemies

    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Pathfinding();
        CheckBounds();
    }

    private void Pathfinding() //Method handles how Enemy naivagtes maze, Source: https://noobtuts.com/unity/2d-pacman-game
    {
        // If statement that checks if enemy has reached waypoint and if they have not collided with player they will move towards it
        if (transform.position != waypoints[currentWaypoint].position && !reachedPlayer)
        {
            Vector3 path = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);
           // transform.rotation = Quaternion.LookRotation(path);
           // transform.Translate(path * speed * Time.deltaTime, Space.World);
            GetComponent<Rigidbody>().MovePosition(path);
        }
        //Else if waypoint is reached move to the next one
        else currentWaypoint = (currentWaypoint + 1) % waypoints.Length;

        // Animation, Unused implmentation to try to get direction enemy is moving, unused beacuse Enemy would get stuck on walls
       // Vector3 dir = waypoints[currentWaypoint].position - transform.position;
       // transform.rotation = Quaternion.LookRotation(dir);
        //transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    private void CheckBounds() //Method that destroys enemy once they leave scene
    {
        if (transform.position.z < -zRange)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z > zRange)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x < -xRange)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x > xRange)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision) //Detects if enemy collides with player
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            reachedPlayer = true;
            anim.SetBool("Eat_b", true); //Plays animation
            anim.SetFloat("Speed_f", 0.09f);
        }
    }
}
