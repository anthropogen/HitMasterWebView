using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform shootPosition;
    public Vector3 ShootPos => shootPosition.position;
}
