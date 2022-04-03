using System;
using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            EventManager.DeathZoneTouched();
        }
    }
}
