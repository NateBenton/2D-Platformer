using System;
using System.Collections;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _NBGames.Scripts.Behaviors
{
    public class LevelExit : MonoBehaviour
    {
        [SerializeField] private int levelToUnlock;
        private Animator _animator;
        
        private static readonly int Transitioning = Animator.StringToHash("Transitioning");
        private static readonly int Activated = Animator.StringToHash("Activated");

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
            if (!other.CompareTag("Player") || !_animator) return;
            _animator.SetBool(Transitioning, true);
        }

        public void ActivateLevelExit()
        {
            if (!_animator) return;
            _animator.SetBool(Activated, true);
            
            EventManager.UnlockLevel(levelToUnlock);
            StartCoroutine(ReturnToHub());
        }

        private IEnumerator ReturnToHub()
        {
            //SoundManager.instance.PlaySound(SoundEffect.LevelWin);
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene("WorldHub");
        }
    }
}
