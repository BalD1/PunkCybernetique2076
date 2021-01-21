using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : LivingEntities
{
    [SerializeField] private Transform characterTransform;

    #region animation curves

    [SerializeField] private AnimationCurve healthPerLevelCurve;
    [SerializeField] private AnimationCurve attackPerLevelCurve;
    [SerializeField] private AnimationCurve speedPerLevelCurve;
    [SerializeField] private AnimationCurve neededExpPerLevelCurve;
    [SerializeField] private AnimationCurve fireRateLevelCurve;

    #endregion

    #region character movements variables

    private float xMovement;
    private float zMovement;
    private Vector3 move;

    #endregion

    private void Awake()
    {
        BaseStats();
    }

    private void Start()
    {

        this.level.ChangeData(null, 0);
        LevelUp(
                  (int)neededExpPerLevelCurve.Evaluate(level.Value + 1),
                  (int)healthPerLevelCurve.Evaluate(level.Value + 1),
                  (int)attackPerLevelCurve.Evaluate(level.Value + 1),
                  (int)speedPerLevelCurve.Evaluate(level.Value + 1),
                  (int)fireRateLevelCurve.Evaluate(level.Value + 1)
         );

        // TEST CODE

        level.DebugLog();
        HP.DebugLog();
        attack.DebugLog();
        speed.DebugLog();
        fireRate.DebugLog();

    }

    void Update()
    {

    }

    private void TakingDamage()
    {

    }
}