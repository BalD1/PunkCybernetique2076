using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallEffects : MonoBehaviour
{
    private List<UIManager.Abilities> abilitiesList;
    private List<EffectsObject> overallEffectsObjects;
    private Sprite abilitySprite;

    // à refaire dans l'éditeur, là c'est trop moche 
    private void Start()
    {
        overallEffectsObjects = new List<EffectsObject>();

        abilitiesList = UIManager.Instance.abilitiesList;
        EffectsObject minorAttackBoost = new EffectsObject();
        minorAttackBoost = CreateEffect(minorAttackBoost, "minorAttackBoost", EffectsObject.Effect.PositiveStatModifier, 15, StatsObject.stats.attack, false, null, "A minor attack boost. +15% permanantly.");
        overallEffectsObjects.Add(minorAttackBoost);

        EffectsObject majorAttackBoost = new EffectsObject();
        majorAttackBoost = CreateEffect(majorAttackBoost, "majorAttackBoost", EffectsObject.Effect.PositiveStatModifier, 30, StatsObject.stats.attack, false, null, "A major attack boost. +30% permanantly.");
        overallEffectsObjects.Add(majorAttackBoost);

        EffectsObject minorFirerateBoost = new EffectsObject();
        minorFirerateBoost = CreateEffect(minorFirerateBoost, "minorFirerateBoost", EffectsObject.Effect.PositiveStatModifier, 5, StatsObject.stats.fireRate, false, null, "A minor fire rate boost. +5% permanantly.");
        overallEffectsObjects.Add(minorFirerateBoost);

        EffectsObject majorFirerateBoost = new EffectsObject();
        majorFirerateBoost = CreateEffect(majorFirerateBoost, "majorFirerateBoost", EffectsObject.Effect.PositiveStatModifier, 10, StatsObject.stats.fireRate, false, null, "A major fire rate boost. +10% permanantly.");
        overallEffectsObjects.Add(majorFirerateBoost);

        EffectsObject fireStatut = new EffectsObject();
        fireStatut = CreateEffect(fireStatut, "fireStatut", EffectsObject.Effect.NegativeStatModifier, 5, StatsObject.stats.HP, true, 5, "A burning state. Inflicts 5% of total HP damages every seconds for 5 seconds.");
        overallEffectsObjects.Add(fireStatut);

        EffectsObject poisonStatut = new EffectsObject();
        poisonStatut = CreateEffect(poisonStatut, "poisonStatut", EffectsObject.Effect.NegativeStatModifier, 2, StatsObject.stats.HP, true, 15, "A poison state. Inflicts 2% of total HP damages every seconds for 15 seconds.");
        overallEffectsObjects.Add(poisonStatut);

        EffectsObject vampire = new EffectsObject();
        vampire = CreateEffect(vampire, "vampire", EffectsObject.Effect.PositiveStatModifier, 5, StatsObject.stats.HP, false, null, "Regenerates 5% of total HP when an ennemy is killed.");
        overallEffectsObjects.Add(vampire);

        GameManager.Instance.OverallEffectObjects = overallEffectsObjects;

    }

    private EffectsObject CreateEffect(EffectsObject effect, string name, EffectsObject.Effect effectType, int amountInPercentage, StatsObject.stats statToAffect, bool temporary, int? time, string summary)
    {
        foreach (UIManager.Abilities abilities in abilitiesList)
        {
            if (abilities.name.Equals(effect.ToString()))
                abilitySprite = abilities.sprite;

        }
        effect.Data(
            effectType,
            name,
            amountInPercentage,
            statToAffect,
            temporary,
            time,
            abilitySprite,
            summary);

        return effect;
    }

}
