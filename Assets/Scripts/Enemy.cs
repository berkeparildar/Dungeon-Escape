using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform pointA, pointB;
    public GameObject acid;
    public GameObject diamond;
    private Animator _animator;
    private bool _animPlaying;
    private BoxCollider2D _boxCollider2D;
    private int _diamondCount;
    private int _health;
    private bool _isAlive;
    private bool _isOnEnd;
    private bool _isSpider;
    private GameObject _player;
    private Player _playerScript;
    private int _speed;
    private SpriteRenderer _spriteRenderer;
    private bool _stagger;

    private void Start()
    {
        _health = 3;
        _speed = 1;
        _isAlive = true;
        _diamondCount = 5;
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _player = GameObject.Find("Player");
        _playerScript = _player.GetComponent<Player>();
        if (gameObject.name == "Spider") Spider();
    }

    private void Update()
    {
        Patrol();
    }

    public void TakeDamage()
    {
        _health--;
        _stagger = true;
        switch (_player.transform.position.x < transform.position.x)
        {
            case true:
                transform.eulerAngles = new Vector3(0, 180, 0);
                break;
            case false:
                _spriteRenderer.flipX = false;
                break;
        }

        _animator.SetTrigger(Animator.StringToHash("hit"));
        StartCoroutine(AnimationWait("hit", "hit_anim"));
        _animator.SetBool(Animator.StringToHash("inCombat"), true);
        _stagger = true;
        if (_health < 1)
        {
            _isAlive = false;
            _boxCollider2D.enabled = false;
            StartCoroutine(AnimationWait("die", "death_anim"));
        }
        Debug.Log(_health);
    }

    private void Patrol()
    {
        if (!_animPlaying && !_isSpider && _isAlive)
        {
            Movement();
            CheckPosition();
        }
    }

    private void CheckPosition()
    {
        if (transform.position == pointA.position)
        {
            _isOnEnd = false;
            StartCoroutine(AnimationWait("idle", "idle_anim"));
        }
        else if (transform.position == pointB.position)
        {
            _isOnEnd = true;
            StartCoroutine(AnimationWait("idle", "idle_anim"));
        }

        if (!(Vector2.Distance(transform.position, _player.transform.position) > 2)) return;
        _animator.SetBool(Animator.StringToHash("inCombat"), false);
        _animator.Play("walk_anim");
        _stagger = false;
    }

    private void Movement()
    {
        if (_stagger) return;
        var step = _speed * Time.deltaTime;
        switch (_isOnEnd)
        {
            case false:
                _spriteRenderer.flipX = false;
                transform.position = Vector3.MoveTowards(transform.position, pointB.position, step);
                break;
            case true:
                _spriteRenderer.flipX = true;
                transform.position = Vector3.MoveTowards(transform.position, pointA.position, step);
                break;
        }
    }

    private IEnumerator AnimationWait(string parameter, string clipName)
    {
        var clips = _animator.runtimeAnimatorController.animationClips;
        float length = 0;
        foreach (var clip in clips)
            if (clip.name == clipName)
                length = clip.length;
        _animPlaying = true;
        _animator.SetTrigger(Animator.StringToHash(parameter));
        if (!_isAlive)
        {
            var droppedDiamond = Instantiate(diamond, transform.position, Quaternion.identity);
            var diamondScript = droppedDiamond.GetComponent<Diamond>();
            diamondScript.diamondCount = _diamondCount;
            Destroy(gameObject, 5.0f);
        }

        yield return new WaitForSeconds(length);
        _animPlaying = false;
    }

    private void Spider()
    {
        _isSpider = true;
    }

    public void FireAcid()
    {
        Instantiate(acid, transform.position, quaternion.identity);
    }
}