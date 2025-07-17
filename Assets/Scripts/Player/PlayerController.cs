using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
  [Header("Player speed")]
  [SerializeField] private float _playerMoveSpeed = 0;
  [SerializeField] private float _playerRotateSpeed = 0;
  private CharacterController _characterController;
  private Vector3 _playerMove;
  private float _playerRotate;

  private void Start()
  {
    gameObject.AddComponent<CharacterController>();
    _characterController = GetComponent<CharacterController>();
  }

  private void Update()
  {
    PlayerMove();
    PlayerRotate();
    Debug.Log(GetInput<float>("Jump"));
  }

  private T GetInput<T>(string actionName) where T : struct
  {
    return InputSystem.actions.FindAction(actionName).ReadValue<T>();
  }

  private void PlayerMove()
  {
    _playerMove.x = GetInput<Vector2>("Move").x;
    _playerMove.z = GetInput<Vector2>("Move").y;
    _playerMove = transform.TransformDirection(_playerMove); // Transform to local axis
    _characterController.Move(_playerMoveSpeed * Time.deltaTime * _playerMove);
  }

  private void PlayerRotate()
  {
    _playerRotate = GetInput<Vector2>("Look").x;
    transform.Rotate(Vector3.up, _playerRotateSpeed * Time.deltaTime * _playerRotate);
  }
}
