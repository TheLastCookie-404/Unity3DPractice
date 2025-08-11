using UnityEngine;

public class FalloffPrevent : MonoBehaviour
{
  [SerializeField] private Vector3 _defaultPosition;
  [SerializeField] private float _fallOffPoint;

  private void Update()
  {
    if (transform.position.y <= _fallOffPoint)
    {
      Debug.Log("Falloff");
      transform.position = _defaultPosition;
    }
  }
}
