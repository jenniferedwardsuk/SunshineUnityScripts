using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FluddUIController : MonoBehaviour {

    public FluddController Fludd;
    public Image FluddLeft;
    public Image FluddRight;
    public RawImage FluddTop;
    public RawImage FluddBottom;

    float FluddTopFullPosition;
    float FluddTopBottomPosition;
    Vector3 FluddTopNewPosition;

    // Use this for initialization
    void Start ()
    {
        FluddTopFullPosition = FluddTop.rectTransform.position.y;
        FluddTopBottomPosition = FluddBottom.rectTransform.position.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        FluddLeft.fillAmount = (Fludd.WaterLevel / Fludd.MaxWaterLevel);

        FluddRight.fillAmount = (Fludd.WaterLevel / Fludd.MaxWaterLevel);

        FluddTopNewPosition = FluddTop.rectTransform.position;
        FluddTopNewPosition.y = FluddTopBottomPosition + (Fludd.WaterLevel / Fludd.MaxWaterLevel) * (FluddTopFullPosition - FluddTopBottomPosition);
        FluddTop.rectTransform.position = FluddTopNewPosition;
    }
}
