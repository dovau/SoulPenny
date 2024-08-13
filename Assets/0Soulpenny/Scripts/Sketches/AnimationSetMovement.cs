using Animancer;
using Soul;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Soul
{
    [System.Serializable]


    public class AnimationSetMovement : AnimationSetBase
    {
        public MixerTransition2DAsset.UnShared StandingLocomotionMT;
        public MixerTransition2DAsset.UnShared CrawlingLocomotionMT;
        public MixerTransition2DAsset.UnShared ProneLocomotionMT;
        public MixerTransition2DAsset.UnShared FlyingLocomotionMT;
        public MixerTransition2DAsset.UnShared SwimmingLocomotionMT;
        public MixerTransition2DAsset.UnShared ClimbingXYLocomotionMT;
        public MixerTransition2DAsset.UnShared StairsLocomotionMT;
        public MixerTransition2DAsset.UnShared FallingGroundMT;
        public MixerTransition2DAsset.UnShared CeilingHangingLocomotionMT;
        public MixerTransition2DAsset.UnShared MonkeyClimbMT;

        public MixerTransition2DAsset.UnShared JumpingMT;

        public MixerTransition2DAsset.UnShared SlidingMT;
        public MixerTransition2DAsset.UnShared DodgingMT;
        public MixerTransition2DAsset.UnShared DashingMT;
        public MixerTransition2DAsset.UnShared RollingMT;

        public LinearMixerTransitionAsset.UnShared ClimbingLadderLMT;
        public LinearMixerTransitionAsset.UnShared LedgeHangingMT;
        public LinearMixerTransitionAsset.UnShared PipeHangMT;
        //public LinearMixerTransitionAsset.UnShared MonkeyClimbMT;


        public ClipTransitionAsset.UnShared falling;
        public ClipTransitionAsset.UnShared floating;

        public ClipTransitionAsset.UnShared landing;
        public ClipTransitionAsset.UnShared vaulting;

        public ClipTransitionAsset.UnShared jumpDiving;



    }


}