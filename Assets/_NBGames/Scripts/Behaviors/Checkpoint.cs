using System;
using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class Checkpoint : MonoBehaviour
    {
        private bool _activated;
        private Animator _animator;
        private static readonly int Activated = Animator.StringToHash("Activated");
        private static readonly int Transitioning = Animator.StringToHash("Transitioning");

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (!_animator)
            {
                Debug.LogWarning($"Animator is null on {gameObject.name}");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_activated) return;
            
            if (!other.tag.Contains("Player")) return;
            EventManager.UpdateRespawnPosition(transform.position);
            
            if (!_animator) return;
            _animator.SetBool(Transitioning, true);
        }

        public void ActivateCheckpoint()
        {
            if (!_animator) return;
            _animator.SetBool(Activated, true);
        }
    }
}
