using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20;
    public float maxSpeed = 25;
    public float upSpeed = 12;
    public AudioClip marioDeath;
    public float deathImpulse = 15;
    public AudioSource marioAudio;
    public AudioSource jumpHoldAudio;
    public GameObject enemies;
    public Animator marioAnimator;
    public Transform gameCamera;
    public GameManager gameManager;

    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool onGroundState = true;
    private bool faceRightState = true;
    private bool alive = true;
    private bool moving = false;
    private bool jumpedState = false;

    int collisionLayerMask = (1 << 6) | (1 << 7) | (1 << 8);

    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();
        marioBody = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 30;
        marioAnimator.SetBool("onGround", onGroundState);
    }

    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState ? 1 : -1);
        }
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            marioAnimator.SetBool("onGround", onGroundState);
            PlayJumpSound();
        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            if (!jumpHoldAudio.isPlaying)
                jumpHoldAudio.Play();
        }
        else if (onGroundState || !alive)
        {
            if (jumpHoldAudio.isPlaying)
                jumpHoldAudio.Stop();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && alive)
        {
            Vector2 contactNormal = col.contacts[0].normal;

            if (contactNormal.y > 0.5f)
            {
                GoombaStomp stomp = col.gameObject.GetComponent<GoombaStomp>();
                if (stomp != null)
                    stomp.OnStompFromPlayer();

                marioBody.linearVelocity = new Vector2(marioBody.linearVelocity.x, 0);
                marioBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            }
            else
            {
                marioAnimator.Play("mario-die");
                marioAudio.PlayOneShot(marioDeath);
                alive = false;
                if (jumpHoldAudio.isPlaying)
                    jumpHoldAudio.Stop();

                if (gameManager != null)
                    gameManager.GameOver();
            }
        }

        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) && !onGroundState)
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
            if (jumpHoldAudio.isPlaying)
                jumpHoldAudio.Stop();
        }
    }



    public void GameRestart()
    {
        marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        faceRightState = true;
        marioSprite.flipX = false;
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        gameCamera.position = new Vector3(0, 0, -10);
        if (jumpHoldAudio.isPlaying)
            jumpHoldAudio.Stop();
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene()
    {
        Time.timeScale = 0.0f;
    }

    void PlayJumpSound()
    {
        marioAudio.PlayOneShot(marioAudio.clip);
    }
}
