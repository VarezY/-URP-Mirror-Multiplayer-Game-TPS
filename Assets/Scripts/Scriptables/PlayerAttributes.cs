using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/PlayerAttributes", order = 0)]
    public class PlayerAttributes : ScriptableObject
    {
        [Header("Player Movement")]
        public float baseMoveSpeed;
        public float sprintSpeedMultiplier;
        public float walkSpeedMultiplier;
        public float jumpPower;

        [Header("Health and Stamina")] 
        public float baseHp;
        public float baseStamina;
        public float increaseRateStamina;
        public float decreaseRateStamina;
    }
}