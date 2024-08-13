using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class MeshTransferEditor : EditorWindow
{
    //Written by Doga Uslu, please do not distribute without permission

    GameObject sourceHierarchy;
    GameObject targetRig;
    GameObject rootObject; // Add reference to the root object
    string materialsPath;
    string assetPath;
    string texturesPath;

    private bool createMaterials = false;
    private Shader shaderForMaterials;
    private Material defaultMaterial; // For resetting materials

    private bool isProPixelizer = false;
    private bool applyBoneRotation = true;

    private bool nameHasPrefix = false;
    private bool nameHasSuffix = false;
    string jointName;
    private string[] directionalSuffixes = { "_r", "_l" };
    public bool usesDynamicBone;

    // GUI Input Fields (simplified example)
    string headKeywords = "monocle,hair,hat"; // User-inputted in the GUI
    string waistKeywords = "belt,waist,sash,holster";


    // Internal Dictionary
    Dictionary<string, string> keywordToJointMap = new Dictionary<string, string>
    {
        { "hair", "Head" },
        { "hairclip", "head" },
        { "veil", "head" },
        { "eyepatch", "head" },
        { "hat", "Head" },
        { "beard", "Head" },
        { "moustache", "Head" },
        { "eyebrow", "Head" },
        { "nose", "Head" },
        { "monocle", "Head" },
        { "eyemask", "Head" },
        { "chest" , "spine3" },
        { "scarf" ,  "neck"},
        { "necklace_base", "neck" },
        { "necklace_end","spine3" },
        { "glove-r", "hand_r" },
        { "glove-l", "hand_l" },
        { "shoe1_r", "foot_r" },
        { "shoe1_l", "foot_l" },
        { "shoe2_r", "toes1_r" },
        { "shoe2_l", "toes1_l" }
        // Add more mappings as needed
};

    [MenuItem("Tools/Mesh Transfer")]
    public static void ShowWindow()
    {

        GetWindow<MeshTransferEditor>("Mesh Transfer");
    }

    void OnGUI()
    {
        GUILayout.Label("Transfer Meshes to Rig", EditorStyles.boldLabel);

        sourceHierarchy = (GameObject)EditorGUILayout.ObjectField("Source Hierarchy", sourceHierarchy, typeof(GameObject), true);
        targetRig = (GameObject)EditorGUILayout.ObjectField("Target Rig", targetRig, typeof(GameObject), true);
        rootObject = (GameObject)EditorGUILayout.ObjectField("Root Object", rootObject, typeof(GameObject), true); // Add root object field
        materialsPath = EditorGUILayout.TextField("Materials Path", materialsPath);
        createMaterials = EditorGUILayout.Toggle("Create Materials", createMaterials);
        GUI.enabled = createMaterials; // Enable the shader field only if createMaterials is checked
        shaderForMaterials = (Shader)EditorGUILayout.ObjectField("Shader for Materials", shaderForMaterials, typeof(Shader), false);
        GUI.enabled = true; // Re-enable GUI for the following elements

        nameHasPrefix = EditorGUILayout.Toggle("Name Has Prefix", nameHasPrefix);
        usesDynamicBone = EditorGUILayout.Toggle("Uses Dynamic Bone", usesDynamicBone);
        applyBoneRotation = EditorGUILayout.Toggle("Apply Bone Rotation", applyBoneRotation);


        texturesPath = EditorGUILayout.TextField("Textures Path", texturesPath);// GUI for specifying the textures path

        if (GUILayout.Button("Transfer Meshes"))
        {
            TransferMeshes();
        }
        if (GUILayout.Button("Release Meshes"))
        {
            ReleaseMeshes();
        }
        if (GUILayout.Button("Create Materials"))
        {
            CreateMaterials();
        }
        if (createMaterials)
        {
            isProPixelizer = EditorGUILayout.Toggle("Is ProPixelizer", isProPixelizer);
        }
        if (GUILayout.Button("Assign Materials"))
        {
            AssignMaterials();
        }
        if (GUILayout.Button("Release Materials"))
        {
            ReleaseMaterials();
        }

    }

    void InitializeKeywordToJointMap()
    {
        keywordToJointMap = new Dictionary<string, string>();
        foreach (var pair in new[] { (headKeywords, "HeadJoint"), (waistKeywords, "WaistJoint") })
        {
            foreach (var keyword in pair.Item1.Split(','))
            {
                if (!keywordToJointMap.ContainsKey(keyword.Trim()))
                    keywordToJointMap.Add(keyword.Trim(), pair.Item2);
            }
        }
    }

    void AssignMeshToJointBasedOnKeyword(Transform meshTransform)
    {
        string meshName = meshTransform.name.ToLower();
        foreach (var entry in keywordToJointMap)
        {
            if (meshName.Contains(entry.Key))
            {
                Transform targetJoint = FindDeepChild(targetRig.transform, entry.Value);
                if (targetJoint != null)
                {
                    // Logic to set meshTransform parent to targetJoint...
                    break; // Assuming one keyword match is enough
                }
            }
        }
    }
    void TransferMeshes()
    {
        if (sourceHierarchy == null || targetRig == null || rootObject == null)
        {
            Debug.LogError("Source Hierarchy, Target Rig, and Root Object must be set");
            return;
        }

        foreach (Transform sourceTransform in sourceHierarchy.GetComponentsInChildren<Transform>())
        {
            string meshName = sourceTransform.gameObject.name.ToLower(); // Convert to lowercase for case-insensitive matching

            // Skip the sourceHierarchy object itself to exclude it from processing
            if (sourceTransform == sourceHierarchy.transform) continue;

            // Extract joint name using the new method
            string jointName = ExtractJointName(meshName);

            Transform targetJoint = FindDeepChild(targetRig.transform, jointName);

            // Proceed with mesh transformation and parenting if a target joint is found
            if (targetJoint != null)
            {
                GameObject meshObject = sourceTransform.gameObject;
                meshObject.transform.SetParent(targetJoint, false);

                // Compensate for the parent's scale
                Vector3 inverseScaleFactor = new Vector3(
                    1f / targetJoint.lossyScale.x,
                    1f / targetJoint.lossyScale.y,
                    1f / targetJoint.lossyScale.z);
                meshObject.transform.localScale = Vector3.Scale(meshObject.transform.localScale, inverseScaleFactor);

                meshObject.name += "-m"; // Append the "-m" suffix
                if (usesDynamicBone)
                {
                    HandleDynamicBoneColliders(meshObject, meshName, jointName);
                }
                ApplyRotationBasedOnName(meshObject, meshName, jointName);
                // Optionally, here you can save this meshObject as a prefab
                string assetPath = "Assets/MeshTemp/" + meshObject.name + ".prefab";
                PrefabUtility.SaveAsPrefabAssetAndConnect(meshObject, assetPath, InteractionMode.AutomatedAction);

                Debug.Log($"Mesh transferred and assigned to joint: {jointName} ({meshName})");
            }
            else
            {
                Debug.LogWarning($"No matching joint found for: {meshName}.");
            }
        }
    }

    string ExtractJointName(string meshName)
    {
        // Split the name into parts
        string[] parts = meshName.Split('_');
        string jointName = parts.Length > 3 ? parts[3] : ""; // 4th part is the joint name

        // Check for left or right indicators in the 5th part and adjust
        if (parts.Length > 4)
        {
            if (parts[4] == "r" || parts[4] == "l")
            {
                jointName += "_" + parts[4];
            }
        }
        // Ignore any additional parts beyond this point
        return jointName;
    }
    //void TransferMeshes()
    //{
    //    if (sourceHierarchy == null || targetRig == null || rootObject == null)
    //    {
    //        Debug.LogError("Source Hierarchy, Target Rig, and Root Object must be set");
    //        return;
    //    }

    //    foreach (Transform sourceTransform in sourceHierarchy.GetComponentsInChildren<Transform>())
    //    {
    //        string meshName = sourceTransform.gameObject.name.ToLower(); // Convert to lowercase for case-insensitive matching

    //        // Skip the sourceHierarchy object itself to exclude it from processing
    //        if (sourceTransform == sourceHierarchy.transform) continue;

    //        Transform targetJoint = null;
    //        string jointName = "";

    //        // First, try keyword-based assignment
    //        foreach (var pair in keywordToJointMap)
    //        {
    //            if (meshName.Contains(pair.Key))
    //            {
    //                targetJoint = FindDeepChild(targetRig.transform, pair.Value);
    //                break; // Found a match, exit the loop
    //            }
    //        }

    //        // If no keyword match was found, use the original logic to determine the jointName
    //        if (targetJoint == null)
    //        {
    //            if (nameHasPrefix)
    //            {
    //                string[] parts = meshName.Split('_');
    //                if (parts.Length >= 3)
    //                {
    //                    jointName = parts[2]; // Base case for names with less complexity
    //                    if (parts.Length > 3)
    //                    {
    //                        // Assuming potential suffix or additional details beyond the base joint name
    //                        bool isLastPartSuffix = parts.Last().Length == 1 || new[] { "l", "r", "front", "back" }.Contains(parts.Last());
    //                        jointName = isLastPartSuffix ? String.Join("_", parts.Skip(3).Take(parts.Length - 4)) : String.Join("_", parts.Skip(3));
    //                        if (isLastPartSuffix)
    //                        {
    //                            jointName += "_" + parts.Last();
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                jointName = meshName; // Direct assignment for non-prefixed names
    //            }

    //            // Finding the joint based on the determined jointName
    //            targetJoint = FindDeepChild(targetRig.transform, jointName);
    //        }

    //        // Proceed with mesh transformation and parenting if a target joint is found
    //        if (targetJoint != null)
    //        {
    //            GameObject meshObject = sourceTransform.gameObject;
    //            meshObject.transform.SetParent(targetJoint, false);

    //            // Compensate for the parent's scale
    //            Vector3 inverseScaleFactor = new Vector3(
    //                1f / targetJoint.lossyScale.x,
    //                1f / targetJoint.lossyScale.y,
    //                1f / targetJoint.lossyScale.z);
    //            meshObject.transform.localScale = Vector3.Scale(meshObject.transform.localScale, inverseScaleFactor);

    //            meshObject.name += "-m"; // Append the "-m" suffix
    //            if(usesDynamicBone)
    //            {
    //                HandleDynamicBoneColliders(meshObject, meshName, jointName);

    //            }
    //            ApplyRotationBasedOnName(meshObject, meshName, jointName );
    //            // Optionally, here you can save this meshObject as a prefab
    //            string assetPath = "Assets/MeshTemp/" + meshObject.name + ".prefab";
    //            PrefabUtility.SaveAsPrefabAssetAndConnect(meshObject, assetPath, InteractionMode.AutomatedAction);

    //            Debug.Log($"Mesh transferred and assigned to joint: {jointName} ({meshName})");
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"No matching joint found for: {meshName}.");
    //        }
    //    }
    //}
    //string ExtractJointName(string meshName)
    //{
    //    // Logic to extract the part of the mesh name that corresponds to a joint
    //    // This example assumes your mesh name format and keywordToJointMap structure
    //    string jointName = "";
    //    foreach (var keyword in keywordToJointMap.Keys)
    //    {
    //        if (meshName.Contains(keyword))
    //        {
    //            jointName = keywordToJointMap[keyword];
    //            break; // Assuming first match is adequate
    //        }
    //    }
    //    return jointName;
    //}
    // Example implementation for FindDeepChild (based on your context)
    Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name.ToLower().Equals(name.ToLower()))
                return child;

            Transform result = FindDeepChild(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
    void HandleDynamicBoneColliders(GameObject meshObject, string meshName, string jointName)
    {
        // Regex to identify if _r or _l is a suffix
        var suffixRegex = new Regex(@"(_[rl])(?:$|[-]|-m)");
        var match = suffixRegex.Match(jointName);
        bool isRight = match.Success && match.Groups[1].Value == "_r";
        bool isLeft = match.Success && match.Groups[1].Value == "_l";

        // z9999 Uncomment this when DynamicBone is back in the project
        //if(this.GetComponent<DynamicBoneCollider>() == null)
        //{
        //    DynamicBoneCollider dynamicBoneCollider = this.AddComponent<DynamicBoneCollider>();
        
        //    if (meshName.Contains("_head"))
        //    {
        //        dynamicBoneCollider.m_Radius = 1f;
        //    }
        //}


    }
    // This method encapsulates the new keyword-based joint assignment
    Transform CheckForKeywordAndAssignJoint(string meshName)
    {
        foreach (var pair in keywordToJointMap)
        {
            if (meshName.ToLower().Contains(pair.Key))
            {
                return FindDeepChild(targetRig.transform, pair.Value);
            }
        }
        return null; // Return null if no keyword match is found
    }



    void CreateMaterials()
    {

        if (string.IsNullOrEmpty(materialsPath) || shaderForMaterials == null)
        {
            Debug.LogError("Materials Path and Shader for Materials must be set.");
            return;
        }

        foreach (Renderer renderer in sourceHierarchy.GetComponentsInChildren<Renderer>())
        {
            // Use the mesh name directly for the material name
            string meshName = renderer.gameObject.name;
            Material newMaterial = new Material(shaderForMaterials)
            {
                name = meshName // Directly use the mesh name
            };
            // Transfer ProPixelizer properties if applicable
            ApplyProPixelizerProperties(renderer.material, newMaterial);
            // Assign Albedo texture from specified textures path
            AssignAlbedoTexture(renderer.material, newMaterial);
            string materialPath = AssetDatabase.GenerateUniqueAssetPath($"{materialsPath}/{newMaterial.name}.mat");
            AssetDatabase.CreateAsset(newMaterial, materialPath);

            Debug.Log($"Material {newMaterial.name} created and saved at {materialPath}.");
        }
    }

    void AssignAlbedoTexture(Material original, Material newMaterial)
    {
        string textureName = original.name.Replace("-material", ""); // Assuming you're still removing "-material" suffix
        Texture2D albedoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturesPath + "/" + textureName + ".png"); // Assuming PNG format

        if (albedoTexture != null)
        {
            newMaterial.SetTexture("_MainTex", albedoTexture);
            newMaterial.SetColor("_EmissionColor", new Color(0f, 0f, 0f, 1f)); // Black

        }
        else
        {
            Debug.LogWarning("Albedo texture not found for: " + original.name);
        }
    }


    void ReleaseMaterials()
    {
        if (sourceHierarchy == null)
        {
            Debug.LogError("Source Hierarchy must be set");
            return;
        }

        foreach (Renderer renderer in sourceHierarchy.GetComponentsInChildren<Renderer>())
        {
            Material[] resetMaterials = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < resetMaterials.Length; i++)
            {
                resetMaterials[i] = defaultMaterial; // Set to default material or implement other logic
            }
            renderer.sharedMaterials = resetMaterials;
        }

        Debug.Log("Materials released.");
    }

    void ApplyProPixelizerProperties(Material original, Material newMaterial)
    {
        if (original.HasProperty("_BaseMap"))
        {
            Texture albedoTexture = original.GetTexture("_BaseMap");
            newMaterial.SetTexture("_Albedo", albedoTexture);
            newMaterial.SetColor("_EmissionColor", new (0,0,0,0));
        }
    }

    Vector3 CalculateCumulativeParentScale(Transform transform)
    {
        Vector3 cumulativeScale = Vector3.one;
        Transform current = transform.parent;

        while (current != null)
        {
            cumulativeScale = Vector3.Scale(cumulativeScale, current.localScale);
            current = current.parent;
        }

        return cumulativeScale;
    }

    void ApplyRotationBasedOnSuffix(GameObject meshObject, string suffix)
    {
        Vector3 rotation = suffix == "r" ? new Vector3(0, 90, 90) : new Vector3(0, -90, -90);
        meshObject.transform.localRotation = Quaternion.Euler(rotation);
    }

    void ApplyRotationBasedOnName(GameObject meshObject, string meshName, string jointName)
    {
        if (applyBoneRotation)
        {

            // Regex to identify if _r or _l is a suffix
            var suffixRegex = new Regex(@"(_[rl])(?:$|[-]|-m)");
            var match = suffixRegex.Match(jointName);
            bool isRight = match.Success && match.Groups[1].Value == "_r";
            bool isLeft = match.Success && match.Groups[1].Value == "_l";

            Vector3 rotation = Vector3.zero;//Default rot

            // --------------------------------------------------------------------------------------------------
            // will apply regex to !nameHasPrefix later cause not priority,
            // from now on everything I do will probably have prefix
            // --------------------------------------------------------------------------------------------------
            if (!nameHasPrefix)
            {
                // Rotation logic based on name keywords

                //if (meshName.Contains("upperarm_") || meshName.Contains("forearm_") || meshName.Contains("shoulder_"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(0, 90, 90) : new Vector3(0, -90, -90);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
                //else if (meshName.Contains("thigh_") || meshName.Contains("leg_"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(0, 90, 180) : new Vector3(0, -90, 180);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
                //else if (meshName.Contains("foot_"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(-60, 0, 180) : new Vector3(-60, 0, -180);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
                //else if (meshName.Contains("hand_"))
                //{
                //    // Right hand rotation is weird, thus the 66 on Y, something messes up between MB Lab and AutoRig Pro
                //    // Update on the following, seems fixed for now but keeping the comment above just in case
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(0, 90, 90) : new Vector3(0, -90, -90);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
                //else if (meshName.Contains("thumb"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(-90, 0, 90) : new Vector3(-90, 0, 0);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
                //else if (meshName.Contains("index"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
                //else if (meshName.Contains("middle"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
                //else if (meshName.Contains("ring"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
                //else if (meshName.Contains("pinky"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}

                //else if (meshName.Contains("foot_"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
                //else if (meshName.Contains("toes1_"))
                //{
                //    Vector3 rotation = meshName.Contains("_r") ? new Vector3(-90, 0, 0) : new Vector3(-90, 0, 0);
                //    meshObject.transform.localRotation = Quaternion.Euler(rotation);
                //}
            }
            // --------------------------------------------------------------------------------------------------

            else
            {
                {
                    if (meshName.Contains("_upperarm_") || meshName.Contains("_forearm_") || meshName.Contains("_shoulder_"))
                    {
                        rotation = isRight ? new Vector3(0, 90, 90) : new Vector3(0, -90, -90);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(0, 90, 90) : new Vector3(0,-90,-90);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_thigh_") || meshName.Contains("_leg_"))
                    {
                        rotation = isRight ? new Vector3(0, 90, 180) : new Vector3(0, -90, -180);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(0, 90, 180) : new Vector3(0, -90, -180);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_foot_"))
                    {
                        rotation = isRight ? new Vector3(-60, 0, 180) : new Vector3(-60, 0, -180);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(-60, 0, 180) : new Vector3(-60, 0, -180);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_hand_"))
                    {
                        rotation = isRight ? new Vector3(0, 90, 90) : new Vector3(0, -90, -90);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        // Right hand rotation is weird, thus the 66 on Y, something messes up between MB Lab and AutoRig Pro
                        // Update on the following, seems fixed for now but keeping the comment above just in case
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(0, 90, 90) : new Vector3(0, -90, -90);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_thumb"))
                    {
                        rotation = isRight ? new Vector3(-90, 0, 90) : new Vector3(-90, 0, -90);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(-90, 0, 90) : new Vector3(-90, 0, -90);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_index"))
                    {
                        rotation = isRight ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_middle"))
                    {
                        rotation = isRight ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_ring"))
                    {
                        rotation = isRight ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_pinky"))
                    {
                        rotation = isRight ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_foot_"))
                    {
                        rotation = isRight ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                    else if (meshName.Contains("_toes1_"))
                    {
                        rotation = isRight ? new Vector3(-90, 0, 180) : new Vector3(-90, 0, 180);
                        meshObject.transform.localRotation = Quaternion.Euler(rotation);
                        //Vector3 rotation = jointName.Contains("_r") ? new Vector3(-90, 0, 180) : new Vector3(-90, 0,180);
                        //meshObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                }
            }
        }
    }
    void ReleaseMeshes()
    {
        if (sourceHierarchy == null || targetRig == null)
        {
            Debug.LogError("Source Hierarchy and Target Rig must be set");
            return;
        }

        foreach (Transform child in targetRig.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.EndsWith("-m"))
            {
                GameObject meshObject = child.gameObject;
                meshObject.name = meshObject.name.Replace("-m", ""); // Remove "-m" suffix
                meshObject.transform.SetParent(sourceHierarchy.transform, false);
                meshObject.transform.localScale = Vector3.one;
                meshObject.transform.localRotation = Quaternion.identity;
                Debug.Log("Mesh released: " + meshObject.name);
            }
        } 
    }
    void AssignMaterials()
    {
        if (sourceHierarchy == null || string.IsNullOrEmpty(materialsPath))
        {
            Debug.LogError("Parts GameObject and Materials Path must be set");
            return;
        }

        foreach (Transform child in sourceHierarchy.transform)
        {
            string partName = child.gameObject.name;
            Material partMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialsPath + "/" + partName + ".mat");

            if (partMaterial != null && child.GetComponent<Renderer>())
            {
                child.GetComponent<Renderer>().material = partMaterial;
                Debug.Log("Material assigned to: " + partName);
            }
            else
            {
                Debug.LogWarning("No matching material found for: " + partName);
            }
        }
    }



}