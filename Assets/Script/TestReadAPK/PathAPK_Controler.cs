using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;

public class PathAPK_Controler : Path_Controler
{
    private string m_Root;
    private ZipArchive m_ZipArchive;

    protected override void OnStart()
    {
        this.m_Root = Application.dataPath;
        this.m_ZipArchive = ZipFile.Open(m_Root, ZipArchiveMode.Read);
        base.OnStart();
    }

    protected override string GetParentPath()
    {
        return m_Root;
    }

    protected override string GetCurrentPath()
    {
        return m_CurrentPath;
    }

    protected override string[] GetDirs(string dir)
    {
        var entries = m_ZipArchive.Entries;
        var length = entries.Count;
        var dirs = new List<string>();
        for (int i = 0; i < length; i++)
        {
            var entry = entries[i];
            if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\"))
            {
                dirs.Add(entry.FullName);
            }
        }

        return dirs.ToArray();
    }

    protected override string[] GetFiles(string dir)
    {
        var entries = m_ZipArchive.Entries;
        var length = entries.Count;
        var files = new List<string>();
        for (int i = 0; i < length; i++)
        {
            var entry = entries[i];
            if (!entry.FullName.EndsWith("/") && !entry.FullName.EndsWith("\\"))
            {
                files.Add(entry.FullName);
            }

            //temp code write ini
            if (entry.FullName.EndsWith(".ini"))
            {
                var stream = entry.Open();
                var sw = System.IO.File.OpenWrite(System.IO.Path.Combine(Application.persistentDataPath, entry.Name));
                stream.CopyTo(sw);
                sw.Flush();
                sw.Close();
            }

            //temp code change apk
            if (entry.FullName.EndsWith(".ini"))
            {
                this.StartCoroutine(MoveStreamingAssetsFile(entry.Name));
            }
        }

        return files.ToArray();
    }

    private IEnumerator MoveStreamingAssetsFile(string fileName)
    {
        var source = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        var dest = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        var web = UnityWebRequest.Get(source);
        yield return web.SendWebRequest();
        try
        {
            if (web.isDone)
            {
                //System.IO.File.WriteAllBytes(dest, web.downloadHandler.data);
                System.IO.File.Delete(source);
                if (System.IO.File.Exists(dest))
                {
                    System.IO.File.Delete(dest);
                }
            }
            else
            {
                UnityEngine.Debug.LogError(web.error);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.ToString());
        }
    }
}