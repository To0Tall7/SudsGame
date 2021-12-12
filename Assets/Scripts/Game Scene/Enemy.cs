using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    [SerializeField] float speed;
    private AudioSource enemyAudio;
    private SpriteRenderer enemySprite;
    private Animator playerAnimator;
    private Animator virusAnimator;
    private GameObject leftPlatform;
    private GameObject centerPlatform;
    private GameObject rightPlatform;
    private Rigidbody2D enemyRb;
    private float jumpModifier = 5.0f;
    private double platformLength = 13.2135 / 2;
    private bool isOnGround;
    private SpawnManager spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        rightPlatform = GameObject.Find("Right Platform");
        centerPlatform = GameObject.Find("Center Platform");
        leftPlatform = GameObject.Find("Left Platform");
        enemyAudio = GetComponent<AudioSource>();
        enemySprite = GetComponent<SpriteRenderer>();
        playerAnimator = player.GetComponent<Animator>();
        virusAnimator = GetComponent<Animator>();
        speed = 1.75f + .25f * spawnManager.getWaveNumber();

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

    private void FixedUpdate()
    {
        //If close to a lower platform, and not on top of that platform, and the player is on a platform...
        if ((Math.Abs(transform.position.x - leftPlatform.transform.position.x) <= platformLength + 1f || Math.Abs(transform.position.x - rightPlatform.transform.position.x) <= platformLength + 1f) && transform.position.y <= 1f && player.transform.position.y >= 1f && Math.Abs(transform.position.x - player.transform.position.x) <= 2*platformLength)
        {
            Jump();
        }
        //If close to the center platform, and not on top of that platform, and on top of one of the lower platforms, and the player is on the higher platform...
        if (Math.Abs(transform.position.x - centerPlatform.transform.position.x) <= platformLength + 1f && (transform.position.y <= 4f && transform.position.y >= 1f) && player.transform.position.y >= 4f && Math.Abs(transform.position.x - player.transform.position.x) <= 2 * platformLength)
        {
            Jump();
        }
    }

    void Die()
    {
        virusAnimator.SetTrigger("VirusIsDead");
        gameManager.UpdateScore(69);//Nice
        //Play death animation stuff here.
        Destroy(gameObject);//Enemy dies.
    }

    void FollowPlayer()//Follows the player (horizontally only).
    {
        if (player.transform.position.x != transform.position.x)
        {
            float xDirection = player.transform.position.x - transform.position.x;
            Vector2 direction = new Vector2(xDirection / Mathf.Abs(xDirection), 0f);//Either (-1,0) or (1,0).
            if (direction.x > 0)
            {
                enemySprite.flipX = true;
            }
            else
            {
                enemySprite.flipX = false;
            }
            virusAnimator.SetTrigger("VirusRun");
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    void DamagePlayer()
    {
        playerAnimator.SetTrigger("HurtPlayer");
        playerAnimator.SetTrigger("HurtPlayer");
        gameManager.UpdateLives(-1);
    }

    void Jump()
    {
        if (isOnGround)
        {
            enemyRb.AddForce(new Vector2(0f, 1.0f) * jumpModifier, ForceMode2D.Impulse);
        }
    }

    #region Collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hitbox"))//If the enemy gets punched...
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))//If the enemy collides with the player...
        {
            virusAnimator.Play("Virus Kick");
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
        }
        virusAnimator.SetTrigger("VirusIdle");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Platform") && enemyRb.velocity.y == 0))//If enemy touches the ground, or touches the platform AND IS NOT PASSING THROUGH THE PLATFORM...
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))//If enemy stops touching the ground/platform...
        {
            isOnGround = false;
        }
    }

    #endregion


}
