using UnityEngine;
using System.Collections.Generic;
using System;

public class Platform : MonoBehaviour
{
  [SerializeField] private Player player;
  [SerializeField] private List<Enemy> enemies;
  [field: SerializeField] public Path Path { get; private set; }
  [field: SerializeField] public PlatformType Type { get; private set; }
  public event Action<Platform> AllEnemiesDiedOnPlatform;
  public bool Clear => enemies.Count == 0;

  private void OnEnable()
  {
    foreach (var e in enemies)
    {
      e.Died += OnEnemyDied;
    }
  }

  private void OnDisable()
  {
    foreach (var e in enemies)
    {
      e.Died -= OnEnemyDied;
    }
  }

  private void OnEnemyDied(Enemy enemy)
  {
    if (!enemies.Contains(enemy))
      return;
    enemies.Remove(enemy);
    enemy.Died -= OnEnemyDied;
    if (enemies.Count == 0)
      AllEnemiesDiedOnPlatform?.Invoke(this);
  }

  public void OnPlayerReachedPlatform()
  {
    foreach (var enemy in enemies)
      enemy.StartMoveToPlayer(player);
  }

  public void Init(Player player)
  {
    this.player = player;
  }
}