using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
  private PlayerInputSystem _playerInputSystem;
  private Animator _animator;

  private bool _isMovePressed;
  private bool _isRunPresed;
  private bool _isJumpPressed;

  private void Awake()
  {
    _animator = gameObject.GetComponent<Animator>();
    _playerInputSystem = new PlayerInputSystem();
  }

  private void Update()
  {
    GetInputs();
    MoveAnimation(_isMovePressed);
    RunAnimation(_isRunPresed);
    JumpAnimation(_isJumpPressed);
  }

  private void OnEnable()
  {
    _playerInputSystem.Enable();
  }

  private void OnDisable()
  {
    _playerInputSystem.Disable();
  }

  private void GetInputs()
  {
    _isMovePressed = _playerInputSystem.Player.Move.IsPressed();
    _isRunPresed = _playerInputSystem.Player.Sprint.IsPressed();
    _isJumpPressed = _playerInputSystem.Player.Jump.WasPressedThisFrame();
  }

  private void MoveAnimation(bool input) => _animator.SetBool("Walk", input);
  private void RunAnimation(bool input) => _animator.SetBool("Run", input);
  private void JumpAnimation(bool input) => _animator.SetBool("Jump", input);

}
