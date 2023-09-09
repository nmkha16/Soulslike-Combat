public enum AttackSequence{
    Attack1 = 0,
    Attack2,
    Attack3,
    Heavy_Attack,
    Running_Attack,
    Parry_Stab,
}

public enum MovementSequence{
    Roll
}

public enum DefenseSequence{
    Block,
    Parry,
}

public enum MariaAttackType{
    Sword,
    KickLeft,
    KickRight,
}

public enum RollDirection{
    Left = 0,
    Right,
    Forward,
    Backward = 3,
}

public enum SoundId{
    sfx_parry,
    sfx_parry_stab,
    sfx_sword_fast_whoosh,
    sfx_sword_whoosh,
    sfx_shield_hit,
    sfx_sword_hit,
    sfx_non_weapon_hit
}

public enum HitWeapon{
    Sword,
    Kick
}