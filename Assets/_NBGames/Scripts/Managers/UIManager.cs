using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _NBGames.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("General Configuration")]
        [SerializeField] private CanvasGroup _playerStatsCanvas;
        
        [Header("Heart Configuration")]
        [SerializeField] private Sprite[] _heartSprites;
        [SerializeField] private Image[] _heartUIImages;

        [Header("Coin Configuration")] 
        [SerializeField] private TextMeshProUGUI _coinText;

        [Header("Lives Configuration")] 
        [SerializeField] private TextMeshProUGUI _livesText;

        public static UIManager instance;
        private int _healthRemainder;
        private Animator _animator;

        private static readonly int FadeOut = Animator.StringToHash("FadeOut");
        private static readonly int FadeIn = Animator.StringToHash("FadeIn");

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogWarning("UIManager already exists! Destroying!");
                Destroy(this.gameObject);
            }

            CacheComponents();
        }

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += FadeInLevel;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= FadeInLevel;
        }

        private void CacheComponents()
        {
            _animator = GetComponent<Animator>();

            if (!_animator)
            {
                Debug.LogError($"Animator is null on {gameObject.name}");
            }
        }

        private void Start()
        {
            UpdateHealthUI(GameManager.instance.StartingHealth);
            UpdateCoinUI(GameManager.instance.CoinCount);
            UpdateLivesUI(GameManager.instance.LivesCount);
        }
        
        public void UpdateHealthUI(int health)
        {
            for (var i = 0; i < _heartUIImages.Length; i++)
            {
                _healthRemainder = Mathf.Clamp(health - (i * 2), 0, 2);

                _heartUIImages[i].sprite = _healthRemainder switch
                {
                    0 => _heartSprites[2],
                    1 => _heartSprites[1],
                    2 => _heartSprites[0],
                    _ => _heartUIImages[i].sprite
                };
            }
        }

        public void UpdateCoinUI(int coinAmount)
        {
            _coinText.text = $"x{coinAmount}";
        }

        public void UpdateLivesUI(int livesAmount)
        {
            _livesText.text = $"x{livesAmount}";
        }

        public void Fade(bool fadeOut)
        {
            if (!_animator) return;
            _animator.SetTrigger(fadeOut ? FadeOut : FadeIn);
        }
        
        private void FadeInLevel(Scene oldScene, Scene currentScene)
        {
            Fade(false);
        }

        private void EnablePlayerStats()
        {
            if (!_playerStatsCanvas) return;
            _playerStatsCanvas.alpha = 1f;
        }

        public void DisablePlayerStats()
        {
            if (!_playerStatsCanvas) return;
            _playerStatsCanvas.alpha = 0f;
        }

        public IEnumerator NewGameFade()
        {
            Fade(true);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("WorldHub");
            EnablePlayerStats();
        }
    }
}
