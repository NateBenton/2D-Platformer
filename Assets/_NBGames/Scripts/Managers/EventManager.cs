using System;
using UnityEngine;

namespace _NBGames.Scripts.Managers
{
    public class EventManager : MonoBehaviour
    {
        public static event Action<int> onIncreaseCoinCount;
        public static void IncreaseCoinCount(int amount)
        {
            onIncreaseCoinCount?.Invoke(amount);
        }

        public static event Action<int> onIncreasePlayerHealth;
        public static void IncreasePlayerHealth(int amount)
        {
            onIncreasePlayerHealth?.Invoke(amount);
        }

        public static event Action<int> onDamagePlayer;
        public static void DamagePlayer(int damageAmount)
        {
            onDamagePlayer?.Invoke(damageAmount);
        }

        public static event Action onRespawnWorldObjects;
        public static void RespawnWorldObjects()
        {
            onRespawnWorldObjects?.Invoke();
        }

        public static event Action<Vector2> onUpdateRespawnPosition;
        public static void UpdateRespawnPosition(Vector2 newPosition)
        {
            onUpdateRespawnPosition?.Invoke(newPosition);
        }

        public static event Action<int> onUnlockLevel;
        public static void UnlockLevel(int levelToUnlock)
        {
            onUnlockLevel?.Invoke(levelToUnlock);
        }

        public static event Action onLoadLevel;
        public static void LoadLevel()
        {
            onLoadLevel?.Invoke();
        }

        public static event Action<float> onSpringLaunch;
        public static void SpringLaunch(float launchHeight)
        {
            onSpringLaunch?.Invoke(launchHeight);
        }

        public static event Action onDeathZoneTouched;
        public static void DeathZoneTouched()
        {
            onDeathZoneTouched?.Invoke();
        }
    }
}
