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
  [SerializeField] private float _moveInterpolatoinFactor = 10f;

  private CharacterController _characterController;
  private PlayerInputSystem _playerInputSystem;

  private Vector2 _moveInput;
  private Vector2 _lookInput;
  private bool _sprintInput;
  private bool _jumpInput;

  private Vector3 _playerMove;
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
    ApplyControllerMove(_playerMove);
    Gravity();
    GetInpup();

    Move(_moveInput);
    Jump(_jumpInput);
    Sprint(_sprintInput);
    Rotate(_lookInput.x);
  }

  private void ApplyControllerMove(Vector3 motion)
  {
    motion = transform.TransformDirection(motion); // Transform to local axis
    _characterController.Move(motion * Time.deltaTime);
  }

  public void Gravity()
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

  private void GetInpup()
  {
    _moveInput = _playerInputSystem.Player.Move.ReadValue<Vector2>() * _playerMoveSpeed;
    _lookInput = _playerInputSystem.Player.Look.ReadValue<Vector2>() * _playerRotateSpeed;
    _jumpInput = _playerInputSystem.Player.Jump.WasPressedThisFrame();
    _sprintInput = _playerInputSystem.Player.Sprint.IsPressed();
  }

  private void Move(Vector2 motionInput)
  {
    _playerMove.x = Mathf.Lerp(_playerMove.x, motionInput.x, _moveInterpolatoinFactor * Time.deltaTime);
    _playerMove.z = Mathf.Lerp(_playerMove.z, motionInput.y, _moveInterpolatoinFactor * Time.deltaTime);
  }

  private void Sprint(bool sprintInput)
  {
    _playerMoveSpeed = sprintInput ? _playerSprintSpeed : _playerDefaultSpeed;
  }

  private void Rotate(float rotateInput)
  {
    transform.Rotate(Vector3.up, Time.deltaTime * rotateInput);
  }

  private void Jump(bool jumpInput)
  {
    if (jumpInput && _characterController.isGrounded)
    {
      // This is formula for jump to a specific height (* -2f) is part of formula
      _playerMove.y = Mathf.Sqrt(_playerJumpHeight * -2f * (Physics.gravity.y * _playerMass));
    }
  }
}
