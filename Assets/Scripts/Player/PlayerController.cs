using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRigidbody;

    [Header("Speed setup")]
    public Vector2 friction = new Vector2(.1f, 0);
    public float speed;
    public float speedRun;
    public float forceJump = 2;

    [Header("Animation setup")]
    public float jumpScaleY = 1.5f;
    public float jumpScaleX = .7f;
    private float _currentSpeed;
    public float animationDuration = .3f;
    public Ease ease = Ease.OutBack;

    [Header("Animation player")]
    public string boolRun = "Run";
    public Animator animator;


   // private bool _isRunnig = false;

    [Header("Varicoes no impacto")]
    private float _lastVelocityY;
    private bool _isGrounded = false;
    private float _lastImpactTime = 0f;
    private float _impactCooldown = 0.25f; // tempo mínimo entre impactos

    public void Update()
    {
        HandleJump();
        HandleMoviment();
        DetectImpact();
    }

    public void HandleMoviment()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentSpeed = speedRun;
            animator.speed = 2;
        }
        else
        {
            _currentSpeed = speed;
            animator.speed = 1;

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myRigidbody.velocity = new Vector2(-_currentSpeed, myRigidbody.velocity.y);
            if(myRigidbody.transform.localScale.x != -1)
            {
                myRigidbody.transform.DOScaleX(-1, .1f);
            }

            animator.SetBool(boolRun, true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRigidbody.velocity = new Vector2(_currentSpeed, myRigidbody.velocity.y);
            if (myRigidbody.transform.localScale.x != 1)
            {
                myRigidbody.transform.DOScaleX(1, .1f);
            }
            animator.SetBool(boolRun, true);
        }
        else
        {
            animator.SetBool(boolRun, false);
        }

        if (myRigidbody.velocity.x > 0)
        {
            myRigidbody.velocity += friction;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            myRigidbody.velocity -= friction;
        }
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.velocity = Vector2.up * forceJump;
            myRigidbody.transform.localScale = Vector2.one;

            DOTween.Kill(myRigidbody.transform);

            HandleScaleJump();
        }
    }

    private void HandleScaleJump()
    {
        myRigidbody.transform.DOScaleY(jumpScaleY, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        myRigidbody.transform.DOScaleX(jumpScaleX, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);

    }
    //Detecta o momento de tocar o solo
    private void DetectImpact()
    {
        //Detecta se aprou de cair e ainda nao fez impacto recentemente
        bool landed = _lastVelocityY < -0.1f && Mathf.Abs(myRigidbody.velocity.y) < 0.05f;

        if (landed && !_isGrounded && Time.time - _lastImpactTime > _impactCooldown)
        {
            HandleImpact();
            _isGrounded = true;
            _lastImpactTime = Time.time; // registra o momento do impacto
        }
        // Se começou a subri, reseta o estado
        if (myRigidbody.velocity.y > 0.05f)
        {
            _isGrounded = false;
        }
        _lastVelocityY = myRigidbody.velocity.y;
    }
    //Animação de "encolher" ao tocar solo
    private void HandleImpact()
    {
        DOTween.Kill(myRigidbody.transform);

        myRigidbody.transform.localScale = Vector3.one;

        float impactScaleY = 0.7f; // Encolhe verticalmente
        float impactScaleX = 1.2f; // Aumenta um pouco horizotalmente
        float impactDuration = 0.15f;

        myRigidbody.transform.DOScaleY(impactScaleY, impactDuration)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo);

        myRigidbody.transform.DOScaleX(impactScaleX, impactDuration)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo);
    }
}
