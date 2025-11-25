using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRididbody;
    public HealthBase healthBase;

    [Header("Speed Setup")]
    public Vector2 friction = new Vector2(.1f, 0);
    public float speed;
    public float speedRun;
    public float forceJump = 2;

    [Header("Animation Setup")]
    public float jumpScaleY = 1.5f;
    public float jumpScaleX = 0.8f;
    public float animationDuration = .3f;
    public Ease ease = Ease.OutBack;

    [Header("Animation Player")]
    public string boolRun = "Run";
    public string triggerDeath = "Death";
    public Animator animator;
    public float playerSwipeDuration = .1f;

    private float _currentSpeed;
    //private bool _isRunning = false;

    private float _direction = 1;


    private void Awake()
    {
        if (healthBase != null)
        {
            healthBase.OnKill += OnPlayerKill;
        }
    }
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
    private void OnPlayerKill()
    {
        healthBase.OnKill -= OnPlayerKill;

        animator.SetTrigger(triggerDeath);
    }

    private void Update()
    {
        HandleJump();
        HandleMoviment();
    }
    private void HandleMoviment()
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
            myRididbody.velocity = new Vector2(-_currentSpeed, myRididbody.velocity.y);
            if (myRididbody.transform.localScale.x != -1)
            {
                _direction = -1;
                myRididbody.transform.DOScaleX(-1, playerSwipeDuration);
            }
            animator.SetBool(boolRun, true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRididbody.velocity = new Vector2(+_currentSpeed, myRididbody.velocity.y);
            if (myRididbody.transform.localScale.x != 1)
            {
                _direction = 1;
                myRididbody.transform.DOScaleX(1, playerSwipeDuration);
            }
            animator.SetBool(boolRun, true);
        }
        else
        {
            animator.SetBool(boolRun, false);
        }

        if (myRididbody.velocity.x > 0)
        {
            myRididbody.velocity += friction;
        }
        else if (myRididbody.velocity.x < 0)
        {
            myRididbody.velocity -= friction;
        }
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRididbody.velocity = Vector2.up * forceJump;
            transform.localScale = new Vector2(_direction, 1);

            transform.DOKill();
            HandleScaleJump();
        }
    }

    private void HandleScaleJump()
    {
        float dir = _direction;

        transform.DOScaleY(jumpScaleY, animationDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(ease);

        transform.DOScaleX(jumpScaleX * dir, animationDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(ease);
    }
    private void OnDestroy()
    {
        transform.DOKill();
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}