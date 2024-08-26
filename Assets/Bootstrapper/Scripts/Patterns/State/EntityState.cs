using UnityEngine;

public abstract class EntityState : GameEntity
{
  [SerializeField] private EntityStateMachine stateMachine;
  public abstract void Enter();
  public abstract void Run();
  public abstract void Exit();
}