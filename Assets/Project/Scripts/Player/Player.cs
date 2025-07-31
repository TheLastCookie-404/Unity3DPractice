using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
  [Header("Player settings")]
  [SerializeField] private float _defaultSpeed = 5f;
  [SerializeField] private float _sprintSpeed = 10f;
  [SerializeField] private float _rotateSpeed = 30f;
  [SerializeField] private float _mass = 5f;
  [SerializeField] private float _jumpHeight = 1.5f;
  [SerializeField] private float _moveInterpolatoinFactor = 10f;

  private CharacterController _characterController;
  private Vector3 _move;
  private float _moveSpeed;

  private void Awake()
  {
    _characterController = gameObject.GetOrAddComponent<CharacterController>();
  }

  private void Update()
  {
    ApplyControllerMove(_move);
    Gravity();
  }

  private void ApplyControllerMove(Vector3 motion)
  {
    motion = transform.TransformDirection(motion); // Transform to local axis
    _characterController.Move(motion * Time.deltaTime);
  }

  private void Gravity()
  {
    if (!_characterController.isGrounded)
    {
      // Multyplied by Time.deltaTime twice, for downwards acceleration (gravity * mass * time^2)
      _move.y += Physics.gravity.y * _mass * Time.deltaTime;
    }
    else
      // -0.5f is for reducing fluctuations when character is on the ground
      // thats a fix for _characterController.isGrounded
      _move.y = -0.5f;
  }

  public void Move(Vector2 motionInput)
  {
    _move.x = Mathf.Lerp(_move.x, motionInput.x * _moveSpeed, _moveInterpolatoinFactor * Time.deltaTime);
    _move.z = Mathf.Lerp(_move.z, motionInput.y * _moveSpeed, _moveInterpolatoinFactor * Time.deltaTime);
  }

  public void Sprint(bool sprintInput)
  {
    _moveSpeed = sprintInput ? _sprintSpeed : _defaultSpeed;
  }

  public void Rotate(float rotateInput)
  {
    transform.Rotate(Vector3.up, Time.deltaTime * rotateInput * _rotateSpeed);
  }

  public void Jump(bool jumpInput)
  {
    if (jumpInput && _characterController.isGrounded)
    {
      // This is formula for jump to a specific height (* -2f) is part of formula
      _move.y = Mathf.Sqrt(_jumpHeight * -2f * (Physics.gravity.y * _mass));
    }
  }
}
