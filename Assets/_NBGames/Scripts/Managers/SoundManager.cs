using System;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _NBGames.Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Sound Effect Configuration")]
        [SerializeField] private AudioClip _playerJump;
        [SerializeField] private AudioClip _playerHurt;
        [SerializeField] private AudioClip _coinSound;
        [SerializeField] private AudioClip _healthSound;
        [SerializeField] private AudioClip _enemyDeathSound;
        [SerializeField] private AudioClip _playerDeathSound;
        [SerializeField] private AudioClip _treasureChestSound;
        [SerializeField] private AudioClip _itemWooshSound;
        [SerializeField] private AudioClip _springSound;

        [Header("Stinger Configuration")] 
        [SerializeField] private AudioClip _levelWinStinger;
        
        public AudioClip PlayerJump => _playerJump;
        public AudioClip CoinSound => _coinSound;
        public AudioClip HealthSound => _healthSound;
        public AudioClip EnemyDeathSound => _enemyDeathSound;
        public AudioClip PlayerHurt => _playerHurt;
        public AudioClip LevelWinStinger => _levelWinStinger;
        public AudioClip PlayerDeathSound => _playerDeathSound;
        public AudioClip TreasureChestSound => _treasureChestSound;
        public AudioClip ItemWooshSound => _itemWooshSound;
        public AudioClip SpringSound => _springSound;

        private AudioClip _soundToPlay;
        private AudioSource _audioSource;

        public static SoundManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogWarning("SoundManager already exists! Destroying!");
                Destroy(this.gameObject);
            }

            CacheComponents();
        }

        private void CacheComponents()
        {
            _audioSource = GetComponent<AudioSource>();

            if (!_audioSource)
            {
                Debug.LogError($"AudioSource is null on {gameObject.name}");
            }
        }

        public void PlaySound(SoundEffect sound)
        {
            if (!_audioSource) return;

            switch (sound)
            {
                case SoundEffect.PlayerJump:
                    _soundToPlay = _playerJump;
                    break;
                
                case SoundEffect.Health:
                    _soundToPlay = _healthSound;
                    break;
                
                case SoundEffect.EnemyDeath:
                    _soundToPlay = _enemyDeathSound;
                    break;
                
                case SoundEffect.PlayerHurt:
                    _soundToPlay = _playerHurt;
                    break;
                
                case SoundEffect.PlayerDeath:
                    _soundToPlay = _playerDeathSound;
                    break;
                
                case SoundEffect.LevelWin:
                    _soundToPlay = _levelWinStinger;
                    break;
                
                case SoundEffect.TreasureChest:
                    _soundToPlay = _treasureChestSound;
                    break;
                
                case SoundEffect.Spring:
                    _soundToPlay = _springSound;
                    break;
                
                case SoundEffect.ItemWoosh:
                    _soundToPlay = _itemWooshSound;
                    break;

                default:
                    _soundToPlay = _coinSound;
                    break;
            }

            if (_soundToPlay == _coinSound || _soundToPlay == _healthSound)
            {
                _audioSource.clip = _soundToPlay;
                _audioSource.Play();
                _audioSource.volume = 0.6f;
            }
            else
            {
                _audioSource.volume = 1f;
                _audioSource.PlayOneShot(_soundToPlay);
            }
        }
    }
}
