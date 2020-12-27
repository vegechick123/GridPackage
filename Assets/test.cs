using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public _Color color1;
    public _Color color2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ColorMixing mixing = ColorMixing.instance;
        Debug.Log( mixing.AnalysisColor
           (mixing.MixColor(mixing.GetColor(color1), mixing.GetColor(color2))));
    }
}
