using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class WebViewController : MonoBehaviour
{
    /// <summary>
    /// Canvas。
    /// </summary>
    [Header("Canvas")]
    [SerializeField]
    Canvas webViewCanvas;

    /// <summary>
    /// WebView。
    /// </summary>
    WebViewObject webView = null;

    /// <summary>
    /// WebViewを閉じたときのコールバック。
    /// </summary>
    event System.Action OnCompleteCloseCallback = delegate{};

    /// <summary>
    /// WebViewを開く。
    /// </summary>
    /// <param name="url">Url。</param>
    public void Open(string url)
    {
        var webViewData = new WebViewData();
        webViewData.Url = url;
        Open(webViewData);
    }

    /// <summary>
    /// WebViewを開く。
    /// </summary>
    /// <param name="webViewData">WebViewData。</param>
    public void Open(WebViewData webViewData)
    {
        StartCoroutine(OpenCoroutine(webViewData));
    }

    /// <summary>
    /// WebView開くコルーチン。
    /// </summary>
    /// <param name="webViewData">WebViewData。</param>
    IEnumerator OpenCoroutine(WebViewData webViewData)
    {
        if(webViewCanvas == null)
        {
            yield break;
        }

        if(webView == null)
        {
            // WebViewの作成。
            var webViewObject = new GameObject("WebViewObject");
            webViewObject.transform.SetParent(gameObject.transform);
            webViewObject.transform.localPosition = Vector3.zero;
            webViewObject.transform.localScale = Vector3.one;
            webView = webViewObject.AddComponent<WebViewObject>();
        }

        if(webViewData.OnCloseCallback != null)
        {
            OnCompleteCloseCallback = webViewData.OnCloseCallback;
        }

        // WebViewの初期化。
        webView.Init
        (
            transparent:true,

            // コールバック。
            cb:(msg) => 
            {
                var urlScheme = WebViewURLController.GetUri(msg);
                if(urlScheme.Scheme == "file" || urlScheme.Scheme == "http" || urlScheme.Scheme == "https")
                {
                    webView.LoadURL(msg);
                    return;
                }
                WebViewURLController.ProcessURL(msg, Close, webViewData.CustomCallbackData);
            },

            // エラー時。
            err:(msg) =>
            {
                if(webViewData.OnErrorCallback != null)
                {
                    webViewData.OnErrorCallback(WebViewStatus.ACCESS_FAILUER);
                }
            },

            // Httpeエラー。
            httpErr:(msg) =>
            {
                if(webViewData.OnErrorCallback != null)
                {
                    webViewData.OnErrorCallback(WebViewStatus.HTTP_ERROR);
                }
            },

            // ロード完了後。
            ld:(msg) =>
            {
                webView.EvaluateJS(@"
                    document.body.style.background = 'white';
                ");
                if(webViewData.OnOpenCallback != null)
                {
                    webViewData.OnOpenCallback();
                }
            },
            enableWKWebView:true
        );

        // URLの読み込み。
        webView.LoadURL(webViewData.Url);

        // マージンのセット。
        SetMargin();

        // WebViewの表示。
        webView.SetVisibility(true);

        yield break;
    }

    /// <summary>
    ///　マージンのセット。
    /// </summary>
    void SetMargin()
    {
        var viewRect = transform.GetComponent<RectTransform>();

        // 角の座標を取得。
        var rectCorners = new Vector3[4];
        viewRect.GetWorldCorners(rectCorners);

        // World座標からスクリーン座標に変換。
        var uiCamera = webViewCanvas.worldCamera;
        var upperLeft = RectTransformUtility.WorldToScreenPoint(uiCamera, rectCorners[1]);
        var lowerRight = RectTransformUtility.WorldToScreenPoint(uiCamera, rectCorners[3]);

        var margin = new Vector4(upperLeft.x, Screen.height - upperLeft.y, Screen.width - lowerRight.x, lowerRight.y);
        if(webView != null)
        {
            webView.SetMargins((int)margin.x, (int)margin.y, (int)margin.z, (int)margin.w);
        }
    }

    /// <summary>
    /// WebViewを閉じる。
    /// </summary>
    public void Close()
    {
        if(webView != null)
        {
            webView.SetVisibility(false);
            Destroy(webView.gameObject);
            webView = null;
            if(OnCompleteCloseCallback != null)
            {
                OnCompleteCloseCallback();
            }
        }
    }
}
