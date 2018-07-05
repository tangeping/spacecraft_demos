using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : testbase {

	// Use this for initialization
    public Vector3 position,direciton,forward;
    public int nID = 10001;
    private  GameObject clone_perfab;
    public float speed = 3.0f;

    void Start () {

        GameObject weapon_parent = Trans.FindObj(gameObject,"weapon_born_" + nID);
        if (weapon_parent == null)
        {
            Debug.LogError("not found " + "weapon_born_" + nID);
        }

        GameObject prefab = Resources.Load<GameObject>("character/spacecraft02/prefab/shoot_" + nID);
        clone_perfab = GameObject.Instantiate(prefab, weapon_parent.transform.position, weapon_parent.transform.rotation);
        clone_perfab.transform.parent = weapon_parent.transform;

        position = transform.TransformPoint( clone_perfab.transform.position);
        direciton = transform.TransformPoint(clone_perfab.transform.rotation.eulerAngles);
        forward = transform.TransformPoint(clone_perfab.transform.forward);

        StartCoroutine(onDestory());
    }

    // Update is called once per frame
    void Update () {

        if (clone_perfab != null)
        {
            clone_perfab.transform.position += forward * speed /** Time.deltaTime*/;
            Debug.Log("position:" + clone_perfab.transform.position);
        }
	}

    IEnumerator onDestory()
    {
        yield return new WaitForSeconds(400);
        Destroy(clone_perfab);
    }
}
