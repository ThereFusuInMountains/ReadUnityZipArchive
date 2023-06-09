using UnityEngine.Events;
using UnityEngine;

public abstract class View_Base<Data> : MonoBehaviour where Data : Data_Base
{
    protected Data data;
    public UnityEvent<View_Base<Data>> m_OnShow = new UnityEvent<View_Base<Data>>();
    public UnityEvent<View_Base<Data>> m_OnHide = new UnityEvent<View_Base<Data>>();

    public void Show()
    {
        this.gameObject.SetActive(true);
        OnShow();
        m_OnShow?.Invoke(this);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
        OnHide();
        m_OnHide?.Invoke(this);
    }
    public void Refresh(Data data = default(Data))
    {
        SetData(data);
        OnRefresh();
    }

    public Data GetData()
    {
        return data;
    }
    public void SetData(Data data)
    {
        if (data != null)
        {
            this.data = data;
        }
    }
    public void ResetData()
    {
        data = null;
    }

    protected Component GetComponentWithName<Component>(string name) where Component : UnityEngine.Component
    {
        return GetComponentWithName<Component>(this.transform, name);
    }
    protected Component GetComponentWithName<Component>(Transform transform, string name) where Component : UnityEngine.Component
    {
        if (transform.name == name && transform.TryGetComponent<Component>(out var component))
        {
            return component;
        }
        var count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = transform.GetChild(i);
            component = GetComponentWithName<Component>(child, name);
            if (component != null)
            {
                return component;
            }
        }
        return null;
    }

    private void Awake()
    {
        OnInit();
    }
    private void Start()
    {

    }
    protected abstract void OnInit();

    protected abstract void OnShow();
    protected abstract void OnHide();
    protected abstract void OnRefresh();
}
