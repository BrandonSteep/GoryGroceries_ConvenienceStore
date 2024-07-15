using UnityEngine;
using UnityEngine.UI;

public class ScreenCursor : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private RectTransform cursorRectTransform;
    private Rect canvasRect;

    private void Awake()
    {
        cursorRectTransform = GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>().rect;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePosition, canvas.worldCamera, out canvasPosition);
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(canvasPosition.x, canvasRect.xMin, canvasRect.xMax),
            Mathf.Clamp(canvasPosition.y, canvasRect.yMin, canvasRect.yMax)
        );
        cursorRectTransform.localPosition = clampedPosition;
    }
}