using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test7 : MonoBehaviour {

    // Use this for initialization
    //----------------枪口火花设置------------//
    GameObject sparks_left_1, sparks_left_2, sparks_right_1, sparks_right_2;

    public bool isPlay = false;
    void InitSparks()
    {
        sparks_left_1 = Trans.FindObj(gameObject, "locator1").transform.Find("spacecraft02_pao").gameObject;
        if (sparks_left_1 == null)
        {
            Debug.LogError("sparks_left_1 not found!!");
        }

        sparks_left_2 = Trans.FindObj(gameObject, "locator2").transform.Find("spacecraft02_pao").gameObject; ;
        if (sparks_left_2 == null)
        {
            Debug.LogError("sparks_left_2 not found!!");
        }

        sparks_right_1 = Trans.FindObj(gameObject, "locator3").transform.Find("spacecraft02_pao").gameObject; ;
        if (sparks_right_1 == null)
        {
            Debug.LogError("sparks_right_1 not found!!");
        }

        sparks_right_2 = Trans.FindObj(gameObject, "locator4").transform.Find("spacecraft02_pao").gameObject; ;
        if (sparks_right_2 == null)
        {
            Debug.LogError("sparks_right_2 not found!!");
        }

    }

    void SparkPlay()
    {
        for (int i = 0; i < sparks_left_1.transform.childCount; i++)
        {
            ParticleSystem effect = sparks_left_1.transform.GetChild(i).GetComponent<ParticleSystem>();
            effect.Stop();
            effect.Play();
        }

        for (int i = 0; i < sparks_left_2.transform.childCount; i++)
        {
            ParticleSystem effect = sparks_left_2.transform.GetChild(i).GetComponent<ParticleSystem>();
            effect.Stop();
            effect.Play();
        }

        for (int i = 0; i < sparks_right_1.transform.childCount; i++)
        {
            ParticleSystem effect = sparks_right_1.transform.GetChild(i).GetComponent<ParticleSystem>();
            effect.Stop();
            effect.Play();
        }

        for (int i = 0; i < sparks_right_2.transform.childCount; i++)
        {
            ParticleSystem effect = sparks_right_2.transform.GetChild(i).GetComponent<ParticleSystem>();
            effect.Stop();
            effect.Play();
        }
    }

    void Start () {
        InitSparks();

    }
	
	// Update is called once per frame
	void Update () {
		
        if(isPlay)
        {
            isPlay = false;
            SparkPlay();
        }
	}
}
