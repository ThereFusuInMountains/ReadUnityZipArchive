using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdaptive : MonoBehaviour
{
    public RectTransform image1;
    public RectTransform image2;
    public UIPosAdaptiveHanlder.RectTransfrom_HoverType type;
    public UIPosAdaptiveHanlder.Padding padding;

    private UIPosAdaptiveHanlder.RectTransfrom_HoverType oldType;
    private UIPosAdaptiveHanlder.Padding oldPadding;

    private void Update()
    {
        oldType = type;
        oldPadding = padding;
        image1.CalculateAdaptive(image2, type, oldPadding);
        //image2.ForceAreaBounds(image1);
    }
}
