using System;
using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class ReparentPlayer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player Feet")) return;
            GameManager.instance.ChangePlayerParent(transform);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player Feet")) return;
            GameManager.instance.ClearPlayerParent();
        }
    }
}
