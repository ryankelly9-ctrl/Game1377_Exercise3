using UnityEngine;

public class ScreenBounds
{
    private static float screenHalfHeight = Camera.main.orthographicSize;
    private static float screenHalfWidth = Camera.main.aspect * screenHalfHeight;

    public static float ScreenTop = screenHalfHeight;
    public static float ScreenBottom = -screenHalfHeight;
    public static float ScreenRight = screenHalfWidth;
    public static float ScreenLeft = -screenHalfWidth;
}
