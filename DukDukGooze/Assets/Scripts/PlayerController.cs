/*
 * Sean O'Sullivan, K00180620, Programming Digital Games Engines, CA1
 * PlayerController.cs handles all the player input, collision detection, and FX for the Player
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput; //Float value of horizontal axis pressed
    private float verticalInput; //Float value of verticle axis pressed
    private float speed = 3.0f; // Speed at which player moves
    private float zRange = 7; //Range on z axis used to keep player in game area
    private float xRange = 7; //Range on x axis used to keep player in game area
    private float force = 70.0f; // Force at which player repels enemies when powered up

    public Animator anim; //Animator object used for animation of player character
    public Rigidbody rb; //Rigidbody used for collisons
   
    public GameObject powerUpIndicator; // Custom Game Object used to show when player has power up
    public GameManager gameManager; //Game Manager used for UI and progression

    public AudioClip pickUpSound; //Audio Object used for SFX for picking up power up
    public AudioClip battleSound; //Audio object used for SFX for colliding with enemy
    public AudioClip wallSound; ////Audio object used for SFX for destroying wall

    public AudioSource playerAudio; //Audio listener used for SFX

    private bool gameOver = false; //Booleans used for Game Logic
    private bool hasPowerUp = false;

    public ParticleSystem explosionParticle; // Particle Object used to play FX for colliding with enemy

    //Unused Objects intended for wall physics effect
    //public Rigidbody wallRb;
    //public GameObject walls;


    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();
        rb.GetComponent<Rigidbody>();
        powerUpIndicator.GetComponent<GameObject>();
        playerAudio = GetComponent<AudioSource>();
        // wallRb.GetComponent<Rigidbody>();  Unused code intended for wall physics effect
        // wallRb.useGravity = false;
        // wallRb.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        KeepInBounds();
        powerUpIndicator.transform.position = transform.position;// Ensures power up indicator follows player
    }

    private void PlayerInput() //Method to handle player input, player moves along axis postions, Source for code to handle direction player is facing: https://answers.unity.com/questions/803365/make-the-player-face-his-movement-direction.html
    {
        if(!gameOver) //Code does not run if player is defeated
        { 
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(-horizontalInput, 0.0f, -verticalInput); //Minus values used to handle movement along Isometric view
            transform.rotation = Quaternion.LookRotation(movement);
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
            anim.SetFloat("Speed_f", Mathf.Abs(horizontalInput + verticalInput));  //Mathf ensures value is always positive for animation
        }   
    }

    private void KeepInBounds() //Method ensures player does not move out of the game area
    {
        if (transform.position.z < -zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zRange);
        }
        else if (transform.position.z > zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }
        else if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerUp")) //OnTriggerEnter method to handle player picking up Power Up
        {
            hasPowerUp = true;
            powerUpIndicator.gameObject.SetActive(true);
            playerAudio.PlayOneShot(pickUpSound, 1.0f);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") && hasPowerUp) //OnCollison method to destroy wall when player collides and has Power Up
        {
            Destroy(collision.gameObject);
            playerAudio.PlayOneShot(wallSound, 1.0f);
            //wallRb.useGravity = true; //On Collision used here as it was intended to handle wall physics
            //wallRb.constraints = RigidbodyConstraints.None;
            //rb.AddForce(transform.forward * force);
        }
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp) //OnCollison method to send enemy flying when player collides and has Power Up
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(battleSound, 1.0f);
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            enemyRb.AddForce(awayFromPlayer * force, ForceMode.Impulse);
        }
        else if (collision.gameObject.CompareTag("Enemy") && !hasPowerUp) //OnCollison method to send enemy flying when player collides and has Power Up
        {
            gameOver = true;
            anim.SetBool("Death_b", true);
            anim.SetInteger("DeathType_int", 1);
            gameManager.GameOver();
        }
    }

}

