using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    private float speed = 2.0f;
    private AudioSource enemyAudio;
    private Animator VirusAnimator;
    private Animator PlayerAnimator;
    private SpriteRenderer PlayerSprite;

    // private bool VirusIsRunning, VirusIsJumping, VirusIsKicking, VirusIsDead, VirusIsLanding, VirusIsSpawning = false;
    private bool VirusIsRunning, VirusIsKicking = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAudio = GetComponent<AudioSource>();
        PlayerAnimator = GetComponent<Animator>();
        VirusAnimator = GetComponent<Animator>();
        PlayerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        PlayerSprite = GetComponent<SpriteRenderer>();
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
        // VirusAnimator.SetBool("VirusIsDead", true);
        gameManager.UpdateScore(69);//Nice
        //Play death animation stuff here.
        Destroy(gameObject);//Enemy dies.
    }

    void FollowPlayer()//Follows the player (horizontally only).
    {
        if (player.transform.position.x != transform.position.x)
        {
            VirusAnimator.SetBool("VirusIsRunning", true);
            float xDirection = player.transform.position.x - transform.position.x;
            Vector2 direction = new Vector2(xDirection / Mathf.Abs(xDirection), 0f);//Either (-1,0) or (1,0).
            transform.Translate(direction * speed * Time.deltaTime);
            VirusAnimator.SetBool("VirusIsRunning", false);
        }
    }

    void DamagePlayer()
    {
        PlayerAnimator.SetTrigger("isHurt");
        gameManager.UpdateLives(-1);
    }

    #region Collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hitbox"))//If the enemy gets punched...
        {
            // VirusAnimator.SetBool("VirusIsDead", true);
            Die();
            // VirusAnimator.SetBool("VirusIsRunning", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))//If the enemy collides with the player...
        {
            VirusAnimator.SetBool("VirusIsKicking", true);
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
            VirusAnimator.SetBool("VirusIsKicking", false);
        }
    }

    #endregion

    
}
