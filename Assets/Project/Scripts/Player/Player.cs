using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
  [Header("Player settings")]
  [SerializeField, DefaultValue(5f)] private float _defaultSpeed;
  [SerializeField, DefaultValue(10f)] private float _sprintSpeed;
  [SerializeField, DefaultValue(20f)] private float _rotateSpeed;
  [SerializeField, DefaultValue(5f)] private float _fallSpeed;
  [SerializeField, DefaultValue(1.5f)] private float _jumpHeight;
  [SerializeField, DefaultValue(10f)] private float _moveInterpolatoinFactor;
  [SerializeField, DefaultValue(10f)] private float _slopeSlideSpeed;

  private CharacterController _characterController;
  private Vector3 _move;
  private RaycastHit _raycastHit;

  private float _moveSpeed;
  private float _groundedAngle;

  private void Awake()
  {
    _characterController = gameObject.GetOrAddComponent<CharacterController>();
  }

  private void Update()
  {
    _groundedAngle = GetGroundAngle(out _raycastHit);
    Debug.Log(_groundedAngle);

    ApplyControllerMove(_move);
    Gravity();
    SlopeSlide();
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
      _move.y += Physics.gravity.y * _fallSpeed * Time.deltaTime;
    }
    else
      // -0.5f is for reducing fluctuations when character is on the ground
      // thats a fix for _characterController.isGrounded
      _move.y = -0.5f;
  }

  private float GetGroundAngle(out RaycastHit raycastHit)
  {
    Physics.SphereCast(transform.position, 0.3f, Vector3.down, out raycastHit, 5f);
    return Vector3.Angle(raycastHit.normal, Vector3.up);
  }

  private void SlopeSlide()
  {
    // InverseTransformDirection to make slide correct directions
    _raycastHit.normal = transform.InverseTransformDirection(_raycastHit.normal);

    if (_groundedAngle >= _characterController.slopeLimit && _characterController.isGrounded)
    {
      _move = Vector3.ProjectOnPlane(Vector3.up * (_move.y * _slopeSlideSpeed), _raycastHit.normal);
    }
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
      // This is formula for jump to a specific height "-2f" is part of formula
      // for jumping to a specific height
      _move.y = Mathf.Sqrt(_jumpHeight * -2f * (Physics.gravity.y * _fallSpeed));
    }
  }
}
