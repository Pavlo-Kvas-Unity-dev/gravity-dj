using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class GravityStrengthSliderController : MonoBehaviour
{
    private Slider slider;

    public LayoutElement zeroRegionLE;
    
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
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

    public void Reset()
    {
        slider.value = 0;
    }
}
