using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PersonView { FirstPerson, ThirdPerson };

public class PlayerCam : MonoBehaviour
{
  [Header("Обрезка угла наклона камеры")]
  [SerializeField, Tooltip("Максимальный угол поворота камеры"), Range(70, 90)] private float _maxAngle;
  [SerializeField, Tooltip("Минимальный угол поворота камеры"), Range(70, 90)] private float _minAngle;
  [SerializeField, Tooltip("Скорость вращения камеры"), DefaultValue(20)] private float _camRotationSpeed;
  [SerializeField, Tooltip("Положение камеры относительно персонажа")] private PersonView _view;
  [SerializeField] private Camera _firstPeronCamera;
  [SerializeField] private Camera _thirdPeronCamera;

  private PlayerInputSystem _playerInputSystem;
  private InputAction _lookAction;
  private Vector2 _rotateInput;

  private float _camRotation;

  private void Awake()
  {
    _playerInputSystem = new PlayerInputSystem();
    _lookAction = _playerInputSystem.FindAction("Look");

    SetCamPosition(_view, _firstPeronCamera, _thirdPeronCamera);
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
    _rotateInput = _lookAction.ReadValue<Vector2>();
  }

  public void Rotate(float inputValue)
  {
    _camRotation -= inputValue * _camRotationSpeed * Time.deltaTime;
    _camRotation = Mathf.Clamp(_camRotation, -_maxAngle, _minAngle);

    transform.localRotation = Quaternion.Euler(Vector3.right * _camRotation);
  }

  private void SetCamPosition(PersonView personView, Camera _firstPeronCamera, Camera _thirdPeronCamera)
  {
    switch (personView)
    {
      case PersonView.FirstPerson:
        _firstPeronCamera.enabled = true;
        _thirdPeronCamera.enabled = false;
        break;
      case PersonView.ThirdPerson:
        _firstPeronCamera.enabled = false;
        _thirdPeronCamera.enabled = true;
        break;
    }
  }
}
