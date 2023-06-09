using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PathItem_View : View_Base<PathItemData>
{
    private Button btn_PathItem;
    private Text text_Text;
    private Image img_Icon;
    private Image img_Folder;
    private Text text_Desc;
    public UnityEvent<PathItem_View> m_OnClickBtin = new UnityEvent<PathItem_View>();

    protected override void OnInit()
    {
        btn_PathItem = this.GetComponentWithName<Button>(nameof(btn_PathItem));
        text_Text = this.GetComponentWithName<Text>(nameof(text_Text));
        img_Icon = this.GetComponentWithName<Image>(nameof(img_Icon));
        img_Folder = this.GetComponentWithName<Image>(nameof(img_Folder));
        text_Desc = this.GetComponentWithName<Text>(nameof(text_Desc));

        btn_PathItem.onClick.AddListener(OnClickBtin);
    }

    protected override void OnHide()
    {
        Debug.Log(nameof(OnHide));
    }

    protected override void OnRefresh()
    {
        if (this.data == null)
        {
            return;
        }
        RefreshText();
        RefreshIcon();
        RefreshFolder();
        RefreshDesc();
    }

    protected override void OnShow()
    {
        Debug.Log(nameof(OnShow));
    }

    private void RefreshText()
    {
        var path = this.data.m_TextPath;
        if (!string.IsNullOrEmpty(path))
        {
            var last = this.data.m_Prefix + new System.IO.DirectoryInfo(path).Name;
            text_Text.text = last;
        }
        else
        {
            text_Text.text = this.data.m_Prefix;
        }
    }

    private void RefreshDesc()
    {
        text_Desc.text = this.data.m_TextPath;
    }

    private void RefreshIcon()
    {
        if (!string.IsNullOrEmpty(this.data.m_IconPath))
        {
            img_Icon.sprite = Resources.Load<Sprite>(this.data.m_IconPath);
        }
    }

    private void RefreshFolder()
    {
        if (img_Folder.gameObject.activeInHierarchy != this.data.m_IsFolder)
        {
            img_Folder.gameObject.SetActive(this.data.m_IsFolder);
        }
    }

    private void OnClickBtin()
    {
        m_OnClickBtin?.Invoke(this);
    }
}

[System.Serializable]
public class PathItemData : Data_Base
{
    public string m_IconPath;
    public string m_Prefix;
    public string m_TextPath;
    public bool m_IsFolder;
}