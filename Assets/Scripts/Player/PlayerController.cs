using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
  [Header("Player speed")]
  [SerializeField] private float _playerDefaultSpeed = 5f;
  [SerializeField] private float _playerSprintSpeed = 10f;
  [SerializeField] private float _playerRotateSpeed = 30f;
  [SerializeField] private float _playerMass = 5f;
  [SerializeField] private float _jumpHeight = 1.5f;
  private CharacterController _characterController;
  private PlayerInputSystem _playerInputSystem;
  private InputAction _moveAction;
  private InputAction _lookAction;
  private InputAction _jumpAction;
  private InputAction _sprintAction;
  private Vector3 _playerMove;
  private float _playerMoveSpeed;
  private float _playerRotate;

  private void Awake()
  {
    _characterController = gameObject.GetOrAddComponent<CharacterController>();

    _playerInputSystem = new PlayerInputSystem();
    _moveAction = _playerInputSystem.FindAction("Move");
    _lookAction = _playerInputSystem.FindAction("Look");
    _jumpAction = _playerInputSystem.FindAction("Jump");
    _sprintAction = _playerInputSystem.FindAction("Sprint");
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
    ApplyMove();
    ApplySprint();
    ApplyRotate();
    ApplyGravity();
    ApplyJump();
  }

  private void ApplyMove()
  {
    Debug.Log(_playerMoveSpeed);

    _playerMove.x = _moveAction.ReadValue<Vector2>().x * _playerMoveSpeed;
    _playerMove.z = _moveAction.ReadValue<Vector2>().y * _playerMoveSpeed;

    _playerMove = transform.TransformDirection(_playerMove); // Transform to local axis
    _characterController.Move(_playerMove * Time.deltaTime);
  }

  private void ApplySprint()
  {
    _playerMoveSpeed = _sprintAction.IsPressed() ? _playerSprintSpeed : _playerDefaultSpeed;
  }

  private void ApplyRotate()
  {
    _playerRotate = _lookAction.ReadValue<Vector2>().x;
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
    if (_jumpAction.WasPressedThisFrame() && _characterController.isGrounded)
    {
      // This is formula for jump to a specific height (* -2f) is part of formula
      _playerMove.y = Mathf.Sqrt(_jumpHeight * -2f * (Physics.gravity.y * _playerMass));
    }
  }
}
