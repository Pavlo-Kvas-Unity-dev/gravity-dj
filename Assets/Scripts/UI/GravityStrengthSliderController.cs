using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace GravityDJ.UI
{
    [RequireComponent(typeof(Slider))]
    public class GravityStrengthSliderController : MonoBehaviour
    {
        [SerializeField] private LayoutElement zeroGravityRegionLayoutElement;
        [SerializeField] private RectTransform sliderBodyTransform;

        private Slider slider;

        [Inject] private GravityController gravityController;
        [Inject] private GravityController.Settings gravitySettings;

        private void Awake()
        {
            InitSlider();
        }

        public void Reset() 
        {
            slider.value = 0;
        }

        private void SetZeroThreshold(float zeroRegionSizeNorm)
        {
            StartCoroutine(SetZeroThresholdCoroutine(zeroRegionSizeNorm));
        }

        private IEnumerator SetZeroThresholdCoroutine(float zeroRegionSizeNorm)
        {
            //wait until layout group is updated
            yield return new WaitForEndOfFrame();

            var zeroRegionSize = sliderBodyTransform.rect.height * zeroRegionSizeNorm;
            
            zeroGravityRegionLayoutElement.minHeight = zeroGravityRegionLayoutElement.preferredHeight = zeroRegionSize;
        }

        private void InitSlider()
        {
            slider = GetComponent<Slider>();
            
            slider.onValueChanged.AddListener(gravityController.OnGravitySliderValueChanged);

            SetRange();

            SetZeroThreshold(gravitySettings.gravitySliderZeroThreshold);
        }

        private void SetRange()
        {
            slider.minValue = -1;
            slider.maxValue = 1;
        }

        public void UpdateValue(float newValue)
        {
            slider.value = newValue;
        }
    }
}
