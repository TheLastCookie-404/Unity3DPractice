using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
  [Header("Обрезка угла наклона камеры")]
  [SerializeField, Tooltip("Максимальный угол поворота камеры"), Range(70, 90)] private int _maxAngle;
  [SerializeField, Tooltip("Минимальный угол поворота камеры"), Range(70, 90)] private int _minAngle;
  private float _camRotation;
  private void Start()
  {

  }

  private void Update()
  {
    _camRotation = GetInput<Vector2>("Look").y;
    // Debug.Log(transform.rotation.x);
    CamRotate(_camRotation);
  }

  private T GetInput<T>(string actionName) where T : struct
  {
    return InputSystem.actions.FindAction(actionName).ReadValue<T>();
  }

  // private void CamRotate(float inputValue) => transform.localRotation = Quaternion.Euler(CamCropAngle(inputValue), 0f, 0f);
  private void CamRotate(float inputValue)
  {
    transform.Rotate(Vector3.left, inputValue * CamCropAngle(-0.9f, 0.9f));
  }

  // private float CamCropAngle(float valueToCrop) // Обрезка наклона камеры до максимального и минимального 
  // {
  //   _camRotation -= valueToCrop;
  //   _camRotation = Mathf.Clamp(_camRotation, -_maxAngle, _minAngle);
  //   return _camRotation;
  // }

  private int CamCropAngle(float min, float max) // Обрезка наклона камеры до максимального и минимального 
  {
    if (transform.rotation.x >= max || transform.rotation.x <= min) return 0;
    return 1;
  }

}
