using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
  private PlayerInputSystem _playerInputSystem;
  private Animator _animator;
  private int _moveStateHash;

  private void Awake()
  {
    _animator = gameObject.GetComponent<Animator>();
    _playerInputSystem = new PlayerInputSystem();
    // _moveStateHash = Animator.StringToHash("Walk");
  }

  private void Update()
  {
    MoveAnimation();
    JumpAnimation();
  }

  private void OnEnable()
  {
    _playerInputSystem.Enable();
  }

  private void OnDisable()
  {
    _playerInputSystem.Disable();
  }

  private void MoveAnimation()
  {
    if (_playerInputSystem.Player.Move.WasPressedThisFrame())
    {
      _animator.SetBool("Walk", true);
    }
    else if (_playerInputSystem.Player.Move.WasReleasedThisFrame())
    {
      _animator.SetBool("Walk", false);
    }
  }

  private void JumpAnimation()
  {
    if (_playerInputSystem.Player.Jump.WasPressedThisFrame())
    {
      _animator.SetBool("Jump", true);
    }
    else if (_playerInputSystem.Player.Jump.WasReleasedThisFrame())
    {
      _animator.SetBool("Jump", false);
    }
  }
}
