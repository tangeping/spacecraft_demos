using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBar : Progressbar
{

    public override void Progressbar_WeaponBronEvent(WeaponAttri weapon)
    {
        if (weapon.BarNode == "missile_cd" && _weapAttri == null)
        {
            AttachToWeap(weapon);
            SetProgressBar();
        }
    }
}
