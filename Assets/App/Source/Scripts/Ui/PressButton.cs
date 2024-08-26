using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PressButton : Button
{
    public UnityEvent Pressed;
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Pressed?.Invoke();
    }
}
