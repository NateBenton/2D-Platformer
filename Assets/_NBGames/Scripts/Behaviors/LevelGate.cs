using System;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class LevelGate : MonoBehaviour
    {
        [Tooltip("The index of the level in the GameManager.")]
        [SerializeField] private int _levelIndex;
        [SerializeField] private GateEnterZone _gateEnterZone;

        private Animator _animator;
        private static readonly int Opening = Animator.StringToHash("Opening");

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
            if (!other.tag.Contains("Player")) return;

            if (!GameManager.instance.CheckIfLevelUnlocked(_levelIndex)) return;
            _animator.SetBool(Opening, true);
            _gateEnterZone.enabled = true;
        }
    }
}
