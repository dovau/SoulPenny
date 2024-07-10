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
using ECM2.Examples.FirstPerson;
using System;

namespace Soul
{
    public class FPPlayerCharacterOLDOLD : Character
    {


        FPPlayerControls controls;
        FPPlayerControls.BaseActions baseActions;
    
        
        public FPPlayerControls Controls => controls;

    


        [Tooltip("The first person camera parent.")]
        public GameObject cameraParent;
        private float _cameraPitch;
        public bool useFSM = true;

        [Space]
        Vector2 lookInput;
        public bool invertLook = true;
        public Vector2 mouseSensitivity = new Vector2(1.0f, 1.0f);
        float mouseX, mouseY;


        protected override void Awake()
        {
            base.Awake();
            controls = new FPPlayerControls();
            baseActions = controls.Base;

            Cursor.lockState = CursorLockMode.Locked;
            SubscribeToInputEvents();
        }
        private void SubscribeToInputEvents()
        {
            //baseActions.MouseX.performed += ctx => lookInput.x = ctx.ReadValue<float>();
            //baseActions.MouseY.performed += ctx => lookInput.y = ctx.ReadValue<float>();


        }


        private void Update()
        {
            HandleMouseLook(lookInput);
            Debug.Log("Camera pitch = " + _cameraPitch);
        }

        public virtual void AddControlYawInput(float value)
        {
            if (value != 0.0f)
                AddYawInput(value);

        }

        public virtual void AddControlPitchInput(float value, float minPitch = -80.0f, float maxPitch = 80.0f)
        {
            if (value != 0.0f)
            {
                _cameraPitch = MathLib.ClampAngle(_cameraPitch, minPitch, maxPitch);
            }
        }

        private void OnGUI()
        {
            
        }
        public void HandleMouseLook(Vector2 lookInput)
        {
            //Vector2 lookInput = new Vector2
            //{
            //    x = UnityEngine.Input.GetAxisRaw("Mouse X"),
            //    y = UnityEngine.Input.GetAxisRaw("Mouse Y")
            //};
            //Vector2 lookInput = controls.Base.Look.ReadValue<Vector2>();
     
            
            lookInput *= mouseSensitivity;
            Debug.Log(lookInput);

            AddControlYawInput(lookInput.x);
            AddControlPitchInput(invertLook ? -lookInput.y : lookInput.y);
            Debug.Log("Processed? " + lookInput);
        }
        protected virtual void UpdateCameraParentRotation()
        {
            cameraParent.transform.localRotation = Quaternion.Euler(_cameraPitch, 0.0f, 0.0f);

            //z9999 see if camera pitch should be replaced by OR affected by lookinput, cause as of now look input is being read. I guess.
            // 240707 - 1238
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
