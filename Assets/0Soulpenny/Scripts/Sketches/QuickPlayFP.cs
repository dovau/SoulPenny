using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

namespace Soul

{
    public class QuickPlayFP : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private AnimationClip idleClip;
        [SerializeField] private AnimationClip movingClip;
        [SerializeField] private FPPlayerMediator _Mediator;
        [SerializeField] private FPPlayerBrain _Brain;

        private void Start()
        {
            _Brain = _Mediator.GetComponent<FPPlayerBrain>();
        }

        void OnEnable()
        {
            _Animancer.Play(idleClip);
        }

        private void Update()
        {
            if (_Animancer != null)
            {
                var currentState = _Brain.MovementSM.CurrentState;
                if(currentState is StandingState )
                {
                    _Animancer.Play(idleClip);
                }
                else if(currentState is WalkingState )
                {
                    _Animancer.Play(movingClip);
                }

            }
        }
    }
}