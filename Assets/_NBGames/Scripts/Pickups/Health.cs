using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Pickups
{
    public class Health : PickupBase
    {
        [Tooltip("The amount to increase the player health by.")]
        [SerializeField] private int _healthAmount = 1;

        protected override void ProcessPickup()
        {
            EventManager.IncreasePlayerHealth(_healthAmount);
            base.ProcessPickup();
        }
    }
}
