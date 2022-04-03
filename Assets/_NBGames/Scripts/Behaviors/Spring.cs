using System;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class Spring : MonoBehaviour
    {
        [SerializeField] private float _launchHeight = 30f;
        
        private Animator _animator;
        private static readonly int Decompress = Animator.StringToHash("Decompress");

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
            if (!other.CompareTag("Player Feet") || !_animator) return;
            
            SoundManager.instance.PlaySound(SoundEffect.Spring);
            _animator.SetBool(Decompress, true);
            EventManager.SpringLaunch(_launchHeight);
        }

        public void ResetSpring()
        {
            if (!_animator) return;
            _animator.SetBool(Decompress, false);
        }
    }
}
