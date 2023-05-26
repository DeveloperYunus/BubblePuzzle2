using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Networking;

public class IntConnection : MonoBehaviour
{
    public bool shouldControl;

    [HideInInspector] public static bool isConnected;


    void Start()
    {        
        if (shouldControl)
        {
            CheckIntConnection();
        }
        else
        {
            CheckIntPanel(false);
        }
    }

    public void CheckIntConnection()
    {
        StartCoroutine(GetRequest("https://google.com"));
    }

    public void IntConnectBtnAnim(GameObject retryBtn)
    {
        AudioManager.instance.PlaySound("blueBtn");
        retryBtn.GetComponent<RectTransform>().DOScale(1.1f, 0.1f);
        retryBtn.GetComponent<RectTransform>().DOScale(1f, 0.1f).SetDelay(0.1f);
    }


    void CheckIntPanel(bool show)
    {
        if (show)
        {
            GetComponent<CanvasGroup>().DOFade(1, 0.5f);
            GetComponent<RectTransform>().DOScale(1, 0);
        }
        else
        {
            GetComponent<CanvasGroup>().DOFade(0, 0.5f);
            GetComponent<RectTransform>().DOScale(0, 0).SetDelay(0.5f);
        }
    }


    IEnumerator GetRequest(string url)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(url);

        //Sayfaya istek atýlýr ve sonuç gelene kadar beklenir
        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.ConnectionError)                            //internet yok
        {
            isConnected = false;
            CheckIntPanel(true);
        }
        else                                                                                        //internet var
        {
            isConnected = true;
            CheckIntPanel(false);
        }
    }
}
