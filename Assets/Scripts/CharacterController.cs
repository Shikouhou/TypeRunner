using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    [Range(0,10)]
    public float walkSpeed;
    public float fallSpeed = 2.5f;
    public float jumpMultiplier = 2f;
    [Range(1,10)]
    public float jumpVelocity;
    public FadePanel darkness;

    [Header("Sound Effects")]
    public AudioClip jump;
    public AudioClip keyboard;
    public AudioClip points;
    public AudioClip wrong;

    private Rigidbody2D rb;
    private AudioSource sfx;

    private bool isWalking;
    private bool playerFell;

    private Vector3 spawnPos;

	void Start () {
        spawnPos = transform.localPosition;
        isWalking = false;
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponent<AudioSource>();
        darkness.visible = false;

        int level = GetComponent<PointsAdder>().scoreManager.LevelPoints;

        if (level > 0 || level <= 2)
        {
            return;
        }
        else if (level > 2 && level <= 4)
        {
            walkSpeed *= 1.2f;
        }
        else if (level > 4 && level <= 8)
        {
            walkSpeed *= 1.3f;
        }
        else if (level > 6 && level <= 10)
        {
            walkSpeed *= 1.4f;
        }
        else if (level > 10)
        {
            walkSpeed *= 1.5f;
        }
	}
	
	void Update () {
		if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;
        }

        if (isWalking)
        {
            transform.Translate((Vector2.right * walkSpeed) * Time.deltaTime);
        }

        if (rb.velocity.y < -20 && !playerFell)
        {
            playerFell = true;
            sfx.PlayOneShot(wrong);
            GetComponent<PlayerLives>().RemoveLife();
            StartCoroutine(RespawnPlayer());
        }
	}

    private IEnumerator RespawnPlayer()
    {
        darkness.visible = true;
        GetComponent<CharacterAnimation>().SetWalk(false);
        yield return new WaitForSecondsRealtime(1);
        Camera.main.GetComponentInParent<CameraFollowPlayer>().transform.position = new Vector3(spawnPos.x, spawnPos.y, Camera.main.GetComponentInParent<CameraFollowPlayer>().transform.position.z);
        transform.localPosition = spawnPos;
        rb.velocity = Vector3.zero;
        playerFell = false;
        yield return new WaitForSecondsRealtime(1);
        darkness.visible = false;
    }

    public void Jump()
    {
        rb.velocity = Vector2.up * jumpVelocity;
        sfx.PlayOneShot(jump);
    }

    public void Walk(bool condition)
    {
        isWalking = condition;
    }
}
