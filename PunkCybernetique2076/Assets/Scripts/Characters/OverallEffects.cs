using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallEffects : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public string effectName;
        public EffectsObject.Effect effectType;
        public int amount;
        public StatsObject.stats statToAffect;
        public bool temporary;
        public int time;
        public string summary;
    }
    [SerializeField] private List<Data> dataList;

    private List<UIManager.Abilities> abilitiesList;
    private List<EffectsObject> overallEffectsObjects;
    private Sprite abilitySprite;
    
    private void Start()
    {
        overallEffectsObjects = new List<EffectsObject>();
        abilitiesList = UIManager.Instance.abilitiesList;

        foreach(Data ability in dataList)
        {
            EffectsObject newAbility = new EffectsObject();
            newAbility = CreateEffect(newAbility, ability.effectName, ability.effectType, ability.amount, ability.statToAffect, ability.temporary, ability.time, ability.summary);
            overallEffectsObjects.Add(newAbility);
        }

        GameManager.Instance.OverallEffectObjects = overallEffectsObjects;

    }

    private EffectsObject CreateEffect(EffectsObject effect, string name, EffectsObject.Effect effectType, int amountInPercentage, StatsObject.stats statToAffect, bool temporary, int? time, string summary)
    {
        foreach (UIManager.Abilities ability in abilitiesList)
        {
            if (ability.name.Equals(effect.EffectName))
                abilitySprite = ability.sprite;

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
