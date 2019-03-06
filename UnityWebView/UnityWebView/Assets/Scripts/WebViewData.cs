using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewData
{
    /// <summary>
    /// URL。
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 開いた後のコールバック。
    /// </summary>
    public System.Action OnOpenCallback { get; set; }

    /// <summary>
    /// 閉じた後のコールバック。
    /// </summary>
    public System.Action OnCloseCallback { get; set; }

    /// <summary>
    /// エラーコールバック。
    /// </summary>
    public System.Action<WebViewStatus> OnErrorCallback { get; set; }

    /// <summary>
    /// カスタムコールバックデータ。
    /// </summary>
    public Dictionary<string, System.Action> CustomCallbackData { get; set; }
}
