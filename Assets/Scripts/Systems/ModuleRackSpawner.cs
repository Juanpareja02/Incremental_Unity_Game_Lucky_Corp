using UnityEngine;

public class ModuleRackSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] slots;

    private int nextSlot;

    public void SpawnModule(GeneratorDef def, int level)
    {
        if (def == null || def.worldPrefab == null || slots == null || slots.Length == 0)
            return;

        Transform slot = slots[Mathf.Min(nextSlot, slots.Length - 1)];
        Instantiate(def.worldPrefab, slot.position, slot.rotation, slot);
        nextSlot++;
    }
}
