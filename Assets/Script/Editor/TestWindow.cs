using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using UnityEngine.AddressableAssets;

public class TestWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/TestWindow")]
    public static void ShowExample()
    {
        UnityEngine.Debug.Log(Application.persistentDataPath);
        UnityEngine.Debug.Log(Application.dataPath);
        var DirName = "Poco";
        var currentPath = Application.dataPath;
        var sourcePath = System.IO.Path.Combine(currentPath.Remove(currentPath.Length - "Assets".Length), DirName);
        var targetPath = System.IO.Path.Combine(currentPath, "Poco");
        UnityEngine.Debug.Log(sourcePath);
        UnityEngine.Debug.Log(targetPath);
        TestWindow wnd = GetWindow<TestWindow>();
        wnd.titleContent = new GUIContent("TestWindow");
        UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("").Completed += TestWindow_Completed; ;
    }

    private static void TestWindow_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {

    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Script/Editor/TestWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Script/Editor/TestWindow.uss");
        //VisualElement labelWithStyle = new Label("Hello World! With Style");
        //labelWithStyle.styleSheets.Add(styleSheet);
        //root.Add(labelWithStyle);

        // Get a reference to the field from UXML and append to it its value.
        var uxmlField = root.Q<TextField>("the-uxml-field");
        uxmlField.value += "..";

        // Create a new field, disable it, and give it a style class.
        var csharpField = new TextField("C# Field");
        csharpField.value = "It's snowing outside...";
        csharpField.SetEnabled(false);
        csharpField.AddToClassList("some-styled-field");
        csharpField.value = uxmlField.value;
        root.Add(csharpField);

        // Mirror value of uxml field into the C# field.
        uxmlField.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            csharpField.value = evt.newValue;
        });

        root.Add(new ObjectField());
    }


    [MenuItem("Test/ReImport")]
    private static void ReImport()
    {
        //AssetDatabase.Refresh();
        AssetDatabase.ImportAsset("Assets/Script", ImportAssetOptions.Default);
        Debug.Log("资源导入完毕");
    }

    [MenuItem("Test/Print")]
    private static void Print()
    {
        Debug.Log("print");

#if My
        Debug.Log(typeof(My).FullName);
#endif        
    }

    private static string sourceDirName = @"F:\UnityProject\TestUIToolKit\My";
    private static string destDirName = @"F:\UnityProject\TestUIToolKit\Assets\My";
    [MenuItem("Test/del")]
    private static void m_DelDir()
    {
        DelDir(destDirName);

        AssetDatabase.Refresh();
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, "");
    }
    [MenuItem("Test/copy")]
    private static void m_CopyDir()
    {
        CopyDir(sourceDirName, destDirName);

    }

    private static void DelDir(string path)
    {
        if (System.IO.Directory.Exists(path))
        {
            System.IO.Directory.Delete(path, true);
        }
    }
    public static bool CopyDir(string sourceDirName, string destDirName)
    {
        try
        {
            if (sourceDirName.Substring(sourceDirName.Length - 1) != "\\")
            {
                sourceDirName = sourceDirName + "\\";
            }
            if (destDirName.Substring(destDirName.Length - 1) != "\\")
            {
                destDirName = destDirName + "\\";
            }

            if (Directory.Exists(sourceDirName))
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }
                foreach (string item in Directory.GetFiles(sourceDirName))
                {
                    File.Copy(item, destDirName + Path.GetFileName(item), true);
                }
                foreach (string item in Directory.GetDirectories(sourceDirName))
                {
                    sourceDirName = item;
                    destDirName = destDirName + item.Substring(item.LastIndexOf("\\") + 1);
                    CopyDir(sourceDirName, destDirName);
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }


    [MenuItem("Test/Export")]
    private static void Build()
    {
        var scenes = new string[] { "Assets/Scenes/TestUIToolkit.unity" };
        var locationPathName = $"C:/Users/user/Desktop/Export/{System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}";
        var options = BuildOptions.DetailedBuildReport;
        EditorUserBuildSettings.buildAppBundle = true;
        BuildPipeline.BuildPlayer(scenes, locationPathName, BuildTarget.Android, options);
    }
}