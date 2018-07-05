using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour {

    // Use this for initialization
    private float _duration;

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

    void Start () {

        Debug.Log("explode Duration:" + Duration);
        StartCoroutine(OnExplodeTime());
	}
	

    IEnumerator OnExplodeTime()
    {
        yield return new WaitForSeconds(Duration);
        Debug.Log("explode gameObject:" + gameObject.name);
        Destroy(gameObject);
    }
}
