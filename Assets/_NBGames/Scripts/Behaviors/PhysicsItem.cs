using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _NBGames.Scripts.Behaviors
{
    public class PhysicsItem : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            if (!_rigidbody2D)
            {
                Debug.LogWarning($"Rigidbody2D is null on {gameObject.name}");
            }
        }

        private void OnEnable()
        {
            VelocityBoost();
        }

        private void VelocityBoost()
        {
            if (!_rigidbody2D) return;
            _rigidbody2D.velocity = new Vector2(Random.Range(15f, -15f), Random.Range(10f, 11f));
        }
    }
}