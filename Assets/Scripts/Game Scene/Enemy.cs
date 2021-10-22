using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        gameManager.UpdateScore(69);//Nice
        //Play death animation stuff here.
        Destroy(gameObject);//Enemy dies.
    }

    void DamagePlayer()
    {
        gameManager.UpdateLives(-1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hitbox"))
        {
            Die();
        }
    }

}
