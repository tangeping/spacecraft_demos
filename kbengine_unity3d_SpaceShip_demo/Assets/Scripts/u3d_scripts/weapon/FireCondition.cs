using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCondition{

    public int nID;

    public virtual bool CanFire(WeaponAttri attri)
    {
        return true;
    }
}

public class IsCooldown : FireCondition
{
    public override bool CanFire(WeaponAttri attri)
    {
        return attri.Duration == 0;
    }
}