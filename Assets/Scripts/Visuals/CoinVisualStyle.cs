using UnityEngine;

public static class CoinVisualStyle
{
    private static Material coinFaceMaterial;
    private static Material coinRimMaterial;
    private static Material coinMarkMaterial;

    public static void Apply(GameObject coin)
    {
        if (coin == null)
            return;

        EnsureMaterials();

        var renderer = coin.GetComponentInChildren<Renderer>();
        if (renderer)
            renderer.sharedMaterial = coinFaceMaterial;

        if (coin.transform.Find("Coin_Rim"))
            return;

        CreateCylinderChild(coin.transform, "Coin_Rim", new Vector3(0f, 0f, 0f), new Vector3(1.08f, 0.16f, 1.08f), coinRimMaterial);
        CreateCylinderChild(coin.transform, "Coin_TopMark", new Vector3(0f, 1.08f, 0f), new Vector3(0.52f, 0.025f, 0.52f), coinMarkMaterial);

        CreateMarkBar(coin.transform, "Coin_Mark_A", new Vector3(0f, 1.13f, 0f), new Vector3(0.12f, 0.02f, 0.82f), coinMarkMaterial);
        CreateMarkBar(coin.transform, "Coin_Mark_B", new Vector3(0f, 1.14f, 0f), new Vector3(0.82f, 0.02f, 0.12f), coinMarkMaterial);
    }

    private static void EnsureMaterials()
    {
        if (coinFaceMaterial != null)
            return;

        coinFaceMaterial = CreateMaterial("Lucky Coin Face", new Color(1f, 0.69f, 0.16f), new Color(1f, 0.62f, 0.08f), 0.35f);
        coinRimMaterial = CreateMaterial("Lucky Coin Rim", new Color(1f, 0.45f, 0.08f), new Color(1f, 0.36f, 0.04f), 0.7f);
        coinMarkMaterial = CreateMaterial("Lucky Coin Mark", new Color(1f, 0.92f, 0.42f), new Color(1f, 0.82f, 0.22f), 0.5f);
    }

    private static void CreateCylinderChild(Transform parent, string name, Vector3 localPosition, Vector3 localScale, Material material)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = localScale;

        var collider = go.GetComponent<Collider>();
        if (collider)
            Object.Destroy(collider);

        var renderer = go.GetComponent<Renderer>();
        if (renderer)
            renderer.sharedMaterial = material;
    }

    private static void CreateMarkBar(Transform parent, string name, Vector3 localPosition, Vector3 localScale, Material material)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = localScale;

        var collider = go.GetComponent<Collider>();
        if (collider)
            Object.Destroy(collider);

        var renderer = go.GetComponent<Renderer>();
        if (renderer)
            renderer.sharedMaterial = material;
    }

    private static Material CreateMaterial(string name, Color baseColor, Color emission, float emissionPower)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (!shader)
            shader = Shader.Find("Standard");

        var mat = new Material(shader);
        mat.name = name;

        if (mat.HasProperty("_BaseColor"))
            mat.SetColor("_BaseColor", baseColor);
        else
            mat.color = baseColor;

        if (mat.HasProperty("_EmissionColor"))
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emission * emissionPower);
        }

        if (mat.HasProperty("_Metallic"))
            mat.SetFloat("_Metallic", 0.25f);

        if (mat.HasProperty("_Smoothness"))
            mat.SetFloat("_Smoothness", 0.65f);

        return mat;
    }
}
