using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;


public class PlayerEntity : GameEntity
{

    // Use this for initialization
    ShipBase shipScript;
    MeshRenderer render;


    private void Start()
    {
        shipScript = GetComponent<ShipBase>();

        if(shipScript == null)
        {
            Debug.LogError("shipScript not found!!");
            return;
        }

    }

    void OnGUI()
    {
        if (gameObject == null || Camera.main == null||render == null)
            return;

        if (!render.isVisible)
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
        GUI.color = Color.yellow;

        //绘制NPC名称
        GUI.Label(new Rect(uiposition.x - (nameSize.x / 2), uiposition.y - nameSize.y - 5.0f, nameSize.x, nameSize.y), entity_name);

        //计算NPC名称的宽高
 //       Vector2 hpSize = GUI.skin.label.CalcSize(new GUIContent(hp));

//         //设置显示颜色为红
//         GUI.color = Color.red;
// 
//         //绘制HP
//         GUI.Label(new Rect(uiposition.x - (hpSize.x / 2), uiposition.y - hpSize.y - 30.0f, hpSize.x, hpSize.y), hp);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!entityEnabled || KBEngineApp.app == null)
        {
            return;
        }

        if (isPlayer == isControlled)
            return;

        KBEngine.Event.fireIn("updatePlayer", SpaceID, transform.position.x, transform.position.y, transform.position.z, transform.rotation.eulerAngles.y);

    }


    public override void OnJump()
    {
        Debug.Log("jumpState: " + jumpState);

        if (jumpState != 0)
            return;

        jumpState = 1;
    }

    void Update()
    {
        
        if (render == null && shipScript.ship_Body != null)
        {
            render = shipScript.ship_Body.GetComponent<MeshRenderer>();
        }

        if (!entityEnabled)
        {
            Position = destPosition;
            return;
        }

        float deltaSpeed = Speed * Time.deltaTime;

        if (isPlayer == true && isControlled == false)
        {
            return;
        }
        //        Debug.Log("GameEntity::onControlled: " + name + ",isControlled:" + isControlled);
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
