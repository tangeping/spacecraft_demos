using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : HpProgressBar
{

    // Use this for initialization

    // Use this for initialization
    public Image _image = null;

    void Start ()
    {
        _image = GetComponent<Image>();
        if (_image == null)
        {
            Debug.LogError("HpBar _image not be found.");
        }

        SetPercent();
    }

    public override void SetColor(Color rgb)
    {
        _image.color = rgb;
    }

    public override void SetPercent()
    {
        _image.fillAmount = Mathf.Abs(_percent) * 1;
    }
}
