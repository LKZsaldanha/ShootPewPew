﻿using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public bool dashLock = false;

    public bool isAlive = true;

    //id do player (p1 = 0, p2 = 1, p3 = 2, p4 = 3)
    public int playerID = 0;

    [SerializeField] private float moveSpeed = 1;
    private Animator myAnimator;
    private Rigidbody myRb;
    private bool facingRight = true;

    private bool isGrounded = false;
    private Collider[] groundCollisions;
    private Vector3 groundCheckSize = new Vector3 (0.5f,0.2f,0.4f);
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    [SerializeField] private float jumpHeight;
    private bool jumped = false;

    public bool aimingVertical = false;
    

    [SerializeField] private float dashForce;
    public float dashDuration;
    [SerializeField] private float dashCooldown;
    private float lastDashTime;

    //usado para checar se o player precisa fazer a animação de caindo (corrigindo bug de pequenos espaços onde o player "cai")
    private float timerToUnground;
    public float timerCheckFall = 0.5f;

    //NomeDoInput sem o numeroFinal
    private string horizontalAxisName = "HorizontalP";
    private string verticalAxisName = "VerticalP";
    private string jumpInputName = "JumpP";
    private string dashInputName = "RasteiraP";

    [SerializeField] private float controllerDeadZoneValue;



    private void Start () {
        myRb = GetComponent<Rigidbody>();
        myAnimator = GetComponentInChildren<Animator>();
        SetInputAxis();
    }

    private void SetInputAxis()
    {
        float convertedID = playerID + 1;
        horizontalAxisName = horizontalAxisName + convertedID.ToString();
        verticalAxisName = verticalAxisName + convertedID.ToString();
        jumpInputName = jumpInputName + convertedID.ToString();
        dashInputName = dashInputName + convertedID.ToString();
    }

    private void Update()
    {
        if (isAlive)
        {
            myAnimator.SetBool("Dashing", dashLock);
            if (!dashLock)
            {
                AimingVertical();
                if (!aimingVertical)
                {
                    Move();
                }

                if (isGrounded)
                {
                    Jump();
                    if (Time.time - lastDashTime >= dashCooldown)
                    {
                        Dash();
                    }
                }
            }
            else
            {
                if (Time.time - lastDashTime >= dashDuration)
                {
                    dashLock = false;
                    //myAnimator.SetBool("Dashing", dashLock);
                }
                else
                {

                    myAnimator.SetTrigger("Dash");
                    if (facingRight)
                    {
                        myRb.velocity = new Vector3(dashForce, myRb.velocity.y, 0);
                    }
                    else
                    {
                        myRb.velocity = new Vector3(-dashForce, myRb.velocity.y, 0);
                    }
                }

            }
            CheckGrounded();
        }

    }

    private void AimingVertical()
    {
        if (Input.GetAxis(horizontalAxisName) < controllerDeadZoneValue 
            && Input.GetAxis(horizontalAxisName) > -controllerDeadZoneValue)
        {

            myRb.velocity = new Vector3(0, myRb.velocity.y, 0);
            if (Input.GetAxis(verticalAxisName) > controllerDeadZoneValue)
            {
                myAnimator.SetInteger("VerticalAim", 1);
                aimingVertical = true;
            }
            else if (Input.GetAxis(verticalAxisName) < -controllerDeadZoneValue)
            {
                myAnimator.SetInteger("VerticalAim", -1);
                aimingVertical = true;
            }
            else
            {
                myAnimator.SetInteger("VerticalAim", 0);
                aimingVertical = false;
            }
        }
        else
        {
            aimingVertical = false;
            myAnimator.SetInteger("VerticalAim", 0);
        }
    }

    private void Dash()
    {
        if (Input.GetButtonDown(dashInputName))
        {
            //myAnimator.SetTrigger("Dash");
            dashLock = true;
            lastDashTime = Time.time;

        }
    }

    private void Move()
    {      
        float move = Input.GetAxis(horizontalAxisName);
        move = move * moveSpeed;
        myAnimator.SetFloat("MovementSpeed", Mathf.Abs(move));


        myRb.velocity = new Vector3(move, myRb.velocity.y, 0);
            


        if(move>0 && !facingRight)
        {
            Flip();
        }else if(move<0 && facingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown(jumpInputName))
        {
            jumped = true;
            //isGrounded = false;
            myAnimator.SetBool("Grounded", isGrounded);
            myAnimator.SetTrigger("Air");
            myRb.velocity = new Vector3(0, jumpHeight, 0);
        }
    }
    private void CheckGrounded()
    {
        groundCollisions = Physics.OverlapBox(groundCheck.position, groundCheckSize / 2, Quaternion.identity, groundLayer);        
        if (groundCollisions.Length > 0)
        {
            if (!isGrounded)
            {
                isGrounded = true;
                jumped = false;
            }
            timerToUnground = 0;
        }
        else
        {
            if (isGrounded)
            {
                if(!jumped){
                    timerToUnground += Time.deltaTime;
                    if(timerToUnground > timerCheckFall){
                        isGrounded = false;
                        myAnimator.SetTrigger("Air");
                    }
                }else{
                    isGrounded = false;
                    myAnimator.SetTrigger("Air");
                }
            }
        }
        myAnimator.SetBool("Grounded", isGrounded);

    }
    

    

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
