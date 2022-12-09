using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementOffset : MonoBehaviour
{
    public GameObject target;
    public RectTransform canvasRect;
    [SerializeField] float _offsetAmount;

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            return;
        }

        float offsetPosY = target.transform.position.y + _offsetAmount;

        // Final position of marker above GO in world space
        Vector3 offsetPos = new Vector3(target.transform.position.x, offsetPosY, target.transform.position.z);

        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);

        // Set
        transform.localPosition = canvasPos;
    }
}
