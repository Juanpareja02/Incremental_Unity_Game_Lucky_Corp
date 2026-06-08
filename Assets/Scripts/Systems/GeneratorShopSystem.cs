using System.Collections.Generic;
using UnityEngine;

public class GeneratorShopSystem : MonoBehaviour
{
    [SerializeField] private EconomySystem economy;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private ModuleRackSpawner moduleRack;
    [SerializeField] private List<GeneratorDef> generators;

    private readonly Dictionary<GeneratorDef, int> levels = new Dictionary<GeneratorDef, int>();

    public IReadOnlyList<GeneratorDef> Generators => generators;

    private void Awake()
    {
        if (!economy) economy = FindAnyObjectByType<EconomySystem>();
        if (!coinSpawner) coinSpawner = FindAnyObjectByType<CoinSpawner>();
        if (!moduleRack) moduleRack = FindAnyObjectByType<ModuleRackSpawner>();
    }

    public int GetLevel(GeneratorDef def)
    {
        return levels.TryGetValue(def, out int lvl) ? lvl : 0;
    }

    public double GetCost(GeneratorDef def)
    {
        int lvl = GetLevel(def);
        return def.baseCost * System.Math.Pow(def.costGrowth, lvl);
    }

    public bool IsMaxed(GeneratorDef def)
    {
        return def != null && def.HasMaxLevel && GetLevel(def) >= def.maxLevel;
    }

    public bool TryBuy(GeneratorDef def)
    {
        if (def == null || economy == null || coinSpawner == null)
            return false;

        if (IsMaxed(def))
            return false;

        double cost = GetCost(def);
        if (!economy.TrySpend(cost))
            return false;

        levels[def] = GetLevel(def) + 1;
        ApplyUpgrade(def);
        moduleRack?.SpawnModule(def, GetLevel(def));
        return true;
    }

    private void ApplyUpgrade(GeneratorDef def)
    {
        switch (def.effect)
        {
            case GeneratorDef.UpgradeEffect.AddCoinsPerClick:
                coinSpawner.AddCoinsPerClick(Mathf.RoundToInt((float)def.effectValue));
                break;

            case GeneratorDef.UpgradeEffect.MultiplyCoinValue:
                coinSpawner.MultiplyCoinValue(def.effectValue);
                break;

            case GeneratorDef.UpgradeEffect.AddSpitForce:
                coinSpawner.AddSpitForce((float)def.effectValue);
                break;

            case GeneratorDef.UpgradeEffect.AddSpread:
                coinSpawner.AddSpread((float)def.effectValue);
                break;

            case GeneratorDef.UpgradeEffect.AddAutoSpitAmount:
                coinSpawner.AddAutoSpitAmount(Mathf.RoundToInt((float)def.effectValue), def.autoSpitInterval);
                break;
        }
    }
}
