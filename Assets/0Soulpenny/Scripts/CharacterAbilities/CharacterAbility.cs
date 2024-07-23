using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;

namespace Soul
{

    public abstract class CharacterAbility <TCharacter, TBrain,TAbilityManager>: MonoBehaviour
        where TCharacter : Character
        where TBrain : CharacterBrain
        where TAbilityManager : CharacterAbilityManager
    {
        protected TCharacter _character;
        protected TBrain _brain;
        protected TAbilityManager _abilityManager;

        protected bool abilityIsActive;
        public bool AbilityIsActive 
        { get { return abilityIsActive; }
          set { abilityIsActive = value; }
        }



        protected bool isPermitted;
        protected CharacterMediator mediator;




        protected virtual void Awake()
        {

        }


        protected virtual void  Start()
        {

        }
        protected virtual void Update()
        {

        }

        public virtual void Initialize(TCharacter character, TBrain brain, TAbilityManager manager)
        {
            _character = character;
            _brain = brain;
            _abilityManager = manager;
            //model or animator stuff here with animancer - reminder


        }

        public virtual void SetMediator(CharacterMediator mediator)
        {
            this.mediator = mediator;
        }
        public abstract bool CanActivate();
        public abstract void Activate();
        public abstract void Deactivate();

    }

}