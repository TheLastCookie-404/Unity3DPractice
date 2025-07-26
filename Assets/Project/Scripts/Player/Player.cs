using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
  [Header("Player speed")]
  [SerializeField] private float _playerDefaultSpeed = 5f;
  [SerializeField] private float _playerSprintSpeed = 10f;
  [SerializeField] private float _playerRotateSpeed = 30f;
  [SerializeField] private float _playerMass = 5f;
  [SerializeField] private float _playerJumpHeight = 1.5f;
  [SerializeField] private float _accelerationSpeed = 1.2f;

  private CharacterController _characterController;
  private PlayerInputSystem _playerInputSystem;
  private Vector2 _moveInput;
  private Vector2 _lookInput;
  private bool _sprintInput;
  private bool _jumpInput;
  private Vector3 _playerMove;
  private float _moveX;
  private float _moveZ;
  private float _playerMoveSpeed;

  private void Awake()
  {
    _characterController = gameObject.GetOrAddComponent<CharacterController>();
    _playerInputSystem = new PlayerInputSystem();
  }
  private void OnEnable()
  {
    _playerInputSystem.Enable();
  }

  private void OnDisable()
  {
    _playerInputSystem.Disable();
  }

  private void Update()
  {
    GetInpup();
    ApplyMove();
    ApplySprint();
    ApplyRotate();
    ApplyGravity();
    ApplyJump();
  }

  private void GetInpup()
  {
    _moveInput = _playerInputSystem.Player.Move.ReadValue<Vector2>() * _playerMoveSpeed;
    _lookInput = _playerInputSystem.Player.Look.ReadValue<Vector2>() * _playerRotateSpeed;
    _jumpInput = _playerInputSystem.Player.Jump.WasPressedThisFrame();
    _sprintInput = _playerInputSystem.Player.Sprint.IsPressed();
  }

  private void ApplyMove()
  {
    _moveX = Mathf.Lerp(_moveX, _moveInput.x, _accelerationSpeed * Time.deltaTime);
    _moveZ = Mathf.Lerp(_moveZ, _moveInput.y, _accelerationSpeed * Time.deltaTime);
    _playerMove.x = _moveX;
    _playerMove.z = _moveZ;

    _playerMove = transform.TransformDirection(_playerMove); // Transform to local axis
    _characterController.Move(_playerMove * Time.deltaTime);
  }

  private void ApplySprint()
  {
    _playerMoveSpeed = _sprintInput ? _playerSprintSpeed : _playerDefaultSpeed;
  }

  private void ApplyRotate()
  {
    transform.Rotate(Vector3.up, Time.deltaTime * _lookInput.x);
  }

  private void ApplyGravity()
  {
    if (!_characterController.isGrounded)
    {
      // Multyplied by Time.deltaTime twice, for downwards acceleration (gravity * mass * time^2)
      _playerMove.y += Physics.gravity.y * _playerMass * Time.deltaTime;
    }
    else
      // -0.5f is for reducing fluctuations when character is on the ground
      // thats a fix for _characterController.isGrounded
      _playerMove.y = -0.5f;
  }

  private void ApplyJump()
  {
    if (_jumpInput && _characterController.isGrounded)
    {
      // This is formula for jump to a specific height (* -2f) is part of formula
      _playerMove.y = Mathf.Sqrt(_playerJumpHeight * -2f * (Physics.gravity.y * _playerMass));
    }
  }
}
