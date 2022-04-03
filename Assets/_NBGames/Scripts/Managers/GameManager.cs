using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _NBGames.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Player Settings")]
        [Tooltip("The amount of lives the player starts with.")]
        [SerializeField] private int _startingLives = 3;

        [Tooltip("The amount of health the player starts with.")]
        [SerializeField] private int _startingHealth = 6;

        [Tooltip("The max health of the player.")]
        [SerializeField] private int _maxHealth = 6;

        [Header("Levels Unlocked Configuration")]
        [SerializeField] private bool[] _levelsUnlocked;

        private int _playerHealth = 6;
        private PlayerBehavior _player;
        
        public static GameManager instance;
        public int StartingHealth => _startingHealth;
        public int CoinCount { get; private set; }

        public int LivesCount { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogWarning("GameManager already exists! Destroying!");
                Destroy(this.gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.onIncreaseCoinCount += IncreaseCoins;
            EventManager.onIncreasePlayerHealth += IncreasePlayerHealth;
            EventManager.onDamagePlayer += DecreasePlayerHealth;
            EventManager.onUnlockLevel += UnlockLevel;
        }

        private void OnDisable()
        {
            EventManager.onIncreaseCoinCount -= IncreaseCoins;
            EventManager.onIncreasePlayerHealth -= IncreasePlayerHealth;
            EventManager.onDamagePlayer -= DecreasePlayerHealth;
            EventManager.onUnlockLevel -= UnlockLevel;
        }

        private void Start()
        {
            _playerHealth = _startingHealth;
            LivesCount = _startingLives;
        }

        private void IncreaseCoins(int amount)
        {
            CoinCount += amount;
            
            if (CoinCount >= 100)
            {
                CoinCount = (CoinCount - 100);
                LivesCount = Mathf.Min(LivesCount + 1, 99);
                
                UIManager.instance.UpdateLivesUI(LivesCount);
            }
            
            UIManager.instance.UpdateCoinUI(CoinCount);
        }

        private void IncreasePlayerHealth(int amount)
        {
            _playerHealth = Mathf.Min(_playerHealth + amount, _maxHealth);
            UIManager.instance.UpdateHealthUI(_playerHealth);
        }
        
        private void DecreasePlayerHealth(int damageAmount)
        {
            if (!_player) return;
            if (_player.Invincible) return;
            
            _playerHealth = Mathf.Max(0, _playerHealth - damageAmount);
            UIManager.instance.UpdateHealthUI(_playerHealth);

            if (_playerHealth != 0) return;
            PlayerDeath();
        }

        public void PlayerDeath()
        {
            if (LivesCount > 0)
            {
                StartCoroutine(_player.Die());
                LivesCount--;
            }
            else
            {
                StartCoroutine(_player.Die());
                StartCoroutine(GameOver());
            }
        }

        private IEnumerator GameOver()
        {
            yield return new WaitForSeconds(2f);
            LivesCount = _startingLives;
            _playerHealth = _startingHealth;
            CoinCount = 0;
            
            UIManager.instance.DisablePlayerStats();
            UIManager.instance.UpdateCoinUI(CoinCount);
            UIManager.instance.UpdateHealthUI(_playerHealth);
            UIManager.instance.UpdateLivesUI(LivesCount);

            SceneManager.LoadScene("GameOver");
        }

        public void DecreasePlayerLives(int amount = 1)
        {
            LivesCount -= amount;
            UIManager.instance.UpdateLivesUI(LivesCount);
        }

        public void ResetPlayerHealth()
        {
            _playerHealth = _maxHealth;
            UIManager.instance.UpdateHealthUI(_playerHealth);
            UIManager.instance.UpdateLivesUI(LivesCount);
        }

        public void CachePlayer(PlayerBehavior player)
        {
            _player = player;
        }

        private void UnlockLevel(int levelIndex)
        {
            if ((_levelsUnlocked.Length - 1) < levelIndex)
            {
                Debug.LogError("Level to unlock is out of bounds.");
                return;
            }
            
            _levelsUnlocked[levelIndex] = true;
            
            UIManager.instance.Fade(true);
        }

        public bool CheckIfLevelUnlocked(int levelToCheck)
        {
            if (_levelsUnlocked.Length - 1 >= levelToCheck) return _levelsUnlocked[levelToCheck];
            
            Debug.LogError($"Level supplied is out of range on {gameObject.name}");
            return false;
        }

        public void ChangePlayerParent(Transform newParent)
        {
            if (!_player) return;
            
            _player.transform.parent = newParent;
        }

        public void ClearPlayerParent()
        {
            if (!_player) return;
            _player.transform.parent = null;
        }
    }
}
