using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AttackAnimation{
    public AnimationClip anim;
    public AnimationCurve curve;
    public float percentTimeOfStartHitbox;
    public float percentTimeOfEndHitbox;
}

[Serializable]
public class MovementAnimation{
    public AnimationClip anim;
    public Animation curve;
    public float percentTimeOfStartIframe;
    public float percentTimeOfEndIframe;
}

[Serializable]
public class MariaAttackCooldown{
    public MariaAttackType attackType;
    public float cooldown;
}