using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRB;

    private Animator myAni;


    [SerializeField]
    private float movSpeed = 10f;

    private float jumpForce = 500f;

    private bool attack;

    private bool slide;

    private bool jump;

    private bool facingRight;

    Vector2 startPos;

    public string jumpableLayer;

    float groundRayLen;

    float health;

    public float maxHealth = 300;

    float healthPerc;

    public Transform healthFillImage;

    public Text healthText;

    bool isFalling = false;

    public float fallMod = 3f;


    void Start()
    {
        facingRight = true;
        myRB = GetComponent<Rigidbody2D>();
        myAni = GetComponent<Animator>();
        groundRayLen = (GetComponent<BoxCollider2D>().bounds.extents.y);
        health = maxHealth;
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        HandleMovement(h);

        Flip(h);

        HandleAttacks();

        ResetValues();

        #region Health Bar
        health = Mathf.Clamp(health, 0, maxHealth);

        healthPerc = health / maxHealth;

        healthPerc = Mathf.Clamp(healthPerc, 0, 1);

        healthText.text = "" + Mathf.RoundToInt(healthPerc * 100) + "%";

        healthFillImage.localScale = new Vector2(healthPerc, healthFillImage.localScale.y);

        if (Input.GetKeyDown(KeyCode.J))
        {
            health -= 20f;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            health += 20f;
        }
        #endregion
    }

    private void HandleMovement(float h)
    {
        if (!myAni.GetBool("slide") && !this.myAni.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRB.velocity = new Vector2(h * movSpeed, myRB.velocity.y);
        }

        #region Slide Mechanic
        if (slide && !this.myAni.GetAnimatorTransitionInfo(0).IsName("Slide"))
        {
            myAni.SetBool("slide", true);
        }
        else if (!this.myAni.GetAnimatorTransitionInfo(0).IsName("Slide"))
        {
            myAni.SetBool("slide", false);
        }
        #endregion

        #region Jump Mechanic
        if (jump && !myAni.GetBool("slide") && !this.myAni.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRB.AddForce(Vector2.up * jumpForce);
            //myAni.SetTrigger("Jump");

            if (jump && !this.myAni.GetAnimatorTransitionInfo(0).IsName("Jump"))
            {
                //myAni.SetTrigger("Jump");
                myAni.SetBool("jump", true);
            }
            else if (!this.myAni.GetAnimatorTransitionInfo(0).IsName("Jump"))
            {
                //myAni.SetTrigger("Jump");
                myAni.SetBool("jump", false);
            }
        }
        #endregion

        #region Fall Mechanic


        if (myRB.velocity.y <= 0 && !isFalling && !IsGrounded())
        {
            isFalling = true;
            myAni.SetTrigger("Fall");
        }

        if (isFalling && IsGrounded())
        {
            isFalling = false;
        }
        #endregion

        myAni.SetFloat("speed", Mathf.Abs(h));
    }

    private void HandleAttacks()
    {
        if (attack && !this.myAni.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAni.SetTrigger("attack");
            myRB.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attack = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            slide = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jump = true;
        }
 
    }

    private void Flip(float h)
    {
        if (h > 0 && !facingRight || h<0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 curScale = transform.localScale;
            curScale.x *= -1;
            transform.localScale = curScale;
        }
    }

    private void ResetValues()
    {
        attack = false;
        slide = false;
        jump = false;
    }

    bool IsGrounded()
    {
        startPos = transform.position;
        startPos.y = transform.position.y - (groundRayLen + 0.01f);
        return Physics2D.Raycast(startPos, -transform.up, 0.1f);
    }
}
