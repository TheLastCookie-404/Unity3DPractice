using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
  [Header("Player speed")]
  [SerializeField] private float _playerMoveSpeed = 0;
  [SerializeField] private float _playerRotateSpeed = 0;
  private CharacterController _characterController;
  private PlayerInputSystem _playerInputSystem;
  private InputAction _moveAction;
  private InputAction _lookAction;
  private InputAction _jumpAction;
  private Vector3 _playerMove;
  private float _playerRotate;

  private void Awake()
  {
    gameObject.AddComponent<CharacterController>();
    _characterController = GetComponent<CharacterController>();

    _playerInputSystem = new PlayerInputSystem();
    _moveAction = _playerInputSystem.FindAction("Move");
    _lookAction = _playerInputSystem.FindAction("Look");
    _jumpAction = _playerInputSystem.FindAction("Jump");
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
    PlayerMove();
    PlayerRotate();
    PlayerJump();
  }

  private void PlayerMove()
  {
    _playerMove.x = _moveAction.ReadValue<Vector2>().x;
    _playerMove.z = _moveAction.ReadValue<Vector2>().y;
    _playerMove = transform.TransformDirection(_playerMove); // Transform to local axis
    _characterController.Move(_playerMoveSpeed * Time.deltaTime * _playerMove);
  }

  private void PlayerRotate()
  {
    _playerRotate = _lookAction.ReadValue<Vector2>().x;
    transform.Rotate(Vector3.up, _playerRotateSpeed * Time.deltaTime * _playerRotate);
  }

  private void PlayerJump()
  {
    if (_jumpAction.WasPressedThisFrame())
      Debug.Log("Jump emulation");
  }
}
