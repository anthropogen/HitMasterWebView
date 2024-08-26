using System;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
  [SerializeField] private LevelConfig levelConfig;
  [SerializeField] private Game game;
  [SerializeField] private Player player;
  private Platform[] platforms;
  private int currentPlatform = 0;
  public event Action PlayerReachedFinish;


  private void OnEnable()
  {
    player.PathMover.FinishedPath += CheckPlatform;
    game.GameStarted += PlayerMoveToNextPlatform;
  }

  private void OnDisable()
  {
    player.PathMover.FinishedPath -= CheckPlatform;
    game.GameStarted -= PlayerMoveToNextPlatform;
  }

  public void Init(LevelConfig lvl, Player player)
  {
    this.player = player;
    levelConfig = lvl;
    platforms = levelConfig.Platforms;
    foreach (var p in platforms)
    {
      p.Init(player);
      p.AllEnemiesDiedOnPlatform += TryMoveToNextPlatform;
    }
  }

  public void EndGame()
  {
    foreach (var p in platforms)
    {
      p.AllEnemiesDiedOnPlatform -= TryMoveToNextPlatform;
    }

    levelConfig = null;
    platforms = null;
  }

  public Path GetPathToNextPlatform()
  {
    return platforms[currentPlatform].Path;
  }

  private void PlayerMoveToNextPlatform()
  {
    player.PathMover.Path = GetPathToNextPlatform();
  }

  private void TryMoveToNextPlatform(Platform platform)
  {
    if (platforms[currentPlatform] == platform)
      PlayerMoveToNextPlatform();
  }

  private void CheckPlatform()
  {
    currentPlatform++;
    var platform = platforms[this.currentPlatform];
    if (platform.Type == PlatformType.Finish)
    {
      PlayerReachedFinish?.Invoke();
      return;
    }

    if (platform.Clear)
    {
      PlayerMoveToNextPlatform();
    }
    else
    {
      platform.OnPlayerReachedPlatform();
    }
  }
}