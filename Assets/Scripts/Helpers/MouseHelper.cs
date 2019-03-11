using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseHelper
{
    /// <summary>
    /// Returns current mouse position in world coordinates.
    /// </summary>
    /// <param name="camera">Camera on which basis world coordinates are calculated, usually Camera.main</param>
    /// <returns></returns>
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
