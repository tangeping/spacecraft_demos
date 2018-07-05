using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropEntity : GameEntity {

	// Use this for initialization
	void Start () {

        npcHeight = 4.0f;

    }

    void OnGUI()
    {
        if (gameObject == null || Camera.main == null)
            return;

        if (!Trans.FindObj(gameObject, "visible").GetComponent<MeshRenderer>().isVisible)
            return;

        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + npcHeight, transform.position.z);

        if (playerCamera == null)
            playerCamera = Camera.main;

        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
        Vector2 uiposition = playerCamera.WorldToScreenPoint(worldPosition);

        //得到真实NPC头顶的2D坐标
        uiposition = new Vector2(uiposition.x, Screen.height - uiposition.y);

        //计算NPC名称的宽高
        Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(entity_name));

        //设置显示颜色为黄色
        GUI.color = entity_type == "Mine" ? Color.red :Color.green;

        //绘制NPC名称
        GUI.Label(new Rect(uiposition.x - (nameSize.x / 2), uiposition.y - nameSize.y - 5.0f, nameSize.x, nameSize.y), entity_name);

    }

    // Update is called once per frame
    void Update () {

        if (!entityEnabled)
        {
            Position = destPosition;
            return;
        }

        float deltaSpeed = Speed * Time.deltaTime;

        //       Debug.Log("GameEntity::onControlled: " + name + ",isControlled:" + isControlled);
        if (Vector3.Distance(EulerAngles, destDirection) > 0.0004f)
        {
            rotation = Quaternion.Slerp(rotation, Quaternion.Euler(destDirection), 8f * Time.deltaTime);

        }

        float dist = 0f;

        dist = Vector3.Distance(destPosition, Position);

        if (dist > 0.001f)
        {
            Vector3 movement = destPosition - Position;
            movement.y = 0;

            if (dist > deltaSpeed || movement.magnitude > deltaSpeed)
            {
                Position += movement * deltaSpeed;
            }
            else
            {
                Position = destPosition;
            }
        }
        else
        {
            Position = destPosition;
        }
    }
}
