using System;
using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class DamagePlayer : MonoBehaviour
    {
        [Tooltip("Damage applied to the player on collion.")]
        [SerializeField] private int _damageToGive = 1;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            EventManager.DamagePlayer(_damageToGive);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            EventManager.DamagePlayer(_damageToGive);
        }
    }
}
