using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
  private Player _player;
  private PlayerCam _playerCam;
  private PlayerInputSystem _playerInputSystem;
  private Vector2 _moveInput;
  private Vector2 _lookInput;

  private bool _jumpInput;
  private bool _sprintInput;

  private void Awake()
  {
    _player = gameObject.GetOrAddComponent<Player>();
    _playerCam = FindAnyObjectByType<PlayerCam>();
    _playerInputSystem = new PlayerInputSystem();
  }

  private void Update()
  {
    GetInpup();
    _player.Move(_moveInput);
    _player.Jump(_jumpInput);
    _player.Sprint(_sprintInput);
    _player.Rotate(_lookInput.x);
    _playerCam.Rotate(_lookInput.y);
  }

  private void OnEnable()
  {
    _playerInputSystem.Enable();
  }

  private void OnDisable()
  {
    _playerInputSystem.Disable();
  }
  private void GetInpup()
  {
    _moveInput = _playerInputSystem.Player.Move.ReadValue<Vector2>();
    _lookInput = _playerInputSystem.Player.Look.ReadValue<Vector2>();
    _jumpInput = _playerInputSystem.Player.Jump.WasPressedThisFrame();
    _sprintInput = _playerInputSystem.Player.Sprint.IsPressed();
  }
}
