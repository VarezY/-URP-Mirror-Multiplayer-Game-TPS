using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class MovementController : NetworkBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        private Vector2 _moveDir;
        private Vector3 _direction;
        private CharacterController _controller;

        [Header("Stats")]
        [ReadOnly] public float baseMoveSpeed;
        [ReadOnly] public float walkSpeedMultiplier;
        [ReadOnly] public float sprintSpeedMultiplier;
        [ReadOnly] public float jumpPower;
        [ReadOnly] public float gravity;
        [ReadOnly] public float turnSmoothTime;
        
        [Space]
        [HideInInspector] public Transform cam;
        
        private float _currentMoveSpeed;
        private float _velocity;
        private float _turnSmoothVelocity;

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
            MoveCharacter();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveDir = context.ReadValue<Vector2>();

            _direction = new Vector3(_moveDir.x, 0f, _moveDir.y).normalized;
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
        
        private void MoveCharacter()
        {
            if (_direction.magnitude >= .1f)
            {
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                moveDir = new Vector3(moveDir.x, _velocity, moveDir.z);
                _controller.Move(moveDir.normalized * (Time.deltaTime * _currentMoveSpeed));
            }
            
            // _controller.Move(new Vector3(_moveDir.x, _velocity, _moveDir.y) * (Time.deltaTime * _currentMoveSpeed));
        }
    }
}
