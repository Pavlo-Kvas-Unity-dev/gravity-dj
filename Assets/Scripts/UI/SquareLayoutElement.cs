using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareLayoutElement : MonoBehaviour, ILayoutElement
{
    


    public void CalculateLayoutInputHorizontal()
    {
        var parentRectTransform = transform.parent as RectTransform;
        minWidth = preferredWidth = parentRectTransform.rect.height;
        flexibleWidth = 0f;
    }

    public void CalculateLayoutInputVertical()
    {
         
    }

    public float minWidth { get; set; }
    public float preferredWidth { get; set; }
    public float flexibleWidth { get; set; }
    public float minHeight { get; }
    public float preferredHeight { get; }
    public float flexibleHeight { get; }
    public int layoutPriority { get; }
}
