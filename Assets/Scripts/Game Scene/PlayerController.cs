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
    private AudioSource playerAudio;
    [SerializeField] AudioClip[] jumpSounds;
    private int jumpSoundsLength;
    [SerializeField] AudioClip[] punchSounds;
    private int punchSoundsLength;
    private Animator playerAnimator;
    // private bool IsRunning, IsJumping, IsPunching, IsHurt, IsDead, IsLanding = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        jumpSoundsLength = jumpSounds.Length;
        punchSoundsLength = punchSounds.Length;
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
            // playerAnimator.SetBool("isRunning", true);
            transform.Translate(new Vector2(-1.0f, 0f) * speed * Time.deltaTime);
            hitbox.transform.localPosition = new Vector2(-1.0f, 0);
            // playerAnimator.SetBool("isRunning", false);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            playerAnimator.SetBool("isRunning", true);
            transform.Translate(new Vector2(1.0f, 0f) * speed * Time.deltaTime);
            hitbox.transform.localPosition = new Vector2(1.0f, 0);
            playerAnimator.SetBool("isRunning", false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerAnimator.SetBool("isJumping", true);
            playerRb.AddForce(new Vector2(0f, 1.0f) * jumpModifier, ForceMode2D.Impulse);//Add an instantaneous force upward, i.e., a jump.
            PlayJumpSound();
            playerAnimator.SetBool("isJumping", false);
        }
        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastPunchTime >= 0.25f)//Punch if press E and it has been at least 0.5 seconds since last punch.
        {
            playerAnimator.SetBool("isPunching", true);
            lastPunchTime = Time.time;
            StartCoroutine(PunchingCoroutine());
            playerAnimator.SetBool("isPunching", false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Platform") && playerRb.velocity.y == 0))//If player touches the ground, or touches the platform AND IS NOT PASSING THROUGH THE PLATFORM...
        {
            playerAnimator.SetBool("IsLanding", true);
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
        PlayPunchSound();
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        hitbox.SetActive(false);
    }

    private void PlayJumpSound()
    {
        int jumpSoundIndex = Random.Range(0, jumpSoundsLength);
        playerAudio.PlayOneShot(jumpSounds[jumpSoundIndex]);
    }

    private void PlayPunchSound()
    {
        int punchSoundIndex = Random.Range(0, punchSoundsLength);
        playerAudio.PlayOneShot(punchSounds[punchSoundIndex]);
    }
}
