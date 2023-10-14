using UnityEngine;
using UnityEngine.Android;

public struct Strike
{
    public static Vector2Int Single
    {
        get
        {
            _single.x = 1;
            _single.y = 30;
            return _single;
        }
    }
    public static Vector2Int Double
    {
        get
        {
            _double.x = 3;
            _double.y = 00;
            return _double;
        }
    }
    private static Vector2Int _single;
    private static Vector2Int _double;
}