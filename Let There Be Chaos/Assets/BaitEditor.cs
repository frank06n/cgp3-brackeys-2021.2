//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bait))]
public class BaitEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Newfy"))
        {
            Transform platform = ((Bait)target).transform;
            Transform topedge = platform.GetChild(0);
            Transform platform_newg = platform.GetChild(1);

            SpriteRenderer org_sr = platform.GetComponent<SpriteRenderer>();
            SpriteRenderer newg_sr = platform_newg.GetComponent<SpriteRenderer>();

            BoxCollider2D bc = platform.GetComponent<BoxCollider2D>();

            float factor = platform_newg.localScale.x;

            platform.localScale = new Vector3(5 * factor, 5 * factor, 1);
            topedge.localScale = new Vector3(topedge.localScale.x / factor, 1, 1);
            topedge.localPosition = topedge.localPosition / factor;

            org_sr.sprite = newg_sr.sprite;
            bc.size = bc.size / factor;

            org_sr.enabled = true;

            DestroyImmediate(platform_newg.gameObject);
            DestroyImmediate(target);
        }
    }
}