using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    //当前使用的UI编号
    [SerializeField]
    private int showIndex;
    public int ShowIndex
    {
        get => showIndex;
        set
        {
            if (value != showIndex)
            {
                showIndex = value;
                UpdateUI();
            }
        }
    }
    private void Awake()
    {
        instance = this;
        showIndex = -1;
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == ShowIndex)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void OnValidate()
    {
        UpdateUI();
    }
}
