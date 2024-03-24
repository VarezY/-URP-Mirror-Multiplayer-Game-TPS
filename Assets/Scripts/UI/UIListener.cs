using System;
using Events;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIListener : NetworkBehaviour
    {
        [SerializeField] private Slider healthSlider;

        public override void OnStartClient()
        {
            PlayerEvents.Instantiate.OnPlayerGetHit2 += InstantiateOnPlayerGetHit;
            healthSlider.value = 1f;
        }

        private void InstantiateOnPlayerGetHit(float args)
        {
            healthSlider.value = args;
        }
    }
}
