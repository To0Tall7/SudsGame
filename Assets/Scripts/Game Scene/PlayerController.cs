using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float jumpModifier = 10.0f;
    private bool isOnGround;
    private Rigidbody2D playerRb;
    private GameObject hitbox;
    private GameManager gameManager;
    //private bool isPunching = false;
    private float lastPunchTime = 0;//The last time the player punched, in game time.

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerRb = GetComponent<Rigidbody2D>();
        hitbox = transform.GetChild(0).gameObject;//Get the hitbox.
        hitbox.SetActive(false);//Immediately turn it off.
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameOver)
        {
            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector2(-1.0f, 0f) * speed * Time.deltaTime);
            hitbox.transform.localPosition = new Vector2(-1.0f, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector2(1.0f, 0f) * speed * Time.deltaTime);
            hitbox.transform.localPosition = new Vector2(1.0f, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(new Vector2(0f, 1.0f) * jumpModifier, ForceMode2D.Impulse);//Add an instantaneous force upward, i.e., a jump.
        }
        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastPunchTime >= 0.25f)//Punch if press E and it has been at least 0.5 seconds since last punch.
        {
            lastPunchTime = Time.time;
            StartCoroutine(PunchingCoroutine());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Platform") && playerRb.velocity.y == 0))//If player touches the ground, or touches the platform AND IS NOT PASSING THROUGH THE PLATFORM...
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))//If player stops touching the ground/platform...
        {
            isOnGround = false;
        }
    }

    private IEnumerator PunchingCoroutine()
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        hitbox.SetActive(false);
    }
}
