using System;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _NBGames.Scripts.Pickups
{
    public class PickupBase : MonoBehaviour
    {
        [SerializeField] private SoundEffect _soundToPlay;
        [SerializeField] private bool _hasPhysics;
        
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;

        private void Awake()
        {
            CacheComponents();
        }

        private void OnEnable()
        {
            EventManager.onRespawnWorldObjects += Respawn;
        }

        private void OnDisable()
        {
            EventManager.onRespawnWorldObjects -= Respawn;
        }

        private void CacheComponents()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();

            if (!_spriteRenderer)
            {
                Debug.LogWarning($"SpriteRenderer is null on {gameObject.name}");
            }

            if (!_collider)
            {
                Debug.LogWarning($"Collider is null on {gameObject.name}");
            }
        }

        private void Respawn()
        {
            if (_spriteRenderer)
            {
                _spriteRenderer.enabled = true;
            }

            if (_collider)
            {
                _collider.enabled = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            SoundManager.instance.PlaySound(_soundToPlay);
            
            ProcessPickup();
        }

        protected virtual void ProcessPickup()
        {
            if (_hasPhysics)
            {
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                if (_spriteRenderer)
                {
                    _spriteRenderer.enabled = false;
                }

                if (_collider)
                {
                    _collider.enabled = false;
                }
            }
        }
    }
}
