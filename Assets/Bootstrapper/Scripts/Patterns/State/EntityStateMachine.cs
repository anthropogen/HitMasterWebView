using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityStateMachine : GameEntity
{
  [SerializeField] private EntityState[] entityStates;
  [SerializeField] private EntityState firstState;
  private Dictionary<Type, EntityState> states;
  public EntityState CurrentState { get; private set; }

  private void Awake()
  {
    states = entityStates.ToDictionary(s => s.GetType());
    ChangeState(firstState);
  }

  public void Update()
  {
    if (CurrentState != null)
      CurrentState.Run();
  }

  public void ChangeState<T>() where T : EntityState
  {
    var next = states[typeof(T)];
    ChangeState(next);
  }

  public void ChangeState(EntityState next)
  {
    if (CurrentState != null)
      CurrentState.Exit();
    next.Enter();
    CurrentState = next;
  }
}