using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using DevTest;
public class TakeScreenshotURP : MonoBehaviour
{
    private bool takeScreenshot;

    public List<GameObject> sceneObjects;
    public List<InventoryItemData> dataObjects;
    [SerializeField] public string pathFolder = "Icons";
    [SerializeField] int iconSize = 512;
    [SerializeField] Color BackgroundColor;
    [SerializeField] bool TransparentBackground = false;



    public void TakeScreenshot(string fullpath)
    {
        if (TransparentBackground)
            Camera.main.backgroundColor = Color.black;
        else
            Camera.main.backgroundColor = BackgroundColor;
        if (takeScreenshot)
        {
            takeScreenshot = false;
        }
        int width = iconSize;
        int height = iconSize;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect((1920 - width) / 2, (1080 - height) / 2, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);

        if (TransparentBackground)
        {
            Color[] pixels = screenshotTexture.GetPixels(0, 0, screenshotTexture.width, screenshotTexture.height);
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i] == Color.black)
                {
                    pixels[i] = Color.clear;
                }
            }
            screenshotTexture.SetPixels(0, 0, screenshotTexture.width, screenshotTexture.height, pixels);
        }

        screenshotTexture.Apply();
        byte[] byteArray = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullpath, byteArray);
    }


    [ContextMenu("Screenshot")]
    private void ProcessScreenshots()
    {
        StartCoroutine(Screenshot());
    }

    private IEnumerator Screenshot()
    {
        for (int i = 0; i < sceneObjects.Count; i++)
        {
            GameObject obj = sceneObjects[i];
            InventoryItemData data = dataObjects[i];
            obj.gameObject.SetActive(true);

            yield return new WaitForEndOfFrame();

            if (!Directory.Exists($"{Application.dataPath}/{pathFolder}"))
            {
                Directory.CreateDirectory($"{Application.dataPath}/{pathFolder}");
                Debug.Log($"File path: {Application.dataPath}/{pathFolder} Created");
            }

            TakeScreenshot($"{Application.dataPath}/{pathFolder}/{data.DisplayName}_Icon.png");
            obj.gameObject.SetActive(false);

            AssetDatabase.Refresh();
            Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/{pathFolder}/{data.DisplayName}_Icon.png");
            if (s != null)
            {
                data.Icon = s;
                // EditorUtility.SetDirty(data);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TakeScreenshotURPV2))]
    class TakeScreenshotURPV2Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var TakeScreenshotURPV2 = (TakeScreenshotURPV2)target;
            if (TakeScreenshotURPV2 == null) return;

            if (GUILayout.Button("Shot"))
            {
                TakeScreenshotURPV2.ProcessScreenshots();
            }

        }
    }
#endif


}
