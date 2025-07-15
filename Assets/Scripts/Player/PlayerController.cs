using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
  [Header("Player speed")]
  [SerializeField] private float _playerSpeed = 0;
  private CharacterController _characterController;
  private Vector3 _playerInput = new();

  private void Start()
  {
    gameObject.AddComponent<CharacterController>();
    _characterController = GetComponent<CharacterController>();
  }

  private void Update()
  {
    _playerInput.x = GetInput<Vector2>("Move").x;
    _playerInput.z = GetInput<Vector2>("Move").y;
    _characterController.Move(_playerSpeed * Time.deltaTime * _playerInput);

    Debug.Log(GetInput<float>("Jump"));
  }

  private Vector2 OldInput()
  {
    return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
  }

  private T GetInput<T>(string actionName) where T : struct
  {
    return InputSystem.actions.FindAction(actionName).ReadValue<T>();
  }
}
