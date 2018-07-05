using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class WeaponEntity : GameEntity
{
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
