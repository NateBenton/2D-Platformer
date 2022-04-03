using System;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class PlayerFeet : MonoBehaviour
    {
        [Tooltip("The amount the player damages enemies on impact with feet.")]
        [SerializeField] private int _damageAmount = 1;
        
        private PlayerBehavior _player;
        private DamageableBehavior _damageable;
        
        private void Awake()
        {
            _player = GetComponentInParent<PlayerBehavior>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Damageable") || !_player) return;
            if (_player.Invincible) return;
            _damageable = other.GetComponent<DamageableBehavior>();

            _player.BounceOffEnemy();
            
            if (!_damageable) return;
            _damageable.Damage(_damageAmount);
        }
    }
}
