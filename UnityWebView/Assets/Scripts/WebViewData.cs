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
    /// 開いた後のイベント。
    /// </summary>
    public System.Action OnOpened { get; set; }

    /// <summary>
    /// 閉じた後のイベント。
    /// </summary>
    public System.Action OnClosed { get; set; }

    /// <summary>
    /// エラー時イベント。
    /// </summary>
    public System.Action<WebViewStatus> OnError { get; set; }

    /// <summary>
    /// カスタムコールバックデータ。
    /// </summary>
    public Dictionary<string, System.Action> CustomCallbackData { get; set; }
}
