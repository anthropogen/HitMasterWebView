using System;
using UnityEngine;

public class Game : MonoBehaviour
{
  [SerializeField] private Player player;
  [SerializeField] private PlatformMover platformMover;
  [SerializeField] private Transform levelContainer;
  [SerializeField] private Transform startPos;
  private LevelConfig current;
  public event Action GameStarted;
  public event Action LevelFailed;
  public event Action LevelCompleted;

  private void OnEnable()
  {
    platformMover.PlayerReachedFinish += OnPlayerReachedFinish;
    player.Dead += OnPlayerDead;
  }

  private void OnDisable()
  {
    platformMover.PlayerReachedFinish -= OnPlayerReachedFinish;
    player.Dead -= OnPlayerDead;
  }

  public void StartGame(LevelConfig levelConfig)
  {
    player.transform.position = startPos.position;
    current = Instantiate(levelConfig, levelContainer);
    platformMover.Init(current, player);
    player.Gunner.enabled = true;
    player.gameObject.SetActive(true);
    GameStarted?.Invoke();
  }

  private void OnPlayerDead()
  {
    Destroy(current.gameObject);
    platformMover.EndGame();
    player.gameObject.SetActive(false);
    player.Gunner.enabled = false;
    LevelFailed?.Invoke();
  }

  private void OnPlayerReachedFinish()
  {
    Destroy(current.gameObject);
    platformMover.EndGame();
    player.gameObject.SetActive(false);
    player.Gunner.enabled = false;
    LevelCompleted?.Invoke();
  }
}