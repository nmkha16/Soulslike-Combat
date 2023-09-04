using System.Collections;
using System.Collections.Generic;
using UniBT;
using UnityEngine;

namespace AI.Maria.Behaviour{
    public class IsAtSpawnConditional : Conditional
    {
        [SerializeField] private float maxAcceptableDistanceFromSpawn = 0.02f;
        private MariaBoss maria;
        private Vector3 spawnPosition;

        protected override void OnAwake(){
            maria = gameObject.GetComponent<MariaBoss>();
        }

        protected override void OnStart(){
            spawnPosition = maria.spawnPosition;
        }

        protected override bool IsUpdatable()
        {
            return maria.IsArriveAtPosition(spawnPosition,maxAcceptableDistanceFromSpawn);
        }
    }

}
 