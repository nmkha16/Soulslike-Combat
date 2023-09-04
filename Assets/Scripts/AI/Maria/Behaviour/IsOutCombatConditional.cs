using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class IsOutCombatConditional : Conditional
    {
        private MariaBoss maria;
        protected override void OnAwake(){
            maria = gameObject.GetComponent<MariaBoss>();
        }

        protected override bool IsUpdatable()
        {
            return maria.isInCombat == false;
        }
    }
}