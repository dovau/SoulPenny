using ECM2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
namespace Soul
{

    public class CharacterSprint : PlayerAbility
    {

        public float maxSprintSpeed = 10f;
        private float sprintAnimMultiplier = 2f;
        private bool isSprinting = false;
        private float storedWalkSpeed;
        


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Initialize(FPPlayerCharacter playerCharacter, FPPlayerBrain brain, FPPlayerAbilityManager manager)
        {
            base.Initialize(playerCharacter, brain, manager);
            isSprinting = false;
            isPermitted = true;
            storedWalkSpeed = playerCharacter.maxWalkSpeed;
        }

        // Update is called once per frame
        protected override void Update()
        {

        }

        public override bool CanActivate()
        {
            return _brain.MovementSM != null && _brain.MovementSM.CanSprint;
            //I'm talking about here
        }
        public override void Activate()
        {
            ActivateSprint();
        }

        public override void Deactivate()
        {
            DeactivateSprint();
        }
        public void ActivateSprint()
        {
            if (!isSprinting)
            {
                storedWalkSpeed = _playerCharacter.maxWalkSpeed;
                isSprinting = true;
                _playerCharacter.maxWalkSpeed = maxSprintSpeed;
                
                Debug.Log("Sprint Activated. Previous Speed: " + storedWalkSpeed);

            }
            else
            {
                Debug.Log("ActivateSprint was called but already sprinting.");
            }
        }
        public void DeactivateSprint()
        {

            if (isSprinting)
            {
                isSprinting = false;
                _playerCharacter.maxWalkSpeed = storedWalkSpeed;
                Debug.Log("Sprint Deactivated. Current Speed: " + _playerCharacter.maxWalkSpeed);

            }
            else
            {
                Debug.Log("DeactivateSprint was called but was not sprinting.");
            }
        }
    }

}