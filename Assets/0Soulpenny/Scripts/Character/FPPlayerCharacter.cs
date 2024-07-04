using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.Animations;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Composites;

namespace Soul
{
    public class FPPlayerCharacter : Character
    {
        //Removing the inputmanager or FPInput in the old system completely
        //I can directly access FPPlayerControls, which is the new input system asset 
        //public InputManagerOld input;

        private FPPlayerControls controls;
        public FPPlayerControls Controls => controls;



        [Tooltip("The first person camera parent.")]
        public GameObject cameraParent;
        private float _cameraPitch;
        public bool useFSM = true;

        [Space]
        public bool invertLook = true;
        public Vector2 mouseSensitivity = new Vector2(1.0f, 1.0f);



        protected override void Awake()
        {
            base.Awake();
            controls = new FPPlayerControls();

            Cursor.lockState = CursorLockMode.Locked;
            SubscribeToInputEvents();
        }
        private void SubscribeToInputEvents()
        {
            //Debug.Log("Subscribing to Input Events");
            //controls.Base.Look.started += HandleMouseLook;
            //controls.Base.Look.performed += HandleMouseLook;
        }

        protected void Update()
        {
            HandleMouseLook();
            Debug.Log("Camera pitch = " + _cameraPitch);
        }

        public virtual void AddControlYawInput(float value)
        {
            if (value != 0.0f)
                AddYawInput(value);
        }

        public virtual void AddControlPitchInput(float value, float minPitch = -80.0f, float maxPitch = 80.0f)
        {
            if(value != 0.0f)
            {
                _cameraPitch = MathLib.ClampAngle(_cameraPitch, minPitch, maxPitch);
            }
        }

        //protected virtual void UpdateCameraParentRotation()
        //{
        //    cameraParent.transform.localRotation = Quaternion.Euler(_cameraPitch, 0.0f, 0.0f);
        //}

        //protected virtual void LateUpdate()
        //{
        //    UpdateCameraParentRotation();
        //}
        public void HandleMouseLook()
        {
            Vector2 lookInput = new Vector2
            {
                x = UnityEngine.Input.GetAxisRaw("Mouse X"),
                y = UnityEngine.Input.GetAxisRaw("Mouse Y")
            };

            lookInput *= mouseSensitivity;

            AddControlYawInput(lookInput.x);
            AddControlPitchInput(invertLook ? -lookInput.y : lookInput.y);
        }
        //public void HandleMouseLook(InputAction.CallbackContext context)
        //{

        //    Vector2 lookInput = new Vector2
        //    {
        //        x = controls.Base.Look.ReadValue<Vector2>().x,
        //        y = controls.Base.Look.ReadValue<Vector2>().y
        //    };

        //    Debug.Log(lookInput);
        //    lookInput *= mouseSensitivity;

        //    AddControlYawInput(lookInput.x);
        //    AddControlPitchInput(invertLook ? -lookInput.y : lookInput.y);
        //}

        protected override void Reset()
        {
            base.Reset();
            SetRotationMode(RotationMode.None);

        }


        protected override void OnEnable()
        {
            base.OnEnable();
            controls.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            controls.Disable();
        }
    }

}
