using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class LevelConfig : MonoBehaviour
{
    [field: SerializeField] public Platform[] Platforms { get; private set; }

    public void Init(Player player)
    {
        foreach (var platform in Platforms)
        {
            platform.Init(player);
        }
    }
}
