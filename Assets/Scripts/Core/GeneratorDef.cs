using UnityEngine;

[CreateAssetMenu(menuName = "Incremental/Generator")]
public class GeneratorDef : ScriptableObject
{
    public enum UpgradeEffect
    {
        AddCoinsPerClick,
        MultiplyCoinValue,
        AddSpitForce,
        AddSpread,
        AddAutoSpitAmount
    }

    public string displayName = "Upgrade";
    [TextArea] public string description;
    public double baseCost = 10;
    public double costGrowth = 1.35;
    public int maxLevel = 0;

    public UpgradeEffect effect = UpgradeEffect.AddCoinsPerClick;
    public double effectValue = 1;
    public float autoSpitInterval = 2f;

    public GameObject worldPrefab;

    public bool HasMaxLevel => maxLevel > 0;

    public string GetEffectLabel()
    {
        switch (effect)
        {
            case UpgradeEffect.AddCoinsPerClick:
                return $"+{effectValue:0} monedas/click";
            case UpgradeEffect.MultiplyCoinValue:
                return $"x{effectValue:0.##} valor moneda";
            case UpgradeEffect.AddSpitForce:
                return $"+{effectValue:0.#} fuerza";
            case UpgradeEffect.AddSpread:
                return $"+{effectValue:0.#} dispersion";
            case UpgradeEffect.AddAutoSpitAmount:
                return $"+{effectValue:0} auto cada {autoSpitInterval:0.#}s";
            default:
                return "Mejora";
        }
    }
}
