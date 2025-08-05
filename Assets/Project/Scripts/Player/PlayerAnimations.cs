using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
  private PlayerInputSystem _playerInputSystem;
  private Animator _animatior;
  private int _moveStateHash;

  private void Awake()
  {
    _animatior = gameObject.GetComponent<Animator>();
    _playerInputSystem = new PlayerInputSystem();
    // _moveStateHash = Animator.StringToHash("Walk");
  }

  private void LateUpdate()
  {
    if (_playerInputSystem.Player.Move.IsPressed())
    {
      _animatior.SetBool("Walk", true);
    }
    else _animatior.SetBool("Walk", false);
  }

  void OnEnable()
  {
    _playerInputSystem.Enable();
  }

  void OnDisable()
  {
    _playerInputSystem.Disable();
  }
}
