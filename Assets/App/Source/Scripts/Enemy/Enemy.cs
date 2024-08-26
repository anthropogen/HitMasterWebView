using DG.Tweening;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField] private Animator animator;
  [SerializeField] private Renderer render;
  [SerializeField] private HealthBar healthBar;
  [SerializeField] private float speed = 1;
  [SerializeField, Range(1, 100)] private float maxHealth;
  public event Action<Enemy> Died;
  public Heatlh Heatlh { get; private set; }
  private Player player;
  private float startY;

  private void Awake()
  {
    Heatlh = new Heatlh(maxHealth);
    startY = transform.position.y;
  }

  private void OnEnable()
  {
    Heatlh.HealthChanged += healthBar.OnHealthChanged;
    Heatlh.Died += Death;
  }

  private void OnDisable()
  {
    Heatlh.HealthChanged -= healthBar.OnHealthChanged;
    Heatlh.Died -= Death;
  }

  private void Update()
  {
    if (player == null) return;
    if (player.IsDead) return;
    if (Heatlh.IsDead) return;
    if (transform.position.y < startY - 2)
      Heatlh.ApplyDamage(1000);

    var dir = transform.position - player.transform.position;
    if (dir.sqrMagnitude <= 1)
    {
      player.Death();
    }

    dir.y = 0;
    dir.Normalize();
    transform.Translate(dir * speed * Time.deltaTime);
  }

  public void StartMoveToPlayer(Player player)
  {
    this.player = player;
    animator.SetBool("IsWalk", true);
  }

  public void Death()
  {
    player = null;
    healthBar.gameObject.SetActive(false);
    animator.enabled = false;
    render.material.DOKill();
    render.material.DOColor(Color.grey, 0.5f);
    Died?.Invoke(this);
  }
}