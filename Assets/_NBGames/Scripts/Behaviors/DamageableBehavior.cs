using System;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class DamageableBehavior : MonoBehaviour
    {
        [Tooltip("The sprite renderer to be disabled on death.")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Tooltip("Optional colliders to disable on death.")]
        [SerializeField] private Collider2D[] _colliders;

        [Tooltip("Optional components to disable on death.")]
        [SerializeField] private MonoBehaviour[] _behaviorsToDisable;
        
        [SerializeField] private SoundEffect _soundOnDeath;

        private int Health { get; set; }
        private int _defaultHealth;

        private void OnEnable()
        {
            EventManager.onRespawnWorldObjects += Respawn;
        }

        private void OnDisable()
        {
            EventManager.onRespawnWorldObjects -= Respawn;
        }

        private void Awake()
        {
            if (!_spriteRenderer)
            {
                Debug.LogWarning($"SpriteRenderer is null on {gameObject.name}");
            }
        }

        private void Start()
        {
            _defaultHealth = Health;
        }

        public void Damage(int damageAmount)
        {
            Health = Mathf.Max(0, Health - damageAmount);

            if (Health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (!_spriteRenderer) return;
            
            _spriteRenderer.enabled = false;
            SoundManager.instance.PlaySound(_soundOnDeath);
            
            ToggleColliders(false);
            ToggleBehaviors(false);
        }

        private void ToggleColliders(bool state)
        {
            if (_colliders.Length == 0) return;
            
            foreach (var collider2d in _colliders)
            {
                collider2d.enabled = state;
            }
        }

        private void ToggleBehaviors(bool state)
        {
            if (_behaviorsToDisable.Length == 0) return;
            
            foreach (var behavior in _behaviorsToDisable)
            {
                behavior.enabled = state;
            }
        }

        private void ResetHealth()
        {
            Health = _defaultHealth;
        }
        
        private void Respawn()
        {
            Health = _defaultHealth;

            if (!_spriteRenderer) return;
            _spriteRenderer.enabled = true;

            ToggleColliders(true);
            ToggleBehaviors(true);
        }
    }
}
