using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


/// <summary>
/// 协程与多线程的测试
/// 协程可能会卡住主线程
/// 多线程不会
/// 多线程不能访问UnityEngine API
/// </summary>
public class CoroutinTest : MonoBehaviour
{
    public Text m_TextTime;
    public Text m_TextByteCount;
    public CallMethod callMethod;
    public int count = 100;

    public bool perFrame = false;
    /// <summary>
    /// 秒
    /// </summary>
    public int delayTime_Seconds = 1;
    public enum CallMethod
    {
        Coroutine,
        Task,
    }
#if UNITY_EDITOR
    private string strFile = Application.streamingAssetsPath + "/1.apk";
#elif UNITY_ANDROID
    private string strFile = Application.streamingAssetsPath + "/1.apk";
#endif
    private Task task;
    private long oldTime;
    private void OnClickStart()
    {
        Debug.Log(strFile);
        oldTime = System.DateTime.Now.Ticks;
        if (callMethod == CallMethod.Coroutine)
        {
            StartCoroutine(CoroutineTest());
        }
        if (callMethod == CallMethod.Task)
        {
            task = Task.Factory.StartNew(() =>
            {
                SubThread();
            });
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 100, 50), "开始"))
        {
            OnClickStart();
        }
        if (GUI.Button(new Rect(50, 150, 100, 50), "100"))
        {
            count = 100;
        }
        if (GUI.Button(new Rect(50, 250, 100, 50), "100,0"))
        {
            count = 1000;
        }
        if (GUI.Button(new Rect(50, 350, 100, 50), "100,00"))
        {
            count = 10000;
        }
        if (GUI.Button(new Rect(50, 450, 100, 50), "打开每帧"))
        {
            perFrame = true;
        }
        if (GUI.Button(new Rect(50, 550, 100, 50), " 关闭每帧"))
        {
            perFrame = false;
        }
        if (GUI.Button(new Rect(50, 650, 100, 50), "使用协程"))
        {
            callMethod = CallMethod.Coroutine;
        }
        if (GUI.Button(new Rect(250, 50, 100, 50), "使用多线程"))
        {
            callMethod = CallMethod.Task;
        }

        if (GUI.Button(new Rect(250, 150, 100, 50), "释放"))
        {
            OnRelease();
        }
        if (GUI.Button(new Rect(250, 250, 100, 50), "加载"))
        {
            UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("Cube");
        }

        if (GUI.Button(new Rect(250, 350, 100, 50), "实例化"))
        {
            UnityEngine.AddressableAssets.Addressables.InstantiateAsync("Cube");
        }
    }

    private void OnRelease()
    {
        StopAllCoroutines();
        Debug.Log(nameof(OnRelease));
        if (task != null)
        {
            task.Dispose();
            Debug.Log(task == null);
            task = null;
            Debug.Log(nameof(task.Dispose));
        }
    }

    private void Update()
    {
        //var time = System.DateTime.Now.Ticks - oldTime;
        //m_TextTime.text = " " + time;
        Debug.Log("Log");
        Debug.LogError("LogError");
    }

    void SubThread()
    {
        Task.Delay(delayTime_Seconds * 1000).Wait();

        Debug.Log(string.Format("begin time : {0}", System.DateTime.Now.ToString("T")));


        for (int i = 0; i < count; i++)
        {
            Debug.Log("开始");
            UnityWebRequest request = new UnityWebRequest(strFile);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SendWebRequest();
            while (!request.downloadHandler.isDone)
            {
                m_TextByteCount.text = request.downloadProgress.ToString();
                Debug.Log(request.downloadProgress.ToString());
            }
            m_TextByteCount.text = request.downloadedBytes.ToString();

            Debug.Log("结束");
        }

        Debug.Log(string.Format("end time : {0}", System.DateTime.Now.ToString("T")));
    }

    IEnumerator CoroutineTest()
    {
        yield return new WaitForSeconds(delayTime_Seconds);

        Debug.Log(string.Format("begin time : {0}", System.DateTime.Now.ToString("T")));
        for (int i = 0; i < count; i++)
        {
            Debug.Log("开始");
            UnityWebRequest request = new UnityWebRequest(strFile);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SendWebRequest();
            while (!request.downloadHandler.isDone)
            {
                m_TextByteCount.text = request.downloadProgress.ToString();
                Debug.Log(request.downloadProgress.ToString());
                if (perFrame)
                {
                    yield return null;
                }
            }
            m_TextByteCount.text = request.downloadedBytes.ToString();

            Debug.Log("结束");
        }
        Debug.Log(string.Format("end time : {0}", System.DateTime.Now.ToString("T")));
    }
}
