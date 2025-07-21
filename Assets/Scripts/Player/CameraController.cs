using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
  [Header("Обрезка угла наклона камеры")]
  [SerializeField, Tooltip("Максимальный угол поворота камеры"), Range(70, 90)] private float _maxAngle;
  [SerializeField, Tooltip("Минимальный угол поворота камеры"), Range(70, 90)] private float _minAngle;
  [SerializeField, Tooltip("Минимальный угол поворота камеры")] private float _camRotationSpeed;
  private PlayerInputSystem _playerInputSystem;
  private InputAction _lookAction;
  private Vector2 _rotateInput;
  private float _camRotation;

  private void Awake()
  {
    _playerInputSystem = new PlayerInputSystem();

    _lookAction = _playerInputSystem.FindAction("Look");
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
    _rotateInput = _lookAction.ReadValue<Vector2>() * _camRotationSpeed * Time.deltaTime;
    CamRotate(_rotateInput.y, _maxAngle, _minAngle);
  }

  private void CamRotate(float inputValue, float maxAngle, float minAngle)
  {
    _camRotation -= inputValue;
    _camRotation = Mathf.Clamp(_camRotation, -maxAngle, minAngle);

    transform.localRotation = Quaternion.Euler(_camRotation, 0f, 0f);
  }
}
