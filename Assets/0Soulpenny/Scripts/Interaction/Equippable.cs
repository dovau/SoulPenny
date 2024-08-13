using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public class Equippable : Interactable, IEquippable //Down the road I'll bring IEquippable
    {
        private bool isEquipped = false;
        public bool IsEquipped => isEquipped;

        [SerializeField] private float swingDamage;
        [SerializeField] private float thrustDamage;
        [SerializeField] private float throwDamage;
        [SerializeField] private float bashDamage;

        private bool hasDamageCollider;
        public bool HasDamageCollider { get { return hasDamageCollider; } }

        public BoxCollider BoxCollider;

        public enum HandType
        {

            None,     // Can be either
            MainHand, // Right hand
            OffHand,  // Left hand
            TwoHands,  // ->> This however, requires both hands
            Misc // Thinking of another limb like tail, telekinetic, robot arm...etc anything that's not "Humanoid" or one of the R/L arms. Subject to change
        }

        [SerializeField] private HandType handType;
        public HandType GetHandType() => handType;


        public virtual void Equip()
        {
            isEquipped = true;
            // Additional logic when the item is equipped, if necessary
            // Gonna try to change layers now - to avoid collision with player collider when defending or any other action

            Debug.Log($"{this.GetItemName()} is now equipped.");
        }

        public virtual void UnEquip()
        {
            isEquipped = false;
            Debug.Log($"{this.GetItemName()} is now unequipped.");

        }

        //Failsafe to avoid highlighting what we hold OR change it's color when highlit, not implemented fully yet.
        //public override void Highlight(bool highlight)
        //{
        //    if (isEquipped)
        //    {
        //        base.Highlight(highlight);

        //    }
        //    else
        //    {
        //        base.Highlight(highlight);
        //    }
        //}


    }

}