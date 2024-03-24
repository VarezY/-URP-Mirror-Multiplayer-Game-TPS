using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class PlayerEvents : MonoBehaviour
    {
        public new static PlayerEvents Instantiate { get; private set; }
        private void Awake()
        {
            if (Instantiate != null && Instantiate != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instantiate = this; 
            }
        }
        
        public event UnityAction OnPlayerGetHit;
        public event UnityAction<float> OnPlayerGetHit2;

        public virtual void PlayerGetHit()
        {
            OnPlayerGetHit?.Invoke();
        }


        public virtual void PlayerGetHit2(float arg0)
        {
            OnPlayerGetHit2?.Invoke(arg0);
        }
    }
}
