using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WebViewSample : MonoBehaviour
{
    /// <summary>
    /// 開くボタン。
    /// </summary>
    [Header("開くボタン")]
    [SerializeField]
    Button openButton;

    /// <summary>
    /// 閉じるボタン。
    /// </summary>
    [Header("閉じるボタン")]
    [SerializeField]
    Button closeButton;

    /// <summary>
    /// WebView。
    /// </summary>
    [Header("webView")]
    [SerializeField]
    WebViewController webView;

    /// <summary>
    /// 状態ラベル。
    /// </summary>
    [Header("StatusLbl")]
    [SerializeField]
    TextMeshProUGUI statusLbl;

    void Start()
    {
        openButton.onClick.AddListener(OpenWebView);
        closeButton.onClick.AddListener(CloseWebView);
    }

    /// <summary>
    /// WebViewを開く。
    /// </summary>
    void OpenWebView()
    {
        var webViewData = new WebViewData();
        webViewData.Url = GetLocalFilePath("WebViewSample.html");
        webViewData.OnOpenCallback = () => statusLbl.text = "Open";
        webViewData.OnCloseCallback = () => statusLbl.text = "Close";
        webView.Open(webViewData);
    }

    /// <summary>
    /// WebViewを閉じる。
    /// </summary>
    void CloseWebView()
    {
        webView.Close();
    }

    /// <summary>
    /// ローカルのファイルパスを取得。
    /// </summary>
    /// <param name="fileName">ファイル名。</param>
    /// <returns>ローカルのファイルパス。</returns>
    string GetLocalFilePath(string fileName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return System.IO.Path.Combine("file:///android_asset/", fileName);
#else
        return System.IO.Path.Combine("file://" + Application.streamingAssetsPath, fileName);
#endif
    }
}
