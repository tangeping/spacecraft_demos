using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpProgressBar : MonoBehaviour {

    // Use this for initialization
    protected List<float> per_list = new List<float>();

    protected float _percent = 1.0f;

    protected float _setPercent = 1.0f;

    public enum HP_STATE
    {

        STABLE,
        DEC,
        AES,
    };

    protected HP_STATE _state = HP_STATE.STABLE;



    public virtual void SetProgressBar(float value)
    {
//        Debug.LogError(gameObject.name + ",percent:" + value);

        if (!isActiveAndEnabled || value < 0.0f || value > 1.0f)
        {
            return;
        }

        per_list.Add(value);

    }

    protected virtual void onSetState()
    {
        if (_percent == _setPercent)
        {
            _state = HP_STATE.STABLE;
            return;
        }
        else if (_percent < _setPercent)
        {
            _state = HP_STATE.AES;
        }
        else if (_percent > _setPercent)
        {
            _state = HP_STATE.DEC;
        }
    }

    public virtual void HpBarDisable()
    {

    }

    public virtual void PercentAscent()
    {
        if (_percent - _setPercent < 0.01f)
        {
            _percent += 0.01f/*Time.deltaTime*/;
        }
        else
        {
            _percent = _setPercent;
            _state = HP_STATE.STABLE;
        }
    }

    public virtual void PercentDecline()
    {
        if (_percent - _setPercent > 0.01f)
        {
            _percent -= 0.01f/*Time.deltaTime*/;
        }
        else
        {
            _percent = _setPercent;
            _state = HP_STATE.STABLE;
        }
    }

    public virtual void SetColor(Color rgb)
    {

    }

    public virtual void SetPercent()
    {

    }

    public virtual void AdjustPercent()
    {

        if (_state == HP_STATE.STABLE)
        {
            return;
        }

        if (_state == HP_STATE.AES)
        {
            PercentAscent();
        }
        else if (_state == HP_STATE.DEC)
        {
            PercentDecline();
        }

        if (_percent < 0.3)
        {
            SetColor(Color.red);
        }
        else
        {
            SetColor(Color.white);
        }

        SetPercent();
    }


	// Update is called once per frame
	public virtual void Update ()
    {
        if (per_list.Count > 0 && _state == HP_STATE.STABLE)
        {
            _setPercent = per_list[0];
            per_list.Remove(_setPercent);

            onSetState();
        }

        AdjustPercent();

        
    }
}
