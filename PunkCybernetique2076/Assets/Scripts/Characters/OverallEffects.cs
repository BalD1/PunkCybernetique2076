using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallEffects : MonoBehaviour
{
    private List<UIManager.Abilities> abilitiesList;
    private Sprite abilitySprite;

    private void Awake()
    {
        abilitiesList = UIManager.Instance.abilitiesList;
        EffectsObject minorAttackBoost = new EffectsObject();
        CreateEffect(minorAttackBoost, EffectsObject.Effect.PositiveStatModifier, 15, StatsObject.stats.attack, false, null, "A minor attack boost. +15% permanantly.");

        EffectsObject majorAttackBoost = new EffectsObject();
        CreateEffect(majorAttackBoost, EffectsObject.Effect.PositiveStatModifier, 30, StatsObject.stats.attack, false, null, "A major attack boost. +30% permanantly.");

        EffectsObject minorFirerateBoost = new EffectsObject();
        CreateEffect(minorFirerateBoost, EffectsObject.Effect.PositiveStatModifier, 20, StatsObject.stats.fireRate, false, null, "A minor fire rate boost. +20% permanantly.");

        EffectsObject majorFirerateBoost = new EffectsObject();
        CreateEffect(majorFirerateBoost, EffectsObject.Effect.PositiveStatModifier, 40, StatsObject.stats.fireRate, false, null, "A major fire rate boost. +40% permanantly.");

        EffectsObject fireStatut = new EffectsObject();
        CreateEffect(fireStatut, EffectsObject.Effect.NegativeStatModifier, 5, StatsObject.stats.HP, true, 5, "A burning state. Inflicts 5% of total HP damages every seconds for 5 seconds.");

        EffectsObject poisonStatut = new EffectsObject();
        CreateEffect(poisonStatut, EffectsObject.Effect.NegativeStatModifier, 2, StatsObject.stats.HP, true, 15, "A poison state. Inflicts 2% of total HP damages every seconds for 15 seconds.");

        EffectsObject vampire = new EffectsObject();
        CreateEffect(vampire, EffectsObject.Effect.PositiveStatModifier, 5, StatsObject.stats.HP, false, null, "Regenerates 5% of total HP when an ennemy is killed.");


    }

    private void CreateEffect(EffectsObject effect, EffectsObject.Effect effectType, int amountInPercentage, StatsObject.stats statToAffect, bool temporary, int? time, string summary)
    {
        foreach (UIManager.Abilities abilities in abilitiesList)
        {
            if (abilities.name.Equals(effect.ToString()))
                abilitySprite = abilities.image;

        }
        effect.Data(
            effectType,
            amountInPercentage,
            statToAffect,
            temporary,
            time,
            abilitySprite,
            summary);
    }

}
