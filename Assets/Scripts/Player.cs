using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private float _jumpForce = 5.0f;
    private int _health = 4;
    private float _speed = 3.0f;
    public int diamondCount;
    private bool _isGrounded;
    private Animator _playerAnimator;
    private Animator _swordAnimator;
    private SpriteRenderer _sprite;
    private SpriteRenderer _swordArc;
    public LayerMask groundLayer;
    private bool _jumpCoolDown;
    private BoxCollider2D _boxCollider2D;

    public bool IsAlive { get; private set; }

    private GameObject _hud;
    private Image[] _lifeUnits;


    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        _swordAnimator = transform.GetChild(1).GetComponent<Animator>();
        _sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _swordArc = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _hud = UIManager.Instance.gameObject.transform.GetChild(1).gameObject;
        _lifeUnits = _hud.transform.GetChild(1).GetComponentsInChildren<Image>();
        IsAlive = true;
    }

    // Update is called once per frame
    private void Update()
    {
       Movement();
       Jump();
       Attack();
       GroundCheck();
    }

    private void Movement()
    {
        if (IsAlive)
        {
            var move = CrossPlatformInputManager.GetAxis("Horizontal");
            Flip(move);
            _rigidbody2D.velocity = new Vector2(move * _speed, _rigidbody2D.velocity.y);
            _playerAnimator.SetFloat(Animator.StringToHash("move"), Mathf.Abs(move));
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("b_button") && _isGrounded)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            PlayJumpAnimation(_isGrounded);
            _isGrounded = false;
            _jumpCoolDown = true;
            StartCoroutine(JumpCooldown());
        }
        //Debug.Log(diamondCount);
    }

    private void Attack()
    {
        if (CrossPlatformInputManager.GetButtonDown("a_button"))
        {
            _playerAnimator.SetTrigger(Animator.StringToHash("attack"));
            _swordAnimator.SetTrigger(Animator.StringToHash("attack"));
        }
    }

    private void GroundCheck()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, groundLayer.value);
        if (hit2D.collider != null)
        {
            if (_jumpCoolDown == false)
            { 
                _isGrounded = true;
               PlayJumpAnimation(!_isGrounded);
            }
        }
    }

    private void PlayJumpAnimation(bool jump)
    {
        _playerAnimator.SetBool(Animator.StringToHash("jump"), jump);
    }

    private void Flip(float move)
    {
        var swordArcTransform = _swordArc.transform;
        switch (move)
        {
            case > 0:
            {
                _sprite.flipX = false;
                _swordArc.flipX = false;
                _swordArc.flipY = false;
                var pos = swordArcTransform.localPosition;
                pos.x = 1.01f;
                swordArcTransform.localPosition = pos;
                break;
            }
            case < 0:
            {
                _sprite.flipX = true;
                _swordArc.flipX = true;
                _swordArc.flipY = true;
                var pos = swordArcTransform.localPosition;
                pos.x = -1.01f;
                swordArcTransform.localPosition = pos;
                break;
            }
            default:
                _sprite.flipX = _sprite.flipX;
                break;
        }
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        _jumpCoolDown = false;
    }

    public void TakeDamage()
    {
        if (IsAlive)
        {
            _lifeUnits[_health - 1].enabled = false;
            _health--;
            if (_health < 1)
            {
                IsAlive = false;
                _playerAnimator.SetTrigger("die");
            }
        }
    }

    public void AddGem()
    {
        diamondCount++;
        UIManager.Instance.UpdateUIGemCount(diamondCount);
    }
}
