using System;
using UnityEngine;

[Serializable]
public class Heatlh
{
  private float maxHealth;
  private float current;
  public bool IsDead => current <= 0;
  public event Action<float> HealthChanged;
  public event Action Died;

  public Heatlh(float maxHealth)
  {
    if (maxHealth <= 0)
      throw new ArgumentException();
    this.maxHealth = maxHealth;
    current = maxHealth;
  }

  public void ApplyDamage(float damage)
  {
    damage = Mathf.Abs(damage);
    current = Mathf.Max(current - damage, 0);
    HealthChanged?.Invoke(current / maxHealth);
    if (current == 0)
      Died?.Invoke();
  }
}