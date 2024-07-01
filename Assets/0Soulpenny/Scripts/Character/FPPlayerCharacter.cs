using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;
using UnityEngine.Windows;

namespace Soul
{
    public class FPPlayerCharacter : Character
    {
        public InputManager input;
        [Tooltip("The first person camera parent.")]
        public GameObject cameraParent;
        private float _cameraPitch;
        public bool useFSM = true;



        public virtual void AddControlYawInput(float value)
        {
            if (value != 0.0f)
                AddYawInput(value);
        }
        protected override void Awake()
        {
            base.Awake();
            input = GetComponent<InputManager>();
        }

        public virtual void AddControlPitchInput(float value, float minPitch = -80.0f, float maxPitch = 80.0f)
        {
            if(value != 0.0f)
            {
                _cameraPitch = MathLib.ClampAngle(_cameraPitch, minPitch, maxPitch);
            }
        }

        protected virtual void UpdateCameraParentRotation()
        {
            cameraParent.transform.localRotation = Quaternion.Euler(_cameraPitch,0.0f,0.0f);
        }

        protected virtual void LateUpdate()
        {
            UpdateCameraParentRotation();
        }


        protected override void Reset()
        {
            base.Reset();
            SetRotationMode(RotationMode.None);

        }
    }

}
