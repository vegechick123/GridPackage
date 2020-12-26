using System;
using System.Collections.Generic;
using UnityEngine;

public enum _Color : byte
{
    red,
    blue,
    yellow,
    orange,
    green,
    purple,
    orgin
}
public class ColorMixing : MonoBehaviour
{
    public static ColorMixing instance;
    public class MyColor
    {
        public MyColor(_Color color)
        {
            Color = color;
        }
        public _Color Color;
        public static _Color operator +(MyColor c1, MyColor c2)
        {
            if (c1.Color == c2.Color)
                return c1.Color;
            else if (c1.Color == _Color.orgin)
            {
                return c2.Color;
            }
            else if (c2.Color == _Color.orgin)
            {
                return _Color.orgin;
            }
            else if ((c1.Color == _Color.blue && c2.Color == _Color.yellow)
               || (c2.Color == _Color.blue && c1.Color == _Color.yellow))
            {
                return _Color.green;
            }
            else if ((c1.Color == _Color.blue && c2.Color == _Color.red)
               || (c2.Color == _Color.blue && c1.Color == _Color.red))
            {
                return _Color.purple;
            }
            else if ((c1.Color == _Color.yellow && c2.Color == _Color.red)
               || (c2.Color == _Color.yellow && c1.Color == _Color.red))
            {
                return _Color.orange;
            }
            else
            {
                return c2.Color;
            }
        }
    }

    Dictionary<byte, Func<Color>> dic;
    private void Awake()
    {
        instance = this;
        dic = new Dictionary<byte, Func<Color>>()
        {
            {(byte)_Color.blue,()=>{return new Color(0.155f,0.628f,1,1); } },
            {(byte)_Color.green,()=>{return Color.green; } },
            {(byte)_Color.orange,()=>{return new Color(1,0.530f,0,1); } },
            {(byte)_Color.purple,()=>{return new Color(0.918f,0.514f,1,1); } },
            {(byte)_Color.red,()=>{return new Color(0.905f,0.328f,0.328f,1); } },
            {(byte)_Color.yellow,()=>{return Color.yellow; } },
            {(byte)_Color.orgin,()=>{return new Color(1,1,1,1); } },
        };
    }
    public Color MixColor(Color color1, Color color2)
    {
        _Color c1 = AnalysisColor(color1);
        _Color c2 = AnalysisColor(color2);
        MyColor m1 = new MyColor(c1);
        MyColor m2 =new MyColor(c2);
        _Color result = m1 + m2;
        if (result == _Color.orgin)
            return color1;
        else
            return GetColor(result);
        //    if (c2.Equals(_Color.orgin))
        //        return color1;
        //    if (c1.Equals(_Color.orgin))
        //        return color2;
        //    if (TwoColorMixing(c1, c2, _Color.blue, _Color.yellow, _Color.green) != new Color())
        //        return
        //}
        //public Color TwoColorMixing(_Color c1, _Color c2, _Color color1, _Color color2, _Color result)
        //{
        //    if ((c1.Equals(color1) && c2.Equals(color2))
        //|| (c2.Equals(color1) && c1.Equals(color2))
        //)
        //        return GetColor(result);
        //    else
        //        return new Color();
        //}
    }
    public _Color AnalysisColor(Color color)
    {
        foreach (var myEnum in Enum.GetValues(typeof(_Color)))
        {
            if ((_Color)myEnum != _Color.orgin)
            {
                if (color == GetColor((_Color)myEnum))
                {
                    return (_Color)myEnum;
                }
            }
        }
        return _Color.orgin;
    }
    public Color GetColor(_Color color)
    {
        dic.TryGetValue((byte)color, out Func<Color> func);
        return func.Invoke();
    }
}

