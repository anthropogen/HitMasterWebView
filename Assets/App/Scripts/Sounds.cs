using System;
using Bootstrapper;
using Bootstrapper.Data;
using UnityEngine;

public class Sounds : MonoBehaviour
{
  [SerializeField] private AudioSource audioSource;

  private static Sounds instance;
  public static Sounds Instance => instance;

  public void Init(PlayerData data)
  {
    audioSource.volume = data.volume;
    instance = this;
  }

  public void SetVolume(float volume)
  {
    audioSource.volume = volume;
    HyperBootstrapper.Instance.Save();
  }

  private void OnDestroy()
  {
    instance = null;
  }
}