using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Controler_Base<View, Data> : MonoBehaviour where Data : Data_Base where View : View_Base<Data>
{
    /// <summary>
    /// View
    /// </summary>
    protected List<View> activeViews;
    /// <summary>
    /// model
    /// </summary>
    protected List<Data> datas;
    /// <summary>
    /// ?????
    /// </summary>
    protected List<View> pastViews;

    public UnityEvent<View> m_OnAddData = new UnityEvent<View>();
    public UnityEvent<View> m_OnRemoveData = new UnityEvent<View>();
    public UnityEvent<View> m_OnChangeData = new UnityEvent<View>();

    private void Awake()
    {
        activeViews = new List<View>();
        datas = new List<Data>();
        pastViews = new List<View>();
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    protected abstract void OnAwake();

    protected abstract void OnStart();

    #region View
    protected abstract View NewViewItem(Data data);

    //protected void DoAction_WithView(int index, System.Action<View> action)
    //{
    //    if (this.activeViews == null)
    //    {
    //        return;
    //    }
    //    var count = this.activeViews.Count;
    //    if (index < count && index > 0)
    //    {
    //        var view = activeViews[index];
    //        if (view != null)
    //        {
    //            action.Invoke(view);
    //        }
    //    }
    //}

    //protected void ShowView(int index)
    //{
    //    DoAction_WithView(index, ShowView);
    //}
    //protected void ShowView(View view)
    //{
    //    view.gameObject.SetActive(true);
    //    view.Refresh();
    //}

    //protected void HideViw(int index)
    //{
    //    DoAction_WithView(index, HideView);
    //}
    //protected void HideView(View view)
    //{
    //    view.gameObject.SetActive(false);
    //}
    #endregion

    #region Data
    protected void SetDatas(Data[] datas)
    {
        var length = -1;
        if (datas.Length < this.datas.Count)
        {
            length = datas.Length;
            var minIndex = length;
            var maxIndex = this.datas.Count;
            for (int i = maxIndex - 1; i >= minIndex; i--)
            {
                RemoveDataAt(i);
            }
        }
        else
        {
            length = this.datas.Count;
        }
        for (int i = 0; i < length; i++)
        {
            this.SetData(i, datas[i]);
        }
        var count = datas.Length;
        for (int i = length; i < count; i++)
        {
            AddData(datas[i]);
        }
    }
    private void AddData(Data data)
    {
        this.datas.Add(data);
        var view = this.NewViewItem(data);
        this.activeViews.Add(view);
        m_OnAddData?.Invoke(view);
    }
    private void RemoveData(Data data)
    {
        var index = this.datas.IndexOf(data);
        if (index >= 0 && index < this.datas.Count)
        {
            RemoveDataAt(index);
        }
    }
    private void RemoveDataAt(int index)
    {
        var data = this.datas[index];
        this.datas.RemoveAt(index);
        var view = activeViews.Find((m_view) =>
        {
            return m_view.GetData() == data;
        });
        if (view != null)
        {
            view.ResetData();
            m_OnRemoveData?.Invoke(view);
        }
    }
    private void SetData(int index, Data data)
    {
        if (this.datas.Count > index && index >= 0)
        {
            var oldData = this.datas[index];
            this.datas[index] = data;
            var view = activeViews.Find((m_view) =>
            {
                return m_view.GetData() == oldData;
            });
            if (view != null)
            {
                view.SetData(data);
                m_OnChangeData?.Invoke(view);
            }
        }
    }
    #endregion
}
