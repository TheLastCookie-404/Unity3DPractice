using System.ComponentModel;
using UnityEngine;

public class FalloffPrevent : MonoBehaviour
{
  [SerializeField] private Vector3 _defaultPosition;
  [SerializeField, DefaultValue(-20f)] private float _fallOffPoint;
  private CharacterController _characterController;

  private void Awake()
  {
    _characterController = GetComponent<CharacterController>();
  }

  private void Update()
  {
    Teleport();
  }

  private void Teleport()
  {
    // CharacterController overrides transform.position
    // It need to be disabled, before

    if (transform.position.y <= _fallOffPoint)
    {
      _characterController.enabled = false;
      transform.position = _defaultPosition;
      _characterController.enabled = true;
    }
  }
}
