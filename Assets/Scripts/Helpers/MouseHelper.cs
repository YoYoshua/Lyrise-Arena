using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseHelper
{
    public static Vector3 GetMouseWorldPoint(Camera camera)
    {
        if(camera == null)
        {
            camera = Camera.main;
        }

        Vector3 mouseWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPoint.z = 0;
        return mouseWorldPoint;
    }
}
