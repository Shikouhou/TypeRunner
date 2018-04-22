using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    public LayerMask playerMask;

    private Animator anim;
    private CharacterController controller;
    private Rigidbody2D rb;

    public Animator Anim { get { return anim; } }

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetWalk(bool condition)
    {
        anim.SetBool("walk", condition);
        controller.Walk(condition);
    }

    public void SetJump()
    {
        if (anim.GetBool("walk"))
        {          
            StartCoroutine(WaitForGround());
        }
            
    }

    private IEnumerator WaitForGround()
    {
        RaycastHit2D isGrounded = Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - 0.5f), playerMask);

        if (isGrounded.collider == null)
        {
            yield break;
        }

        controller.Jump();
        anim.SetBool("jump", true);

        
        while (isGrounded.collider != null)
        {
            isGrounded = Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - 0.5f), playerMask);
            yield return null;
        }

        while (isGrounded.collider == null)
        {
            isGrounded = Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - 0.5f), playerMask);
            yield return null;
        }

        anim.SetBool("jump", false);


    }
}
