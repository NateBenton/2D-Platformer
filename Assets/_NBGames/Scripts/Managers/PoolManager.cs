using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _NBGames.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _NBGames.Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        [Header("Prefab Configuration")]
        [SerializeField] private GameObject _physicsCoin;
        [SerializeField] private GameObject _bigCoin;
        [SerializeField] private GameObject _physicsHealth;

        [Header("Holder Configuration")] 
        [SerializeField] private Transform _coinHolder;
        [SerializeField] private Transform _healthHolder;
        [SerializeField] private Transform _bigCoinHolder;

        [Header("Item Pools")]
        [SerializeField] private List<GameObject> _coinPool = new List<GameObject>();
        [SerializeField] private List<GameObject> _healthPool = new List<GameObject>();
        [SerializeField] private List<GameObject> _bigCoinPool = new List<GameObject>();

        public static PoolManager instance;

        private List<GameObject> _itemList;
        private GameObject _item;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogWarning("PoolManager already exists! Destroying!");
                Destroy(this.gameObject);
            }
        }

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += DeactivatePools;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= DeactivatePools;
        }

        private void DeactivatePools(Scene oldScene, Scene currentScene)
        {
            foreach (var coin in _coinPool)
            {
                coin.SetActive(false);
            }

            foreach (var health in _healthPool)
            {
                health.SetActive(false);
            }

            foreach (var bigCoin in _bigCoinPool)
            {
                bigCoin.SetActive(false);
            }
        }

        public GameObject GetItem(TreasureItems itemType)
        {
            _itemList = itemType switch
            {
                TreasureItems.Coin => _coinPool,
                TreasureItems.Health => _healthPool,
                TreasureItems.BigCoin => _bigCoinPool,
                _ => _itemList
            };

            foreach (var item in _itemList.Where(item => !item.activeInHierarchy))
            {
                item.SetActive(true);
                return item;
            }

            CreateItem(itemType);
            return _item;
        }

        private void CreateItem(TreasureItems itemtype)
        {
            switch (itemtype)
            {
                case TreasureItems.Coin:
                    _item = Instantiate(_physicsCoin, _coinHolder);
                    _coinPool.Add(_item);
                    break;
                case TreasureItems.Health:
                    _item = Instantiate(_physicsHealth, _healthHolder);
                    _healthPool.Add(_item);
                    break;
            }
        }
    }
}
