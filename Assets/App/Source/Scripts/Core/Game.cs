using System;
using Cinemachine;
using UnityEngine;

public class Game : MonoBehaviour
{
  [SerializeField] private Player player;
  [SerializeField] private PlatformMover platformMover;
  [SerializeField] private Transform levelContainer;
  [SerializeField] private Transform startPos;
  [SerializeField] private CinemachineVirtualCamera cam;
  private LevelConfig current;
  private Vector3 camOffset = new Vector3(0, 4, -4);
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
    current = Instantiate(levelConfig, levelContainer);
    platformMover.Init(current, player);
    player.Warp(startPos.position);
    cam.OnTargetObjectWarped(player.transform, camOffset);
    cam.transform.position = player.transform.position + camOffset;
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
    player.Warp(startPos.position);
    cam.OnTargetObjectWarped(player.transform, camOffset);
    cam.transform.position = player.transform.position + camOffset;
    LevelFailed?.Invoke();
  }

  private void OnPlayerReachedFinish()
  {
    Destroy(current.gameObject);
    platformMover.EndGame();
    player.gameObject.SetActive(false);
    player.Gunner.enabled = false;
    player.Warp(startPos.position);
    cam.OnTargetObjectWarped(player.transform, camOffset);
    cam.transform.position = player.transform.position + camOffset;
    LevelCompleted?.Invoke();
  }
}