using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test3 : MonoBehaviour {

    // Use this for initialization
    public Vector3 EulerAngle;
    public Vector3 Rotation;
    public Vector3 RotationEulerAngle;
    public Vector3 Radian;
    public Vector3 localforward,worldforward,shipforward;

    public Quaternion kbQua, typQua,soQua;

    public int nID = 10001;
    private GameObject clone_perfab, weapon_parent;



    void Start () {

        weapon_parent = Trans.FindObj( gameObject,"weapon_born_" + nID);
        if (weapon_parent == null)
        {
            Debug.LogError("not found " + "weapon_born_" + nID);
        }

        Radian = Trans.ToRotation(weapon_parent.transform.eulerAngles);
        kbQua = Quaternion.Euler(Radian);
        soQua = Quaternion.Euler(weapon_parent.transform.eulerAngles);
        typQua = weapon_parent.transform.rotation;
        localforward = weapon_parent.transform.forward;
        worldforward = weapon_parent.transform.TransformPoint(localforward);
        shipforward = weapon_parent.transform.TransformPoint(localforward);

        GameObject prefab = Resources.Load<GameObject>("character/spacecraft02/prefab/shoot_" + nID);
        clone_perfab = GameObject.Instantiate(prefab, weapon_parent.transform.position, soQua);
 //       clone_perfab.transform.parent = weapon_parent.transform;


    }
	
	// Update is called once per frame
	void Update () {
        Rotation = new Vector3(weapon_parent.transform.rotation.x, weapon_parent.transform.rotation.y, weapon_parent.transform.rotation.z);
        EulerAngle = weapon_parent.transform.eulerAngles;
        RotationEulerAngle = weapon_parent.transform.rotation.eulerAngles;

        clone_perfab.transform.position += worldforward * Time.deltaTime * 0.1f;
    }
}
