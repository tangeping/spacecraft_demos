using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test5 : MonoBehaviour {

    // Use this for initialization

    private GameObject hp_panel;

    private float HeroHeight = 0.0f;
	void Start () {

        GameObject perfab = (GameObject)Resources.Load("character/spacecraft02/prefab/hp_progress_bar");

        hp_panel = Instantiate(perfab, transform.position, transform.rotation);

        hp_panel.transform.localScale = new Vector3(0.006f, 0.006f, 0.006f);

        HeroHeight = GetComponent<Collider>().bounds.size.y;

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + HeroHeight, transform.position.z);

        hp_panel.transform.position = pos;

        hp_panel.transform.rotation = Camera.main.transform.rotation;
    }
}
