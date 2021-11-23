using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    private float speed = 2.0f;
    private AudioSource enemyAudio;
    private Animator virusAnimator;
    private Animator playerAnimator;
    // private bool VirusIsRunning, VirusIsJumping, VirusIsKicking, VirusIsDead, VirusIsLanding, VirusIsSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAudio = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        virusAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameOver)
        {
            Destroy(gameObject);
        }
        FollowPlayer();
    }

    void Die()
    {
        // virusAnimator.SetBool("VirusIsDead", true);
        gameManager.UpdateScore(69);//Nice
        //Play death animation stuff here.
        Destroy(gameObject);//Enemy dies.
    }

    void FollowPlayer()//Follows the player (horizontally only).
    {
        if (player.transform.position.x != transform.position.x)
        {
            virusAnimator.SetBool("VirusIsRunning", true);
            float xDirection = player.transform.position.x - transform.position.x;
            Vector2 direction = new Vector2(xDirection / Mathf.Abs(xDirection), 0f);//Either (-1,0) or (1,0).
            transform.Translate(direction * speed * Time.deltaTime);
            virusAnimator.SetBool("VirusIsRunning", false);
        }
    }

    void DamagePlayer()
    {
        // playerAnimator.SetBool("isHurt", true);
        gameManager.UpdateLives(-1);
        // playerAnimator.SetBool("isHurt", false);
    }

    #region Collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hitbox"))//If the enemy gets punched...
        {
            // virusAnimator.SetBool("VirusIsDead", true);
            Die();
            // virusAnimator.SetBool("VirusIsRunning", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))//If the enemy collides with the player...
        {
            virusAnimator.SetBool("VirusIsKicking", true);
            if (transform.position.x >= collision.transform.position.x)//...and the enemy is to the right of the player...
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10.0f, 0f), ForceMode2D.Impulse);//Send the player left.
                DamagePlayer();
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10.0f, 0f), ForceMode2D.Impulse);//Send the player right.
                DamagePlayer();
            }
            virusAnimator.SetBool("VirusIsKicking", false);
        }
    }

    #endregion

    
}
