using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class MovementController : NetworkBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        private Vector2 _moveDir;
        private CharacterController _controller;

        [ReadOnly] public float baseMoveSpeed;
        [ReadOnly] public float walkSpeedMultiplier;
        [ReadOnly] public float sprintSpeedMultiplier;
        [ReadOnly] public float jumpPower;
        [ReadOnly] public float gravity;

        private float _currentMoveSpeed;
        private float _velocity;

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            playerInput.enabled = true;
            _currentMoveSpeed = baseMoveSpeed;
        }

        public override void OnStartLocalPlayer()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            
            ApplyGravity();

            _controller.Move(new Vector3(_moveDir.x, _velocity, _moveDir.y) * (Time.deltaTime * _currentMoveSpeed));

        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveDir = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _currentMoveSpeed = baseMoveSpeed * sprintSpeedMultiplier;
            
            if (context.canceled)
            {
                _currentMoveSpeed = baseMoveSpeed;
            }
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            _currentMoveSpeed = baseMoveSpeed * walkSpeedMultiplier;
            
            if (context.canceled)
            {
                _currentMoveSpeed = baseMoveSpeed;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            
        }
        
        private void ApplyGravity()
        {
            if (_controller.isGrounded && _velocity < 0.0f)
            {
                _velocity = -1.0f;
            }
            else
            {
                _velocity += gravity * Time.deltaTime;
            }
        }
    }
}
