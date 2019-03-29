using UnityEngine;
using UnityEngine.UI;

namespace GravityDJ.UI
{
    
    /// <summary>
    /// This layout element sets width and height to be equal to the height of the target transform
    /// </summary>
    public class SquareFitInParentLayoutElement : MonoBehaviour, ILayoutElement
    {
        [Tooltip("This layout element sets width and height to be equal to the height of the target transform")]
        [SerializeField] private RectTransform targetHeightTransform;
        
        public void CalculateLayoutInputHorizontal()
        {
            minHeight = preferredHeight = minWidth = preferredWidth = targetHeightTransform.rect.height;
            
            flexibleWidth = 0f;
        }

        public void CalculateLayoutInputVertical()
        {
        }

        public float minWidth { get; set; }
        public float preferredWidth { get; set; }
        public float flexibleWidth { get; set; }
        public float minHeight { get; set; }
        public float preferredHeight { get; set; }
        public float flexibleHeight { get; }
        public int layoutPriority { get; }
    }
}
