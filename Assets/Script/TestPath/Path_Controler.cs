using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;


public class Path_Controler : Controler_Base<PathItem_View, PathItemData>
{
    protected string m_CurrentPath;
    private PathItem_View preView;
    private string[] default_Paths;
    private string[] default_Prefix;
    [SerializeField] private PathItemData[] data;

    protected override void OnAwake()
    {
        this.m_OnAddData.AddListener(OnAddData);
        this.m_OnRemoveData.AddListener(OnRemoveData);
        this.m_OnChangeData.AddListener(OnChangeData);
    }

    /*
     0.上级目录
     1.当前目录
     2.Application.dataPath
     3.Application.temporaryCachePath
     4.Application.streamingAssetsPath
     5.Application.persistentDataPath
     6.Application.consoleLogPath
     */

    protected override void OnStart()
    {
        m_CurrentPath = System.IO.Path.GetDirectoryName(Application.dataPath);
        preView = this.GetComponentInChildren<PathItem_View>();
        preView.gameObject.SetActive(false);
        default_Paths = new string[]
        {
            "父目录",
            "当前目录",
            Application.dataPath,
            Application.temporaryCachePath,
            Application.streamingAssetsPath,
            Application.persistentDataPath,
            Application.consoleLogPath,
            Path.Combine(Application.dataPath, "assets"),
        };
        default_Prefix = new string[]
        {
            "父目录:",
            "当前目录:",
            "dataPath:",
            "temporaryCachePath:",
            "streamingAssetsPath:",
            "persistentDataPath:",
            "consoleLogPath:",
            "dataPath/assets:",
        };
        Refresh();
    }

    protected override PathItem_View NewViewItem(PathItemData data)
    {
        var view = default(PathItem_View);
        if (pastViews.Count > 0)
        {
            view = pastViews[pastViews.Count - 1];
        }

        if (view == null)
        {
            view = GameObject.Instantiate<GameObject>(this.preView.gameObject).GetComponent<PathItem_View>();
            view.name = this.preView.name;
            view.transform.SetParent(this.preView.transform.parent);
            view.transform.localScale = Vector3.one;
            view.m_OnClickBtin.AddListener(OnClick);
            view.m_OnShow.AddListener(OnShowView2);
            view.m_OnHide.AddListener(OnHideView2);
            view.Show();
        }

        view.SetData(data);
        return view;
    }

    protected virtual string GetParentPath()
    {
        return new System.IO.DirectoryInfo(m_CurrentPath).Parent.FullName;
    }

    protected virtual string GetCurrentPath()
    {
        return new System.IO.DirectoryInfo(m_CurrentPath).FullName;
    }

    protected virtual string[] GetDirs(string dir)
    {
        return System.IO.Directory.GetDirectories(dir);
    }

    protected virtual string[] GetFiles(string dir)
    {
        return System.IO.Directory.GetFiles(dir);
    }

    private void Refresh()
    {
        var dir = GetCurrentPath();
        var dirs = GetDirs(dir);
        var files = GetFiles(dir);
        //父目录
        default_Paths[0] = GetParentPath();
        //当前目录
        default_Paths[1] = dir;
        data = new PathItemData[default_Paths.Length + dirs.Length + files.Length];
        var count = 0;
        //默认路径选项
        var length = default_Paths.Length;
        for (int i = 0; i < length; i++)
        {
            data[i + count] = new PathItemData()
            {
                m_Prefix = default_Prefix[i],
                m_IconPath = "dir",
                m_IsFolder = true,
                m_TextPath = default_Paths[i],
            };
        }

        //当前目录中的文件夹
        count = count + default_Paths.Length;
        length = dirs.Length;
        for (int i = 0; i < length; i++)
        {
            data[i + count] = new PathItemData()
            {
                m_IconPath = "dir",
                m_IsFolder = true,
                m_TextPath = dirs[i],
            };
        }

        //当前目录中的文件
        count = count + dirs.Length;
        length = files.Length;
        for (int i = 0; i < length; i++)
        {
            data[i + count] = new PathItemData()
            {
                m_IconPath = string.Empty,
                m_IsFolder = false,
                m_TextPath = files[i],
            };
        }

        try
        {
            SetDatas(data);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void OnClick(PathItem_View view)
    {
        this.m_CurrentPath = view.GetData().m_TextPath;
        Refresh();
    }

    private void OnAddData(PathItem_View view)
    {
        view.Show();
        view.Refresh();
    }

    private void OnRemoveData(PathItem_View view)
    {
        view.Hide();
    }

    private void OnChangeData(PathItem_View view)
    {
        view.Refresh();
    }

    private void OnHideView2(View_Base<PathItemData> view)
    {
    }

    private void OnShowView2(View_Base<PathItemData> view)
    {
    }
}