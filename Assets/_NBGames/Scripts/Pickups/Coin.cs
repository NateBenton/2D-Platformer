using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Pickups
{
    public class Coin : PickupBase
    {
        [Tooltip("The amount to increase the players coins by.")]
        [SerializeField] private int _coinValue = 1;
        
        protected override void ProcessPickup()
        {
            EventManager.IncreaseCoinCount(_coinValue);
            base.ProcessPickup();
        }
    }
}
