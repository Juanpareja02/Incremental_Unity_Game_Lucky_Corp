using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[DefaultExecutionOrder(120)]
public class RuntimeArtDirector : MonoBehaviour
{
    private Material floorMat;
    private Material wallMat;
    private Material darkMetalMat;
    private Material machineMat;
    private Material cyanMat;
    private Material goldMat;
    private Material orangeMat;
    private Material hazardMat;
    private Material gloveMat;
    private Material skinMat;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (FindAnyObjectByType<RuntimeArtDirector>() != null)
            return;

        var go = new GameObject("RuntimeArtDirector");
        DontDestroyOnLoad(go);
        go.AddComponent<RuntimeArtDirector>();
    }

    private void Start()
    {
        if (GameObject.Find("RuntimeArt"))
            return;

        CreateMaterials();
        StyleBaseScene();
        BuildRoomDressing();
        BuildMachineDressing();
        BuildPlayerHands();
        TuneLighting();
    }

    private void CreateMaterials()
    {
        floorMat = CreateMaterial("Arcade Floor", new Color(0.055f, 0.062f, 0.07f), Color.black, 0f, 0.05f, 0.35f);
        wallMat = CreateMaterial("Industrial Wall", new Color(0.075f, 0.085f, 0.095f), Color.black, 0f, 0.02f, 0.25f);
        darkMetalMat = CreateMaterial("Dark Gunmetal", new Color(0.03f, 0.038f, 0.045f), Color.black, 0f, 0.1f, 0.55f);
        machineMat = CreateMaterial("Lucky Corp Red", new Color(0.55f, 0.07f, 0.11f), new Color(0.8f, 0.03f, 0.06f), 0.18f, 0.05f, 0.45f);
        cyanMat = CreateMaterial("Vortex Cyan Neon", new Color(0.05f, 0.85f, 1f), new Color(0.05f, 0.85f, 1f), 1.8f, 0f, 0.85f);
        goldMat = CreateMaterial("Casino Gold Neon", new Color(1f, 0.72f, 0.18f), new Color(1f, 0.58f, 0.08f), 1.2f, 0.2f, 0.7f);
        orangeMat = CreateMaterial("Warning Orange", new Color(1f, 0.38f, 0.08f), new Color(1f, 0.24f, 0.04f), 0.9f, 0.05f, 0.55f);
        hazardMat = CreateMaterial("Hazard Yellow", new Color(1f, 0.85f, 0.1f), new Color(1f, 0.65f, 0.04f), 0.35f, 0.05f, 0.45f);
        gloveMat = CreateMaterial("Technician Glove", new Color(0.025f, 0.028f, 0.032f), Color.black, 0f, 0f, 0.35f);
        skinMat = CreateMaterial("Low Poly Skin", new Color(0.88f, 0.55f, 0.36f), Color.black, 0f, 0f, 0.28f);
    }

    private void StyleBaseScene()
    {
        ApplyMaterial("Floor", floorMat);
        ApplyMaterial("Wall_N", wallMat);
        ApplyMaterial("Wall_S", wallMat);
        ApplyMaterial("Wall_E", wallMat);
        ApplyMaterial("Wall_W", wallMat);
        ApplyMaterial("CoinPile", machineMat);
        ApplyMaterial("Hand", gloveMat);
    }

    private void BuildRoomDressing()
    {
        var root = new GameObject("RuntimeArt").transform;

        BuildLowRail(root);
        BuildFloorGrid(root);
        BuildWallStrips(root);
        BuildPipes(root);
        BuildDoorFrame(root);
        BuildCeilingLights(root);
    }

    private void BuildLowRail(Transform root)
    {
        CreateCube(root, "Coin_Rail_N", new Vector3(0f, 0.22f, 4.72f), new Vector3(9.6f, 0.42f, 0.18f), darkMetalMat);
        CreateCube(root, "Coin_Rail_S", new Vector3(0f, 0.22f, -4.72f), new Vector3(9.6f, 0.42f, 0.18f), darkMetalMat);
        CreateCube(root, "Coin_Rail_E", new Vector3(4.72f, 0.22f, 0f), new Vector3(0.18f, 0.42f, 9.6f), darkMetalMat);
        CreateCube(root, "Coin_Rail_W_N", new Vector3(-4.72f, 0.22f, 2.95f), new Vector3(0.18f, 0.42f, 3.45f), darkMetalMat);
        CreateCube(root, "Coin_Rail_W_S", new Vector3(-4.72f, 0.22f, -2.95f), new Vector3(0.18f, 0.42f, 3.45f), darkMetalMat);

        CreateCube(root, "Rail_Neon_N", new Vector3(0f, 0.48f, 4.6f), new Vector3(9.2f, 0.045f, 0.045f), cyanMat, false);
        CreateCube(root, "Rail_Neon_S", new Vector3(0f, 0.48f, -4.6f), new Vector3(9.2f, 0.045f, 0.045f), cyanMat, false);
        CreateCube(root, "Rail_Neon_E", new Vector3(4.6f, 0.48f, 0f), new Vector3(0.045f, 0.045f, 9.2f), cyanMat, false);
        CreateCube(root, "Rail_Neon_W_N", new Vector3(-4.6f, 0.48f, 2.95f), new Vector3(0.045f, 0.045f, 3.2f), cyanMat, false);
        CreateCube(root, "Rail_Neon_W_S", new Vector3(-4.6f, 0.48f, -2.95f), new Vector3(0.045f, 0.045f, 3.2f), cyanMat, false);
    }

    private void BuildFloorGrid(Transform root)
    {
        for (int i = -4; i <= 4; i++)
        {
            CreateCube(root, "Floor_Line_X_" + i, new Vector3(i, 0.032f, 0f), new Vector3(0.025f, 0.012f, 9.1f), cyanMat, false);
            CreateCube(root, "Floor_Line_Z_" + i, new Vector3(0f, 0.034f, i), new Vector3(9.1f, 0.012f, 0.025f), cyanMat, false);
        }

        CreateCube(root, "Machine_Hazard_A", new Vector3(-1.1f, 0.045f, -1.45f), new Vector3(1.2f, 0.018f, 0.08f), hazardMat, false);
        CreateCube(root, "Machine_Hazard_B", new Vector3(1.1f, 0.045f, -1.45f), new Vector3(1.2f, 0.018f, 0.08f), hazardMat, false);
    }

    private void BuildWallStrips(Transform root)
    {
        CreateCube(root, "Wall_Neon_N", new Vector3(0f, 2.5f, 4.88f), new Vector3(7.8f, 0.08f, 0.06f), cyanMat, false);
        CreateCube(root, "Wall_Neon_S", new Vector3(0f, 2.5f, -4.88f), new Vector3(7.8f, 0.08f, 0.06f), cyanMat, false);
        CreateCube(root, "Wall_Neon_E", new Vector3(4.88f, 2.5f, 0f), new Vector3(0.06f, 0.08f, 7.8f), goldMat, false);
        CreateCube(root, "Wall_Neon_W", new Vector3(-4.88f, 2.5f, 0f), new Vector3(0.06f, 0.08f, 7.8f), goldMat, false);
    }

    private void BuildPipes(Transform root)
    {
        for (int i = -1; i <= 1; i++)
        {
            CreateCube(root, "Cable_N_" + i, new Vector3(-3.2f + i * 0.34f, 1.55f, 4.78f), new Vector3(0.08f, 1.9f, 0.08f), darkMetalMat, false);
            CreateCube(root, "Cable_E_" + i, new Vector3(4.78f, 1.35f, -2.6f + i * 0.34f), new Vector3(0.08f, 1.5f, 0.08f), darkMetalMat, false);
        }
    }

    private void BuildDoorFrame(Transform root)
    {
        var door = FindObject("Door");
        Vector3 center = door ? door.transform.position : new Vector3(-4.8f, 1.5f, 0f);

        CreateCube(root, "Door_Frame_Left", center + new Vector3(0f, 0f, -0.72f), new Vector3(0.22f, 3.4f, 0.12f), darkMetalMat);
        CreateCube(root, "Door_Frame_Right", center + new Vector3(0f, 0f, 0.72f), new Vector3(0.22f, 3.4f, 0.12f), darkMetalMat);
        CreateCube(root, "Door_Frame_Top", center + new Vector3(0f, 1.72f, 0f), new Vector3(0.22f, 0.16f, 1.6f), cyanMat, false);
    }

    private void BuildCeilingLights(Transform root)
    {
        CreatePointLight(root, "Cyan_Key_Light", new Vector3(0f, 3.7f, -1.7f), new Color(0.2f, 0.8f, 1f), 2.8f, 8f);
        CreatePointLight(root, "Gold_Machine_Light", new Vector3(0f, 3.2f, 1.2f), new Color(1f, 0.62f, 0.16f), 1.7f, 5.5f);
    }

    private void BuildMachineDressing()
    {
        var source = FindAnyObjectByType<CoinSource>();
        if (!source)
            return;

        Transform machine = source.transform;
        ApplyRenderer(machine.gameObject, machineMat);

        CreateLocalCube(machine, "Art_Marquee", new Vector3(0f, 1.65f, -0.68f), new Vector3(1.55f, 0.32f, 0.12f), goldMat, false);
        CreateLocalCube(machine, "Art_ReelPanel", new Vector3(0f, 1.1f, -0.72f), new Vector3(1.45f, 0.55f, 0.1f), darkMetalMat, false);

        for (int i = -1; i <= 1; i++)
            CreateLocalCube(machine, "Art_Reel_" + (i + 2), new Vector3(i * 0.42f, 1.1f, -0.79f), new Vector3(0.32f, 0.38f, 0.08f), cyanMat, false);

        CreateLocalCube(machine, "Art_CoinMouthGlow", new Vector3(0f, 0.46f, -0.82f), new Vector3(0.85f, 0.16f, 0.08f), orangeMat, false);
        CreateLocalCube(machine, "Art_SidePanel_L", new Vector3(-0.9f, 0.8f, -0.08f), new Vector3(0.12f, 1.3f, 1.1f), darkMetalMat, false);
        CreateLocalCube(machine, "Art_SidePanel_R", new Vector3(0.9f, 0.8f, -0.08f), new Vector3(0.12f, 1.3f, 1.1f), darkMetalMat, false);
        CreateLocalCube(machine, "Art_Lever_Stem", new Vector3(1.08f, 1.05f, -0.38f), new Vector3(0.08f, 0.55f, 0.08f), cyanMat, false);
        CreateLocalSphere(machine, "Art_Lever_Knob", new Vector3(1.08f, 1.38f, -0.38f), new Vector3(0.2f, 0.2f, 0.2f), goldMat, false);

        AddWorldLabel(machine, "Lucky_Label", "LUCKY CORP", new Vector3(0f, 1.65f, -0.755f), 0.22f, Color.black);
    }

    private void BuildPlayerHands()
    {
        Transform parent = null;
        var hand = FindObject("Hand");
        if (hand)
            parent = hand.transform.parent;

        if (!parent)
        {
            var cam = Camera.main;
            if (cam)
                parent = cam.transform;
        }

        if (!parent || parent.Find("Art_Forearm"))
            return;

        CreateLocalCube(parent, "Art_Forearm", new Vector3(0.42f, -0.42f, 0.72f), new Vector3(0.18f, 0.2f, 0.55f), skinMat, false);
        CreateLocalCube(parent, "Art_Glove", new Vector3(0.42f, -0.34f, 1.02f), new Vector3(0.28f, 0.16f, 0.22f), gloveMat, false);
        CreateLocalCube(parent, "Art_WristBand", new Vector3(0.42f, -0.42f, 0.54f), new Vector3(0.24f, 0.24f, 0.08f), cyanMat, false);
        CreateLocalCube(parent, "Art_WristScreen", new Vector3(0.42f, -0.29f, 0.55f), new Vector3(0.16f, 0.025f, 0.1f), goldMat, false);
    }

    private void TuneLighting()
    {
        var volume = FindAnyObjectByType<Volume>();
        if (volume != null && volume.profile != null)
        {
            if (volume.profile.TryGet(out Bloom bloom))
            {
                bloom.active = true;
                bloom.intensity.Override(0.65f);
                bloom.threshold.Override(0.85f);
            }
        }

        var light = FindObject("Directional Light");
        if (light)
        {
            var dir = light.GetComponent<Light>();
            if (dir)
            {
                dir.intensity = 0.8f;
                dir.color = new Color(0.86f, 0.93f, 1f);
            }
        }
    }

    private void ApplyMaterial(string objectName, Material material)
    {
        var go = FindObject(objectName);
        if (go)
            ApplyRenderer(go, material);
    }

    private void ApplyRenderer(GameObject go, Material material)
    {
        var renderer = go.GetComponentInChildren<Renderer>();
        if (renderer)
            renderer.sharedMaterial = material;
    }

    private GameObject CreateCube(Transform parent, string name, Vector3 position, Vector3 scale, Material material, bool collider = true)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, true);
        go.transform.position = position;
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = scale;
        SetupPrimitive(go, material, collider);
        return go;
    }

    private GameObject CreateLocalCube(Transform parent, string name, Vector3 localPosition, Vector3 localScale, Material material, bool collider)
    {
        if (parent.Find(name))
            return parent.Find(name).gameObject;

        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = localScale;
        SetupPrimitive(go, material, collider);
        return go;
    }

    private GameObject CreateLocalSphere(Transform parent, string name, Vector3 localPosition, Vector3 localScale, Material material, bool collider)
    {
        if (parent.Find(name))
            return parent.Find(name).gameObject;

        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = localScale;
        SetupPrimitive(go, material, collider);
        return go;
    }

    private void SetupPrimitive(GameObject go, Material material, bool collider)
    {
        var renderer = go.GetComponent<Renderer>();
        if (renderer)
            renderer.sharedMaterial = material;

        if (!collider)
        {
            var col = go.GetComponent<Collider>();
            if (col)
                Destroy(col);
        }
    }

    private void CreatePointLight(Transform parent, string name, Vector3 position, Color color, float intensity, float range)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, true);
        go.transform.position = position;

        var light = go.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
    }

    private void AddWorldLabel(Transform parent, string name, string value, Vector3 localPosition, float fontSize, Color color)
    {
        if (parent.Find(name))
            return;

        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        go.transform.localScale = Vector3.one;

        var text = go.AddComponent<TextMeshPro>();
        text.text = value;
        text.fontSize = fontSize;
        text.alignment = TextAlignmentOptions.Center;
        text.color = color;
        text.rectTransform.sizeDelta = new Vector2(1.8f, 0.35f);
    }

    private Material CreateMaterial(string name, Color baseColor, Color emission, float emissionPower, float metallic, float smoothness)
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

        if (mat.HasProperty("_EmissionColor") && emissionPower > 0f)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emission * emissionPower);
        }

        if (mat.HasProperty("_Metallic"))
            mat.SetFloat("_Metallic", metallic);

        if (mat.HasProperty("_Smoothness"))
            mat.SetFloat("_Smoothness", smoothness);

        return mat;
    }

    private static GameObject FindObject(string name)
    {
        var transforms = Resources.FindObjectsOfTypeAll<Transform>();
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i].name == name && transforms[i].gameObject.scene.IsValid())
                return transforms[i].gameObject;
        }

        return null;
    }
}
