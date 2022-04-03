using System;
using System.Collections;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _NBGames.Scripts
{
    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpSpeed = 20f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private BoxCollider2D _feetCollider;
        [SerializeField] private float _invincibilityTime = 2f;
        [SerializeField] private float _bounceVelocity = 14f;
        [SerializeField] private float _timeAfterDeath = 1f;

        [Tooltip("The amount of times the player blinks while invincible.")]
        [SerializeField] private int _blinkAmount = 3;
        
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private bool _canMove = true;
        private bool _isAlive = true;
        private bool _invincible, _hasHorizontalVelocity;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _respawnPosition, _jumpVelocity;
        private float _horizontalMovement, _blinkSpeed;

        private Color _opaqueColor = new Color(1f, 1f, 1f, 1f);
        private Color _blinkColor = new Color(1f, 1f, 1f, 0.5f);

        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int JumpPeak = Animator.StringToHash("Jump_Peak");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Dead = Animator.StringToHash("Dead");

        private int _playerLayer, _enemyLayer;
        

        public bool Invincible => _invincible;

        private void Awake()
        {
            CacheComponents();
        }

        private void OnEnable()
        {
            EventManager.onDamagePlayer += TakeDamage;
            EventManager.onUpdateRespawnPosition += UpdateRespawnPosition;
            EventManager.onUnlockLevel += StopMovement;
            EventManager.onLoadLevel += StopMovement;
            EventManager.onSpringLaunch += SpringLaunch;
            EventManager.onDeathZoneTouched += DeathZoneCollision;
        }
        
        private void OnDisable()
        {
            EventManager.onDamagePlayer -= TakeDamage;
            EventManager.onUpdateRespawnPosition -= UpdateRespawnPosition;
            EventManager.onUnlockLevel -= StopMovement;
            EventManager.onLoadLevel -= StopMovement;
            EventManager.onSpringLaunch -= SpringLaunch;
            EventManager.onDeathZoneTouched -= DeathZoneCollision;
        }

        private void Start()
        {
            _respawnPosition = transform.position;
            GameManager.instance.CachePlayer(this);

            _playerLayer = LayerMask.NameToLayer("Player");
            _enemyLayer = LayerMask.NameToLayer("Enemy");
        }

        private void Update()
        {
            if (!_canMove) return;
            Movement();
            Jump();
            Animations();
        }

        private void CacheComponents()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (!_rigidbody2D)
            {
                Debug.LogError($"RigidBody2D is null on {gameObject.name}");
            }

            if (!_animator)
            {
                Debug.LogError($"Animator is null on {gameObject.name}");
            }

            if (!_spriteRenderer)
            {
                Debug.LogError($"SpriteRenderer is null on {gameObject.name}");
            }
        }
        
        private void Movement()
        {
            _horizontalMovement = Input.GetAxisRaw("Horizontal");
            _rigidbody2D.velocity = new Vector2((_horizontalMovement * _moveSpeed), _rigidbody2D.velocity.y);
        }

        private void Jump()
        {
            if (!Input.GetButtonDown("Jump") || !IsTouchingGround()) return;
            _jumpVelocity = new Vector2(0f, _jumpSpeed);

            if (!_rigidbody2D) return;
            _rigidbody2D.velocity += _jumpVelocity;
            
            SoundManager.instance.PlaySound(SoundEffect.PlayerJump);
        }

        private void Animations()
        {
            FlipSprite();

            if (!_animator) return;

            _animator.SetBool(Running, _hasHorizontalVelocity);

            if (!IsTouchingGround())
            {
                if (_rigidbody2D.velocity.y <= 2 && _rigidbody2D.velocity.y > -2.95f)
                {
                    _animator.SetBool(JumpPeak, true);
                }
                else if (_rigidbody2D.velocity.y > 0)
                {
                    _animator.SetBool(Jumping, true);
                    _animator.SetBool(Running, false);
                    _animator.SetBool(Grounded, false);
                }
                else
                {
                    _animator.SetBool(Jumping, false);
                    _animator.SetBool(JumpPeak, false);
                    _animator.SetBool(Grounded, false);
                }
            }
            else
            {
                _animator.SetBool(Grounded, true);
            }
        }

        private void FlipSprite()
        {
            _hasHorizontalVelocity = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;

            if (!_hasHorizontalVelocity || !_rigidbody2D) return;
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x), 1f);
        }

        private bool IsTouchingGround()
        {
            return _feetCollider.IsTouchingLayers(_groundLayer);
        }
        
        private void DeathZoneCollision()
        {
            GameManager.instance.PlayerDeath();
        }

        private void TakeDamage(int damageAmount)
        {
            if (_invincible || !_isAlive) return;
            StartCoroutine(KnockBack());
            StartCoroutine(Invincibility());
        }

        private IEnumerator KnockBack()
        {
            SoundManager.instance.PlaySound(SoundEffect.PlayerHurt);
            
            _animator.SetBool(Hurt, true);
            _animator.SetBool(Grounded, false);

            if (_isAlive)
            {
                _rigidbody2D.velocity = transform.localScale.x > 0 ? new Vector2(-5f, 16f) : new Vector2(5f, 16f);
            }

            yield return new WaitForSeconds(0.3f);
            
            ZeroVelocity();

            if (_isAlive)
            {
                _canMove = true;
            }
            
            _animator.SetBool(Hurt, false);
        }

        private IEnumerator Invincibility()
        {
            _invincible = true;
            Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer, true);
            StartCoroutine(Blink());
            yield return new WaitForSeconds(_invincibilityTime);
            _invincible = false;
            Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer, false);
        }

        private IEnumerator Blink()
        {
            _blinkSpeed = (_invincibilityTime / 10) / 2;
            
            if (!_spriteRenderer) yield break;
            for (var i = 0; i < _blinkAmount; i++)
            {
                _spriteRenderer.color = _blinkColor;
                yield return new WaitForSeconds(_blinkSpeed);
                _spriteRenderer.color = _opaqueColor;
                yield return new WaitForSeconds(_blinkSpeed);
            }
        }

        public IEnumerator Die()
        {
            Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer, true);
            
            _canMove = false;
            _isAlive = false;
            
            ZeroVelocity();
            SoundManager.instance.PlaySound(SoundEffect.PlayerDeath);
            _animator.SetBool(Dead, true);
            yield return new WaitForSeconds(_timeAfterDeath);
            
            UIManager.instance.Fade(true);
            
            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(2);
            GameManager.instance.ResetPlayerHealth();
            EventManager.RespawnWorldObjects();
            
            transform.position = _respawnPosition;
            _animator.SetBool(Dead, false);
            
            UIManager.instance.Fade(false);

            _canMove = true;
            _isAlive = true;
            
            Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer, false);
        }

        private void ZeroVelocity()
        {
            _rigidbody2D.velocity = Vector2.zero;
        }

        public void BounceOffEnemy()
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _bounceVelocity);
        }
        
        private void UpdateRespawnPosition(Vector2 newPosition)
        {
            _respawnPosition = newPosition;
        }

        private void StopMovement(int levelToUnlock)
        {
            StopMovement();
        }

        private void StopMovement()
        {
            _canMove = false;

            if (!_animator || !_rigidbody2D) return;

            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetBool(Jumping, false);
            _animator.SetBool(Running, false);
            _animator.SetBool(JumpPeak, false);
            _animator.SetBool(Grounded, true);
        }
        
        private void SpringLaunch(float launchHeight)
        {
            if (!_rigidbody2D) return;
            _animator.SetBool(Jumping, true);
            _animator.SetBool(JumpPeak, false);

            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, launchHeight);
        }
    }
}
