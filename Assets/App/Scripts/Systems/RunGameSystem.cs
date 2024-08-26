using Bootstrapper.StateMachine;
using UnityEngine;

public class RunGameSystem : GameSystem
{
  [SerializeField] private Game game;
  private float time;

  internal override void Init()
  {
    game.LevelFailed += OnFailedLevel;
    game.LevelCompleted += OnLevelCompleted;
    game.GameStarted += OnGameStarted;
    game.transform.parent.gameObject.SetActive(false);
  }

  internal override void EnterState()
  {
    game.transform.parent.gameObject.SetActive(true);
    var level = GameData.Free = GameData.Current ?? Blueprint.levels[Random.Range(0, Blueprint.levels.Count)];
    game.StartGame(level.level);
    time = Time.time;
  }


  private void OnGameStarted()
  {
  }

  private void OnLevelCompleted()
  {
    game.transform.parent.gameObject.SetActive(false);
    GameData.Time = Time.time - time;
    GameData.Result = true;
    ToResult();
  }

  private void OnFailedLevel()
  {
    game.transform.parent.gameObject.SetActive(false);
    GameData.Time = Time.time - time;
    GameData.Result = false;
    ToResult();
  }
}