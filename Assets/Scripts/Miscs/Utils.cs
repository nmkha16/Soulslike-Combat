public static class HitEffectHelper{
    public static void PlayHitEffect(HitWeapon hitWeapon){
        switch (hitWeapon)
        {
            case HitWeapon.Sword:
                SoundManager.instance.PlayAudioWithRandomPitch(SoundId.sfx_sword_hit);
                break;
            case HitWeapon.Kick:
                SoundManager.instance.PlayAudioWithRandomPitch(SoundId.sfx_non_weapon_hit);
                break;
        }
    }
}