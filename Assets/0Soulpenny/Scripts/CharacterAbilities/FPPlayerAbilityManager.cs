using ECM2;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Playables;
using UnityEngine;

namespace Soul
{
    public class FPPlayerAbilityManager : CharacterAbilityManager
    {
        FPPlayerMediator _mediator;
        FPPlayerCharacter _playerCharacter;
        FPPlayerBrain _brain;

        public FPPlayerMediator Mediator => _mediator;

        private List<PlayerAbility> currentAbilities = new List<PlayerAbility>();
        public List<PlayerAbility> CurrentAbilities => currentAbilities;

        public List<PlayerAbility> activeAbilities = new List<PlayerAbility>();
        public List<PlayerAbility> ActiveAbilities => activeAbilities;

        

        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
            StartCoroutine(InitializeAfterMediatorSet());
        }
        private IEnumerator InitializeAfterMediatorSet()
        {
            // Wait until the mediator is set
            yield return new WaitUntil(() => _mediator != null);
            GetPlayerCharacter();
            GetBrain();
            CollectAbilities();
        }


        protected void GetPlayerCharacter()
        {
            _playerCharacter = _mediator.Character;
            if (_playerCharacter == null ) 
            {
                Debug.Log("Ability manager failed to find character in the mediator.");            
            }

        }
        protected void GetBrain()
        {
            _brain = _mediator.Brain;
            if (_brain == null)
            {
                Debug.Log("Ability manager failed to find a brain in the mediator");
            }

        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.L))
            {
                MediatorNotifyTest();
            }
        }

        public void SetMediator(FPPlayerMediator mediator)
        {
            _mediator = mediator;
        }
        public void MediatorNotifyTest()
        {
            _mediator.Notify(this, "AbilityManagerNotifying");

        }

        protected void CollectAbilities()
        {
            foreach (var ability in GetComponents<PlayerAbility>())
            {
                AddAbility(ability);
                ability.Initialize(_playerCharacter, _brain, this);
            }
            LogCurrentAbilities();

        }

        private void LogCurrentAbilities()
        {
            Debug.Log("Number of abilities found: " + currentAbilities.Count);

            string abilitiesList = string.Join(", ", currentAbilities.Select(a => a.ToString()));
            Debug.Log("Current abilities are: " + abilitiesList);
        }

        public void AddAbility(PlayerAbility ability)
        {
            if (!currentAbilities.Contains(ability))
            {
                currentAbilities.Add(ability);
                ability.Initialize(_playerCharacter, _brain, this);
            }
        }
        public void RemoveAbility(PlayerAbility ability)
        {
            if (currentAbilities.Contains(ability))
            {
                currentAbilities.Remove(ability);
            }
        }

        public T GetAbility<T>()where T : PlayerAbility
        {
            foreach (var ability in currentAbilities)
            {
                if(ability is T)
                {
                    return ability as T;
                }
            }
            return null;
        }


    }

}