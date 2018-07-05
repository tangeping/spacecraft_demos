using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour {

	// Use this for initialization
    public const int expbar_count = 12;

    private List<GameObject> expbar_list = new List<GameObject>();

    protected List<int> count_list = new List<int>();

    private int setCount = 0 , curCount = 0;

    private float perTime = 0.0f;

    public enum EXP_STATE
    {
        STABLE,
        DEC,
        AES,
    };

    public EXP_STATE _state = EXP_STATE.STABLE;

    void Start () {
		
        for(int i = 1;i <= expbar_count; i++)
        {
            GameObject expcd = Trans.FindObj(gameObject, "expcd_" + i);
            if(expcd == null)
            {
                Debug.LogError("expcd_" + i + ",not found!");
                continue;
            }
            expbar_list.Add(expcd);
        }

        ResetExpProcess();
    }
	
    void ResetExpProcess()
    {
        for (int i = 0; i < expbar_list.Count; i++)
        {
            GameObject obj = expbar_list[i];

            SetOneExpBar(obj, 0);
        }
    }

    void SetOneExpBar(GameObject obj,int value)
    {
        Image objImage =  obj.GetComponent<Image>();
        if(objImage == null)
        {
            Debug.LogError(obj.name + " not found image");
            return;
        }
        if (objImage.fillAmount != value)
        {
            objImage.fillAmount = value;
        }
    }

    public void SetExpProcessBar(int value)
    {
        if(value <0 || value > expbar_count)
        {
            return;
        }
        Debug.Log(name + ",ExpBar::SetExpProcessBar,value:" + value);

        count_list.Add(value);
    }

    void onSetState()
    {
        if (curCount < setCount)
        {
            _state = EXP_STATE.AES;
        }
        else if (curCount > setCount)
        {
            _state = EXP_STATE.DEC;
        }
        else if(curCount == setCount)
        {
            _state = EXP_STATE.STABLE;
        }
    }

    void AdjustPercent()
    {
        if (_state == EXP_STATE.STABLE)
        {
            return;
        }
        perTime += Time.deltaTime;

        if(perTime < 0.2f)
        {
            return;
        }
        perTime = 0.0f;

        if (curCount / expbar_count > 0)
        {
            curCount = curCount % expbar_count;

            ResetExpProcess();
        }

        SetOneExpBar(expbar_list[curCount], 1);

        if (curCount == setCount)
        {
            _state = EXP_STATE.STABLE;
        }
        else
        {
            curCount++;
        }
        
    }

    private void Update()
    {
        if(count_list.Count > 0 && _state == EXP_STATE.STABLE)
        {
            setCount = count_list[0];
            count_list.Remove(setCount);
            onSetState();
        }

        AdjustPercent();
    }
}
