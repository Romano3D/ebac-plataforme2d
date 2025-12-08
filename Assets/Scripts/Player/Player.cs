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



    /*[Header("Animation Setup")]
    [Header("Speed Setup")]
    public Vector2 friction = new Vector2(.1f, 0);
    public float speed;
    public float speedRun;
    public float forceJump = 2;

    public float jumpScaleY = 1.5f;
    public float jumpScaleX = 0.8f;
    public float animationDuration = .3f;
    public SOFloat sojumpScaleY;
    public SOFloat sojumpScaleX;
    public SOFloat soanimationDuration;

    public Ease ease = Ease.OutBack;

    [Header("Animation Player")]
    public string boolRun = "Run";
    public string triggerDeath = "Death";
    public float playerSwipeDuration = .1f;*/

    [Header("Setup")]
    public SOPlayerSetup soPlayerSetup;

    //public Animator animator;

    private float _currentSpeed;
    //private bool _isRunning = false;

    private Animator _currentPlayer;

    private float _direction = 1;

    [Header("Jump Collision Check")]
    public Collider2D groundCheck;
    public float distToGround;
    public float spaceToGround = .1f;
    public ParticleSystem jumpVFX;





    private void Awake()
    {
        if (healthBase != null)
        {
            healthBase.OnKill += OnPlayerKill;
        }

        _currentPlayer = Instantiate(soPlayerSetup.player, transform);

        if (groundCheck != null)
        {
            distToGround = groundCheck.bounds.extents.y;
        }
    }

    private bool IsGrounded()
    {
        Debug.DrawLine(transform.position, -Vector2.up, Color.magenta, distToGround + spaceToGround);
        return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + spaceToGround);
    }
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
    private void OnPlayerKill()
    {
        healthBase.OnKill -= OnPlayerKill;

        _currentPlayer.SetTrigger(soPlayerSetup.triggerDeath);
    }

    private bool _isRunningVFXPlaying = false;
    private void Update()
    {
        IsGrounded();
        HandleJump();
        HandleMoviment();
    }
    private void HandleMoviment()
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
            myRididbody.velocity = new Vector2(-_currentSpeed, myRididbody.velocity.y);
            if (myRididbody.transform.localScale.x != -1)
            {
                _direction = -1;
                myRididbody.transform.DOScaleX(-1, soPlayerSetup.playerSwipeDuration);
            }
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRididbody.velocity = new Vector2(+_currentSpeed, myRididbody.velocity.y);
            if (myRididbody.transform.localScale.x != 1)
            {
                _direction = 1;
                myRididbody.transform.DOScaleX(1, soPlayerSetup.playerSwipeDuration);
            }
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
        }
        else
        {
            _currentPlayer.SetBool(soPlayerSetup.boolRun, false);
        }

        if (myRididbody.velocity.x > 0)
        {
            myRididbody.velocity += soPlayerSetup.friction;
        }
        else if (myRididbody.velocity.x < 0)
        {
            myRididbody.velocity -= soPlayerSetup.friction;
        }
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            myRididbody.velocity = Vector2.up * soPlayerSetup.forceJump;
            transform.localScale = new Vector2(_direction, 1);

            transform.DOKill();
            HandleScaleJump();
            PlayJumpVFX();
        }
    }

    private void PlayJumpVFX()
    {
        VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.JUMP, transform.position);
        // if (jumpVFX != null) jumpVFX.Play();
    }

    private void HandleScaleJump()
    {
        float dir = _direction;

        transform.DOScaleY(soPlayerSetup.jumpScaleY, soPlayerSetup.animationDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(soPlayerSetup.ease);

        transform.DOScaleX(soPlayerSetup.jumpScaleX * dir, soPlayerSetup.animationDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(soPlayerSetup.ease);
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
