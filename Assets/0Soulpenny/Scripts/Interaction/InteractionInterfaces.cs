using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum ItemType
{
    Misc,
    Consumable,
    Tool,
    Spell,
    Clothing,
    Armor,
    Uncountable
}

public enum HandType
{
    None, // 
    Misc, // Anything other than two human hands
    Main, // Main hand, probably almost always right
    Off, // almost always left
    Both, //requires 2 hands to use, but may be used in one too
    All // for creatures with less or more than two limbs
}

public interface IItem
{
    string ItemName { get;}
    ItemType ItemType { get; }
}
public interface IInteractable:IItem
{

    void Interact();
    void Highlight(bool highlight);
}
public interface IConsumable: IItem
{
    void Consume();
    void Deplete();
}
public interface IEquippable :IItem
{
    void Equip();
    void UnEquip();

}
public interface IWeapon : IEquippable
{
    int Damage { get; }
}


public interface IDamagable
{
    void TakeDamage(float damageAmount);

    void TakeDamage(float damageAmount, Vector3 impactDirection);

}