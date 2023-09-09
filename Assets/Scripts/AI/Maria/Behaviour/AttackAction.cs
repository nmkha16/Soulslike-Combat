using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;
using FSM;

namespace AI.Maria.Behaviour{
    public class AttackAction : Action
    {
        protected readonly int animMultiplierHash = Animator.StringToHash("AnimMultiplier");
        [SerializeField] private MariaAttackType attackType;
        [SerializeField] private AnimationClip anim; // reference to the anim clip so can we can get exact anim length
        [SerializeField] private AnimationCurve curve; // the animation curve to apply forward movement
        [SerializeField] private string animationHashParams;
        [SerializeField] private float easing = 3f;
        [SerializeField] private float recommendSpeed = 1.25f;
        [SerializeField] private float percentTimeOfStartHitbox;
        [SerializeField] private float percentTimeOfEndHitbox;
        private Transform hitComponent;
        private int isAttackHash;
        private Animator animator;
        private MariaBoss maria;
        private float animLength;
        private float elapsed = 0f;
        private float startHitbox,endHitbox;
        private HitWeapon hitWeapon;

        private bool calledSound = false;
        public override void Awake() {
            animator = gameObject.GetComponent<Animator>();
            maria = gameObject.GetComponent<MariaBoss>();
            isAttackHash = Animator.StringToHash(animationHashParams);

            switch (attackType)
            {
                case MariaAttackType.Sword:
                    hitWeapon = HitWeapon.Sword;
                    hitComponent = maria.rightLegTransform;
                    break;
                case MariaAttackType.KickLeft:
                    hitWeapon = HitWeapon.Kick;
                    hitComponent = maria.leftLegTransform;
                    break;
                case MariaAttackType.KickRight:
                    hitWeapon = HitWeapon.Kick;
                    hitComponent = maria.rightLegTransform;
                    break;
            }
        }

        public override void Start(){
            animLength = anim.length / recommendSpeed;

            startHitbox = percentTimeOfStartHitbox / recommendSpeed;
            endHitbox = percentTimeOfEndHitbox / recommendSpeed;
        }

        protected override Status OnUpdate()
        {
            animator.SetFloat(animMultiplierHash,recommendSpeed);
            elapsed += Time.deltaTime;

            if (elapsed >= startHitbox && elapsed <= endHitbox){
                maria.OnActivateHitbox?.Invoke(hitComponent,hitWeapon);
                if (!calledSound){
                    PlayAttackSfx();
                    calledSound = true;
                }
            }
            else {
                maria.OnEndHitbox?.Invoke();
            }

            if (elapsed >= animLength){
                elapsed = 0f;
                calledSound = false;
                animator.SetBool(isAttackHash, false);
                animator.SetFloat(animMultiplierHash,1f);
                return Status.Success;
            }
            maria.ApplyGravity();
            maria.CalculateMoveDirection(elapsed,curve,easing);
            maria.FaceMoveDirection();
            maria.Move();

            animator.SetBool(isAttackHash,true);
            return Status.Running;
        }

        public override void Abort(){
            elapsed = 0f;
            calledSound = false;
            animator.SetFloat(animMultiplierHash,1f);
            animator.SetBool(isAttackHash,false);
            maria.OnEndHitbox?.Invoke();
        }

        private void PlayAttackSfx(){
            switch (attackType)
            {
                case MariaAttackType.Sword:
                    SoundManager.instance.PlayAudioWithRandomPitch(SoundId.sfx_sword_fast_whoosh);
                    break;
                case MariaAttackType.KickLeft:
                    break;
                case MariaAttackType.KickRight:
                    break;
            }
        }
    }
}
