using System;
using System.Collections;
using System.Collections.Generic;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Pickups;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _NBGames.Scripts.Behaviors
{
    public class TreasureChest : MonoBehaviour
    {
        [Tooltip("The treasure inside the chest.")]
        [SerializeField] private List<TreasureFields> _treasuresInside;
        
        [Tooltip("Allows the chest to respawn on player respawn.")]
        [SerializeField] private bool _refreshContents;
        
        [Tooltip("How high the item spawns from the chest.")]
        [SerializeField] private float _spawnHeightOffset = 0.3f;

        [Tooltip("The time between items flying out of the treasure chest.")]
        [SerializeField] private float _timeBetweenObjects = 0.1f;

        [SerializeField] private GameObject _buttonPrompt;

        private Animator _animator;
        private bool _canOpen, _opened;
        private static readonly int Open = Animator.StringToHash("Open");

        private Vector2 _spawnPoint;
        private GameObject _currentObject;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (!_animator)
            {
                Debug.LogWarning($"Animator is null on {gameObject.name}");
            }
        }

        private void OnEnable()
        {
            EventManager.onRespawnWorldObjects += RefreshTreasure;
        }

        private void OnDisable()
        {
            EventManager.onRespawnWorldObjects -= RefreshTreasure;
        }

        private void Start()
        {
            _spawnPoint = new Vector2(transform.position.x, transform.position.y - _spawnHeightOffset);
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E) || !_canOpen || _opened || !_animator) return;
            _animator.SetBool(Open, true);
            SoundManager.instance.PlaySound(SoundEffect.TreasureChest);

            _opened = true;
            _canOpen = false;
            
            _buttonPrompt.SetActive(false);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _canOpen = true;
            _buttonPrompt.SetActive(!_opened);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _canOpen = false;
            _buttonPrompt.SetActive(false);
        }

        public IEnumerator SpawnObject()
        {
            foreach (var item in _treasuresInside)
            {
                for (var i = 0; i < item.quantity; i++)
                {
                    SoundManager.instance.PlaySound(SoundEffect.ItemWoosh);
                    yield return new WaitForSeconds(_timeBetweenObjects);

                    _currentObject = PoolManager.instance.GetItem(item.treasure);
                    _currentObject.transform.position = _spawnPoint;
                }
            }
        }
        
        private void RefreshTreasure()
        {
            if (!_refreshContents || !_animator) return;

            _animator.SetBool(Open, false);
            _canOpen = false;
            _opened = false;
        }
    }
}
