using ECM2.Examples.ToggleGravityDirection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;

namespace Soul
{
    public abstract class PlayerAbility : CharacterAbility<FPPlayerCharacter,FPPlayerBrain,FPPlayerAbilityManager>
    {

        protected FPPlayerCharacter _playerCharacter;
        protected FPPlayerBrain _playerBrain;
        protected FPPlayerAbilityManager _manager;
        protected FPPlayerControls controls;



        protected override void Awake()
        {
            base.Awake();
            if (controls != null)
            {
                Debug.Log("Character Controls found");
            }
        }


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

        }
        public override void Initialize(FPPlayerCharacter playerCharacter, FPPlayerBrain brain, FPPlayerAbilityManager manager)
        {
            _playerCharacter = playerCharacter;
            _playerBrain = brain;
            controls = brain.Controls;
            _manager = manager;
            SubscribeToInputEvents(_playerBrain);

        }
        public virtual void SubscribeToInputEvents() 
        {
        
        }
        public virtual void SubscribeToInputEvents(FPPlayerBrain brain)
        {

        }
        // Update is called once per frame
        protected override void Update()
        {
            base .Update();
        }

        public override void SetMediator(CharacterMediator mediator)
        {
            base.SetMediator(mediator);
        }

    }

}