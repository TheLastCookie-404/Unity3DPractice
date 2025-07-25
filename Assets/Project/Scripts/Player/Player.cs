using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
  [Header("Player speed")]
  [SerializeField] private float _playerDefaultSpeed = 5f;
  [SerializeField] private float _playerSprintSpeed = 10f;
  [SerializeField] private float _playerRotateSpeed = 30f;
  [SerializeField] private float _playerMass = 5f;
  [SerializeField] private float _jumpHeight = 1.5f;

  private CharacterController _characterController;
  private PlayerInputSystem _playerInputSystem;
  private Vector2 _moveInput;
  private Vector2 _lookInput;
  private bool _sprintInput;
  private bool _jumpInput;
  private Vector3 _playerMove;
  private float _playerMoveSpeed;
  private float _playerRotate;

  private const float AccelerationSpeed = 1.2f;

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
    _moveInput = _playerInputSystem.Player.Move.ReadValue<Vector2>();
    _lookInput = _playerInputSystem.Player.Look.ReadValue<Vector2>();
    _jumpInput = _playerInputSystem.Player.Jump.WasPressedThisFrame();
    _sprintInput = _playerInputSystem.Player.Sprint.IsPressed();
  }

  private void ApplyMove()
  {
    _playerMove.x = _moveInput.x * _playerMoveSpeed;
    _playerMove.z = _moveInput.y * _playerMoveSpeed;

    _playerMove = transform.TransformDirection(_playerMove); // Transform to local axis
    _characterController.Move(_playerMove * Time.deltaTime);
  }

  private void ApplySprint()
  {
    _playerMoveSpeed = _sprintInput ? _playerSprintSpeed : _playerDefaultSpeed;
  }

  private void ApplyRotate()
  {
    _playerRotate = _lookInput.x;
    transform.Rotate(Vector3.up, _playerRotateSpeed * Time.deltaTime * _playerRotate);
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
      _playerMove.y = Mathf.Sqrt(_jumpHeight * -2f * (Physics.gravity.y * _playerMass));
    }
  }
}
