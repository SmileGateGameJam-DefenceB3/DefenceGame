using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

[CreateAssetMenu]
public class StageData : SingletonScriptableObject<StageData>
{
    [Serializable]
    public class Stage
    {
        public int PlayerGold;
        public int CPUGold;
        public float MinCPUSpawnDelay;
        public float MaxCPUSpawnDelay;
        public bool IsHard;
        public int Factor;
    }

    public List<Stage> Stages;
}
