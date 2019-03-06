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
        // webViewData.Url = "file://" + System.IO.Path.Combine(Application.streamingAssetsPath, "WebPageSample.html");
        webViewData.Url = "jar:file://" + System.IO.Path.Combine(Application.persistentDataPath, "WebPageSample.html");
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
}
