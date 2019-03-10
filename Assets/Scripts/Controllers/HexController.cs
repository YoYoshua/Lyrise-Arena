using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour
{
    [HideInInspector]
    public Hex HexObject { get; set; }

    private bool isHoverStateChanged = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        #region Hover
        Vector3 mousePosition = MouseHelper.GetMouseWorldPoint(Camera.main);
        Vector3 roundedHexPosition = HexHelper.WorldPositionToAxial(mousePosition, HexObject.Radius);

        if(roundedHexPosition == HexObject.AxialPosition)
        {
            SetHover(true);
            isHoverStateChanged = true;
        }
        else if(isHoverStateChanged && (roundedHexPosition != HexObject.AxialPosition))
        {
            SetHover(false);
            isHoverStateChanged = false;
        }
        #endregion
    }

    private void SetHover(bool hover)
    {
        var obj = this.transform.Find("hex").gameObject;
        obj.SetActive(!hover);

        obj = this.transform.Find("hex_hover").gameObject;
        obj.SetActive(hover);
    }
}
