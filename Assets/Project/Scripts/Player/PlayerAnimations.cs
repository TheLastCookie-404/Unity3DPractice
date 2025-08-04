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
    _moveStateHash = Animator.StringToHash("Base Layer.Walk");
  }

  private void Update()
  {
    if (_playerInputSystem.Player.Move.IsPressed())
    {
      // _animatior.Play(_moveStateHash);
    }
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
