using System;
using Cinemachine;
using Events;
using Mirror;
using Scriptables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    [RequireComponent(typeof(NetworkTransformReliable)), RequireComponent(typeof(MovementController))]
    public class CharacterManager : NetworkBehaviour
    {
        public enum PlayerState
        {
            Idle,
            Move,
            Jump,
            Airborne,
            Sprint,
            Aim,
            Action,
        }

        [Header("Player State")]
        [ReadOnly] public PlayerState currentState;
        
        [Space]
        [SerializeField] private PlayerAttributes playerData;
        
        [Header("Player Movement")]
        [SerializeField] private MovementController movement;
        [SerializeField] private float gravityPower;
        [SerializeField] private float moveSmoothDamp;

        [Header("Animation")] 
        [SerializeField] private Animator animator;
        [SerializeField] private NetworkAnimator networkAnimator;

        [Header("Action")] [SerializeField] private GameObject bulletPrefab;
        [Header("Camera")] [SerializeField] private CinemachineFreeLook virtualCamera;
        
        // Public Events
        
        // Private Variables
        private bool _isSprinting;
        private float _hp;
        

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            currentState = PlayerState.Idle;
            _isSprinting = false;

            InitMovement();

            _hp = playerData.baseHp;
            
            InitCamera();
        }

        private void InitMovement()
        {
            movement.baseMoveSpeed = playerData.baseMoveSpeed;
            movement.walkSpeedMultiplier = playerData.walkSpeedMultiplier;
            movement.sprintSpeedMultiplier = playerData.sprintSpeedMultiplier;
            movement.jumpPower = playerData.jumpPower;
            movement.gravity = gravityPower;
            if (Camera.main is not null) movement.cam = Camera.main.transform;
            movement.turnSmoothTime = moveSmoothDamp;
        }

        private void InitCamera()
        {
            virtualCamera = FindObjectOfType<CinemachineFreeLook>();
            var playerTransform = transform;
            virtualCamera.Follow = playerTransform;
            virtualCamera.LookAt = playerTransform;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            
            PlayerUpdateState();
            
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            ChangeState(_isSprinting ? PlayerState.Sprint : PlayerState.Move);

            if (context.canceled)
            {
                ChangeState(PlayerState.Idle);
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _isSprinting = true;

            if (context.canceled)
            {
                _isSprinting = false;
            }
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // PlayerEvents.Instantiate.PlayerGetHit();
                _hp -= 10f;
                float hpPercentage = _hp / playerData.baseHp;
                PlayerEvents.Instantiate.PlayerGetHit2(hpPercentage);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            
        }
        
        [Command(requiresAuthority = false)]
        public void OnAction(InputAction.CallbackContext context)
        {
            ChangeState(PlayerState.Action);
            GameObject item = Instantiate(bulletPrefab);
            NetworkServer.Spawn(item);
        }

        private void ChangeState(PlayerState state)
        {
            if (currentState == state) return;

            PlayerExitState();
            currentState = state;
            PlayerEnterState();
        }

        private void PlayerEnterState()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    TriggerAnimation("Idle");
                    break;
                case PlayerState.Move:
                    TriggerAnimation("Walk");
                    break;
                case PlayerState.Sprint:
                    TriggerAnimation("Run");
                    break;
                case PlayerState.Jump:
                    break;
                case PlayerState.Aim:
                    break;
            }
        }

        private void PlayerUpdateState()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    break;
                case PlayerState.Move:
                    if (_isSprinting)
                    {
                        ChangeState(PlayerState.Sprint);
                    }
                    break;
                case PlayerState.Sprint:
                    if (!_isSprinting)
                    {
                        ChangeState(PlayerState.Move);
                    }
                    break;
                case PlayerState.Jump:
                    break;
                case PlayerState.Aim:
                    break;
                case PlayerState.Action:
                    break;
            }
        }
        
        private void PlayerExitState()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    break;
                case PlayerState.Move:
                    break;
                case PlayerState.Sprint:
                    break;
                case PlayerState.Jump:
                    break;
                case PlayerState.Aim:
                    break;
                case PlayerState.Action:
                    break;
            }
        }

        private void TriggerAnimation(string triggerName)
        {
            animator.SetTrigger(triggerName);
            networkAnimator.SetTrigger(triggerName);
        }
    }
}
