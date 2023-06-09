using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

[DisallowMultipleComponent]
public class TestUIToolkit : MonoBehaviour
{
    [SerializeField]
    private VisualElement m_Root;
    [SerializeField]
    public UIDocument parentUI;

    private void Start()
    {
        parentUI = this.gameObject.AddComponent<UIDocument>();
        //parentUI.visualTreeAsset = VisualTreeAsset.CreateInstance<VisualTreeAsset>();
        m_Root = parentUI.rootVisualElement;
        var UXML = Resources.Load<VisualTreeAsset>("Test_UXML").Instantiate();
        var styleSheet = Resources.Load<StyleSheet>("Test_USS");
        UXML.styleSheets.Add(styleSheet);
        m_Root.Add(UXML);
    }
}
