using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private int speedHash = Animator.StringToHash("Speed");

    public void SetSpeed(float speed)
        => animator.SetFloat(speedHash, speed);
}
