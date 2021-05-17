using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField]
    private Canvas ParentCanvas;

    private void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentCanvas.transform as RectTransform, Input.mousePosition, ParentCanvas.worldCamera, out pos);
        transform.position = ParentCanvas.transform.TransformPoint(pos);
    }
}
