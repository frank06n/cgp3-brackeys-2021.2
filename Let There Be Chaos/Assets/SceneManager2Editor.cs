//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(SceneManager2))]
public class SceneManager2Editor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Clean Hiearchy"))
        {
            Transform Platforms, EnemyTurrets, Collectibles, HelpTexts;
            GameObject[] root_objs = SceneManager.GetActiveScene().GetRootGameObjects();

            Platforms = EnemyTurrets = Collectibles = HelpTexts = null;

            foreach (GameObject obj in root_objs)
            {
                switch (obj.name)
                {
                    case "Platforms":
                        Platforms = obj.transform;
                        break;
                    case "EnemyTurrets":
                        EnemyTurrets = obj.transform;
                        break;
                    case "Collectibles":
                        Collectibles = obj.transform;
                        break;
                    case "HelpTexts":
                        HelpTexts = obj.transform;
                        break;
                }
            }

            foreach (GameObject obj in root_objs)
            {
                if (obj.CompareTag("Platform"))
                {
                    obj.transform.SetParent(Platforms);
                }
                else if (obj.CompareTag("EnemyTurret"))
                {
                    obj.transform.SetParent(EnemyTurrets);
                }
                else if (obj.GetComponent<CollectibleLogic>() != null)
                {
                    obj.transform.SetParent(Collectibles);
                }
                else if (obj.GetComponent<TextMeshPro>() != null)
                {
                    obj.transform.SetParent(HelpTexts);
                }

            }
        }


        if (GUILayout.Button("Show Canvas"))
        {
            foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (obj.name=="Canvas")
                {
                    obj.SetActive(true);
                    break;
                }
            }
        }
        if (GUILayout.Button("Hide Canvas"))
        {
            FindObjectOfType<Canvas>().gameObject.SetActive(false);
        }
    }
}