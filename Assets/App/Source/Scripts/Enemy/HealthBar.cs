using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image foreground;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    public void OnHealthChanged(float fraction)
    {
        foreground.fillAmount = fraction;
    }
    private void Update()
    {
        transform.forward = cam.transform.forward;
    }
}
