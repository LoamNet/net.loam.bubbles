using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static DataPoint GetClosestPointOnLine(DataPoint start, DataPoint end, DataPoint bubble)
    {
        float xdif = end.X - start.X;
        float ydif = end.Y - start.Y;
        float line_len = Mathf.Sqrt(xdif * xdif + ydif * ydif);

        // Direction of line vector
        float directionX = xdif / line_len;
        float directionY = ydif / line_len;

        // get T val into line: Goes from 0 to 1.
        float t = directionX * (bubble.X - start.X) + directionY * (bubble.Y - start.Y);

        // get position of the closest point, p.
        float x = t * directionX + start.X;
        float y = t * directionY + start.Y;

        return new DataPoint(x, y);
    }

    public static bool IsLineTouchingCircle(DataPoint start, DataPoint end, DataPoint bubble, float radius)
    {
        // Get length of line
        float xdif = end.X - start.X;
        float ydif = end.Y - start.Y;
        float line_len = Mathf.Sqrt(xdif * xdif + ydif * ydif);

        // Get directon vector components
        float directionX = xdif / line_len;
        float directionY = ydif / line_len;

        // get T val into line: Goes from 0 to 1.
        float t = directionX * (bubble.X - start.X) + directionY * (bubble.Y - start.Y);

        // if t > 1 or < 0, we're not on the line.
        // At this point, we would be either behind or in front of it.
        if (t > line_len + radius || t < 0 - radius)
            return false;

        // get position of the closest point, p.
        float x = t * directionX + start.X;
        float y = t * directionY + start.Y;

        // Check if less than radius. Start by finding distance to line.
        xdif = x - bubble.X;
        ydif = y - bubble.Y;
        float circleDistance = Mathf.Sqrt(xdif * xdif + ydif * ydif);

        // Return if we hit/pass through the circle or not.
        if (circleDistance <= radius)
            return true;

        return false;
    }

    public static void SetMaterialToColorWithAlpha(GameObject obj, Color color)
    {
        Material surfMaterial = obj.GetComponent<Renderer>().material;
        surfMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        surfMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        surfMaterial.SetInt("_ZWrite", 0);
        surfMaterial.DisableKeyword("_ALPHATEST_ON");
        surfMaterial.EnableKeyword("_ALPHABLEND_ON");
        surfMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        surfMaterial.renderQueue = 3000;
        surfMaterial.SetColor("_Color", color);
    }
}
