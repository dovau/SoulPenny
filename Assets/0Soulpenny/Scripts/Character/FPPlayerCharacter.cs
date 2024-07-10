using ECM2;
using Soul;
using UnityEngine;

namespace ECM2
{
    public class FPPlayerCharacter : Character
    {
        protected FPPlayerControls controls;
        public FPPlayerControls Controls => controls;

        [Tooltip("The first person camera parent.")]
        public GameObject cameraParent;

        private float _cameraPitch;

        protected override void Awake()
        {
            base.Awake();

            controls = new FPPlayerControls();
        }


        public virtual void AddControlYawInput(float value)
        {
            if (value != 0.0f)
                AddYawInput(value);
        }

        public virtual void AddControlPitchInput(float value, float minPitch = -80.0f, float maxPitch = 80.0f)
        {
            if (value != 0.0f)
                _cameraPitch = MathLib.ClampAngle(_cameraPitch + value, minPitch, maxPitch);
        }

        protected virtual void UpdateCameraParentRotation()
        {
            cameraParent.transform.localRotation = Quaternion.Euler(_cameraPitch, 0.0f, 0.0f);
        }

        protected virtual void LateUpdate()
        {
            UpdateCameraParentRotation();
        }

        protected override void Reset()
        {
            // Call base method implementation

            base.Reset();

            // Disable character's rotation,
            // it is handled by the AddControlYawInput method 

            SetRotationMode(RotationMode.None);
        }
    }

}
