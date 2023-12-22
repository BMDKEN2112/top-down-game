using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
{
    [SerializeField]
    private List<ModifierData> modifierData = new List<ModifierData>();

    public string actionName => "Consume";

    public AudioClip actionSFX {get; set;}

    public bool PerformAction(GameObject character)
    {
        Health health = character.GetComponent<Health>();

        if (health != null && health.currentHealth.Value * 10 >= health.maxHealth)
        {
            Debug.Log("Full HP, cannot use health booster.");
            return false;

        }
        foreach (ModifierData item in modifierData)
        {
            item.characterStatModifier.AffectCharacter(character, item.value);
        }
        return true;


    }
}

public interface IDestroyableItem
{

}

public interface IItemAction
{
    public string actionName { get; }

    public AudioClip actionSFX { get; }

    bool PerformAction(GameObject character);
}

[Serializable]
public class ModifierData
{
    public CharacterStatModifierSO characterStatModifier;
    public float value;
}
