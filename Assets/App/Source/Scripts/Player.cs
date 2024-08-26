using System;
using UnityEngine;

public class Player : MonoBehaviour
{
  [field: SerializeField] public PathMover PathMover { get; private set; }
  [field: SerializeField] public Gunner Gunner { get; private set; }
  public bool IsDead => isDead;

  private bool isDead;

  public event Action Dead;

  public void Death()
  {
    if (isDead) return;
    isDead = true;
    gameObject.SetActive(false);
    Dead?.Invoke();
  }

  public void Warp(Vector3 pos)
  {
    isDead = false;
    PathMover.Warp(pos);
  }
}