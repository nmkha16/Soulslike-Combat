using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class ShouldEnterConditional : Conditional
    {
        [SerializeField,Range(0.1f,1.0f)] private float odd = 0.4f;
        protected override bool IsUpdatable()
        {
            var random = UnityEngine.Random.Range(1,11);
            return random < odd * 10;
        }
    }
}
