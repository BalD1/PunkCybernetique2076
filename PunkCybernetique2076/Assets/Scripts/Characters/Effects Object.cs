using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsObject : ScriptableObject
{
    public enum BadEffect
    {
        Poison,
        Burning,
        Slowed,
    }

    public enum GoodEffect
    {
        ExceededHealth,

    }

    public List<BadEffect> badEffects = new List<BadEffect>();
    public List<GoodEffect> goodEffects = new List<GoodEffect>();

    //PUTAIN DE JAP J'AI PAS FINI
}
