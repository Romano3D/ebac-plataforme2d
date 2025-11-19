using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public HealthBase healfBase;

    [Header("Setup")]
    public SOPlayerSetup soPlayerSetup;

    //public Animator animator;

    private Animator _currentPlayer;

    private void Awake()
    {
       if(healfBase != null)
        {
            healfBase.OnKill += OnPlayerKill;
        }

        _currentPlayer = Instantiate(soPlayerSetup.player, transform);
    }
    private void OnPlayerKill()
    {
        healfBase.OnKill -= OnPlayerKill;

        _currentPlayer.SetTrigger(soPlayerSetup.triggerDeath);
    }

    // private bool _isRunnig = false;

    [Header("Varicoes no impacto")]
    private float _lastVelocityY;
    private bool _isGrounded = false;
    private float _lastImpactTime = 0f;
    private float _impactCooldown = 0.25f; // tempo mínimo entre impactos
    private float _currentSpeed;

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
            _currentSpeed = soPlayerSetup.speedRun;
            _currentPlayer.speed = 2;
        }
        else
        {
            _currentSpeed = soPlayerSetup.speed;
            _currentPlayer.speed = 1;

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myRigidbody.velocity = new Vector2(-_currentSpeed, myRigidbody.velocity.y);
            if(myRigidbody.transform.localScale.x != -1)
            {
                myRigidbody.transform.DOScaleX(-1, .1f);
            }

            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRigidbody.velocity = new Vector2(_currentSpeed, myRigidbody.velocity.y);
            if (myRigidbody.transform.localScale.x != 1)
            {
                myRigidbody.transform.DOScaleX(1, .1f);
            }
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
        }
        else
        {
            _currentPlayer.SetBool(soPlayerSetup.boolRun, false);
        }

        if (myRigidbody.velocity.x > 0)
        {
            myRigidbody.velocity += soPlayerSetup.friction;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            myRigidbody.velocity -= soPlayerSetup.friction;
        }
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.velocity = Vector2.up * soPlayerSetup.forceJump;
            myRigidbody.transform.localScale = Vector2.one;

            DOTween.Kill(myRigidbody.transform);

            HandleScaleJump();
        }
    }

    private void HandleScaleJump()
    {
        myRigidbody.transform.DOScaleY(soPlayerSetup.jumpScaleY, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
        myRigidbody.transform.DOScaleX(soPlayerSetup.jumpScaleX, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);

    }

    public void DestroyMe()
    {
        Destroy(gameObject);
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
        float impactScaleX = 1f; // Aumenta um pouco horizotalmente
        float impactDuration = 0.15f;

        myRigidbody.transform.DOScaleY(impactScaleY, impactDuration)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo);

        myRigidbody.transform.DOScaleX(impactScaleX, impactDuration)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo);
    }
}
