using System.Linq;
using Bootstrapper;
using Bootstrapper.Data;
using Bootstrapper.StateMachine;

public class ResultSystem : GameSystem
{
  internal override void EnterState()
  {
    if (!GameData.Result)
    {
      return;
    }

    if (GameData.Current != null)
    {
      var dto = PlayerData.levels.FirstOrDefault(l => l.num == GameData.num);
      if (dto != null)
      {
      }
      else
      {
        PlayerData.levels.Add(new LevelDto
        {
          num = GameData.num,
        });
      }

      HyperBootstrapper.Instance.Save();
    }
  }
}