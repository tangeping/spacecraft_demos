using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUiHP : HpProgressBar
{

    // Use this for initialization
    private Slider hp_slider = null;
    public Image fill_image = null;

    private float _duration = 0.0f;

    const float DisableTime = 5.0f;//持续5.0秒消失
    public float Duration
    {
        get
        {
            return _duration;
        }

        set
        {
            _duration = Mathf.Max(0.0f, value);
        }
    }

    private void Awake()
    {
        Debug.Log(name + "::Awake()");

        ShipBase shipScript = GetComponentInParent<ShipBase>();


        if (shipScript == null)
        {
            Debug.LogError("shipScript not found !!");
            return;
        }

        if (shipScript._attri.HP_MAX >= shipScript._attri.Hp && shipScript._attri.HP_MAX > 0)
        {
            _percent = shipScript._attri.Hp / (shipScript._attri.HP_MAX * 1.0f);
            _setPercent = _percent;
        }

    }

    void Start () {

        Debug.Log(name + "::Start()");

        hp_slider = GetComponentInChildren<Slider>();

        if(hp_slider == null)
        {
            Debug.LogError("hp_slider not found!!");
        }

        SetPercent();

        GameObject fill_obj = Trans.FindObj(gameObject, "Fill");

        if(fill_obj == null)
        {
            Debug.LogError("fill_obj not found!!");
        }

        fill_image = fill_obj.GetComponent<Image>();

	}

    private void onCountDown()
    {
        if (Duration > 0)
        {
            Duration -= Time.deltaTime;
        }
        else if (Duration == 0)
        {
            if (per_list.Count > 0)
            {
                Duration += per_list.Count;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }


    public override void SetProgressBar(float value)
    {
        base.SetProgressBar(value);
        Duration = DisableTime;
    }
    public override void SetColor(Color rgb)
    {
        fill_image.color = rgb;
    }

    public override void SetPercent()
    {
 //       Debug.LogError(name + ",SetPercent::_percent:" + _percent);

        hp_slider.value = Mathf.Abs(_percent) * 1;
    }

    // Update is called once per frame
    public override void Update ()
    {
        base.Update();
        transform.rotation = Camera.main.transform.rotation;

        onCountDown();
    }
}
