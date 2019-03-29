using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GravityDJ.UI
{
    [RequireComponent(typeof(Slider))]
    public class GravityStrengthSliderController : MonoBehaviour
    {
        private Slider slider;

        public LayoutElement zeroRegionLE;

        [Inject] private GravityController gravityController;
    
        public Slider Slider
        {
            get
            {
                if (slider == null)
                {
                    slider = GetComponent<Slider>();
                };
                return slider;
            }
        }

        private void Awake()
        {
            Slider.onValueChanged.AddListener(gravityController.OnStrengthChanged);
        }

        public void SetZeroThreshold(float zeroThreshold)
        {
            StartCoroutine(SetZeroThresholdCoroutine(zeroThreshold));
        }

        private IEnumerator SetZeroThresholdCoroutine(float zeroThreshold)
        {
            //wait until layout group is updated
            yield return new WaitForEndOfFrame();
            var parentHeight = ((RectTransform) (zeroRegionLE.transform.parent)).rect.height;

            zeroRegionLE.minHeight =
                zeroRegionLE.preferredHeight = parentHeight * zeroThreshold;
        }

        public void Reset() //todo rename
        {
            Slider.value = 0;
        }
    }
}
