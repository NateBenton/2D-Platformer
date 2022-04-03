using System;
using System.Collections;
using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class OscillationBehavior : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 3f;
        
        [Tooltip("Changes the X localScale when changing targets.")]
        [SerializeField] private bool _changeLocalScale;
        
        [Tooltip("Time to wait before moving to next point")]
        [SerializeField] private float _waitTime = 0.5f;
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;
        
        private bool _canMove;
        private Vector2 _target;

        private Vector2 _awakePosition;

        private void OnEnable()
        {
            EventManager.onRespawnWorldObjects += ResetPosition;
        }

        private void OnDisable()
        {
            EventManager.onRespawnWorldObjects -= ResetPosition;
        }

        private void Start()
        {
            _target = _start.position;
            _awakePosition = transform.position;
        }

        private void Update()
        {
            if (!_canMove) return;
            Movement();
            CheckPosition();
        }

        private void Movement()
        {
            transform.position = Vector2.MoveTowards
                (
                    transform.position,
                    _target,
                    _moveSpeed * Time.deltaTime
                );
        }

        private void CheckPosition()
        {
            if (transform.position == _start.position)
            {
                StartCoroutine(SwitchTarget(_end.position));
            }
            else if (transform.position == _end.position)
            {
                StartCoroutine(SwitchTarget(_start.position));
            }
        }

        private IEnumerator SwitchTarget(Vector2 nextPosition)
        {
            _canMove = false;
            
            yield return new WaitForSeconds(_waitTime);
            
            _target = nextPosition;
            _canMove = true;

            if (_changeLocalScale)
            {
                transform.localScale = new Vector2(-transform.localScale.x, 1f);
            }
        }

        private void OnBecameVisible()
        {
            _canMove = true;
        }

        private void ResetPosition()
        {
            _target = _start.position;
            transform.position = _awakePosition;

            if (_changeLocalScale)
            {
                transform.localScale = Vector2.one;
            }
        }
    }
}
