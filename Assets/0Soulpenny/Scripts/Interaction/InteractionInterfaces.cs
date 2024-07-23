using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    void Interact();
    void Highlight(bool highlight);
}

public interface IDamagable
{
    void TakeDamage(float damageAmount);

    void TakeDamage(float damageAmount, Vector3 impactDirection);

}
