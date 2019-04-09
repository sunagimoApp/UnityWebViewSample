using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WebViewURLController
{   
    /// <summary>
    /// Unityスキーム名。
    /// </summary>
    const string UNITY_SCHEME = "unity";

    /// <summary>
    /// WebViewを閉じるスキーム名。
    /// </summary>
    const string WEBVIEW_CLOSE_SCHEME = "webview-close";
    
    /// <summary>
    /// ブラウザで開くスキーム名。
    /// </summary>
    const string BROWSER_OPEN = "browser-open";

    /// <summary>
    /// WebViewを閉じる処理。
    /// </summary>
    static System.Action OnClosedWebView = delegate{};

    /// <summary>
    /// URLを処理する。
    /// </summary>
    /// <param name="url">URL。</param>
    /// <param name="onClosedWebView">WebViewを閉じたときのイベント。</param>
    /// <param name="customCallbackData">カスタムコールバックデータ。</param>
    public static void ProcessURL(string url, System.Action onClosedWebView, Dictionary<string, System.Action> customCallbackData = null)
    {
        OnClosedWebView = onClosedWebView;

        if(string.IsNullOrEmpty(url))
        {
            return;
        }

        // Uriを取得。
        var uri = GetUri(url);

        if(uri == null)
        {
            return;
        }

        // スキーム名にUnityが含まれている場合はスキーム部分を削除する。
        if(uri.Scheme == UNITY_SCHEME)
        {
            var unityReplaceSb = new System.Text.StringBuilder(url);
            unityReplaceSb.Replace(uri.GetLeftPart(System.UriPartial.Scheme), "");
            url = unityReplaceSb.ToString();
            uri = GetUri(url);
        }

        // パラメーターを取得。
        var commonReplaceSb = new System.Text.StringBuilder(url);
        commonReplaceSb.Replace(uri.GetLeftPart(System.UriPartial.Scheme), "");

        var param = commonReplaceSb.ToString();

        // 共通のURLスキームを処理。
        if(IsProcessCommonURLScheme(uri.Scheme, param))
        {
            return;
        }

        // カスタムされたコールバックを処理。
        if(customCallbackData != null)
        {
            foreach(var callbackData in customCallbackData)
            {
                if(callbackData.Key == uri.Scheme)
                {
                    callbackData.Value();
                }
            }
        }
    }

    /// <summary>
    /// Uriを取得。
    /// </summary>
    /// <param name="url">URL。</param>
    /// <returns>Uri</returns>
    static public Uri GetUri(string url)
    {
        System.Uri uri;
        try
        {
            uri = new System.Uri(url);
        }
        catch (System.UriFormatException e)
        {
            Debug.LogError("URIスキームのフォーマットが正しくありません。: " + e);
            return null;
        }
        finally
        {
            Debug.Log(url);
        }
        return uri;
    }

    /// <summary>
    /// 共通のURLスキーム処理。
    /// </summary>
    /// <param name="scheme">スキーム。</param>
    /// <param name="param">パラメーター。</param>
    static bool IsProcessCommonURLScheme(string scheme, string param)
    {
        switch(scheme)
        {
            case WEBVIEW_CLOSE_SCHEME:
                OnClosedWebView();
                return true;
            case BROWSER_OPEN:
                Application.OpenURL(param);
                return true;
            default:
                return false;
        }
    }
}
