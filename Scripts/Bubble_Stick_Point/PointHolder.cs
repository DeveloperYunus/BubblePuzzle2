using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PointHolder : MonoBehaviour
{
    public Transform bh, sh;                                                                        //bh = bubbleHolder ,  sh = stickHolder
    public int stickHeigth;
    public GameObject winPnl;

    public List<GameObject> bubble;                                                                       //oynanabilir baloncuklar
    public List<GameObject> stick;                                                                        //aralardaki �ubuklar


    [HideInInspector]
    public Transform lastBubble, previousBubble;                                                          //son t�klanan ve bir �nceki t�klanan baloncuklar� tutar
    int totalBubbleCount;                                                                           //toplam baloncuk say�s� 
    public static bool winGame;

    [Header("Chain Properties")]
    public int zincirBoyutu;
    public int zincirSayisi;
    public float zincirBoyutuCarpan�;
    public List<GameObject> stickChain;                                                             //zincir halkalar�
    GameObject secondHingeJoint;


    private void Start()
    {
        lastBubble = null;                                                                          //baloncuk ismi oldu�undan ba�langi� ismi olarak kullan�lamaz                        
        previousBubble = null;
        winGame = false;
        totalBubbleCount = transform.childCount;

        bh = GameObject.Find("BubbleHolder").transform;
        sh = GameObject.Find("StickHolder").transform;
        winPnl = GameObject.Find("WinPnl");

        bh.GetComponent<CanvasGroup>().DOFade(1, 2f);
        GetComponent<CanvasGroup>().alpha = 0;

        ProductBubble();
        ProductStick();
        StartCoroutine(ChanceGravity());
    }

    void ProductBubble()//baloncuklar� �retir
    {
        for (int i = 0; i < totalBubbleCount; i++)
        {
            int bKind = transform.GetChild(i).GetComponent<PointProperties>().pointKind;
            GameObject a = Instantiate(bubble[bKind], transform.GetChild(i).GetComponent<RectTransform>().position, Quaternion.identity, bh);
            a.GetComponent<BubbleController>().ph = this;
            a.GetComponent<BubbleController>().bubbleName = i;
            a.GetComponent<BubbleController>().bubbleValue = transform.GetChild(i).GetComponent<PointProperties>().pointValue;
            a.GetComponent<BubbleController>().bubbleKind = bKind;
        }
    }
    void ProductStick()
    {
        for (int i = 0; i < totalBubbleCount - 1; i++)
        {
            for (int ii = i+1; ii < totalBubbleCount; ii++)
            {
                Transform child1 = bh.GetChild(i);                                          //cok fazla i�lem yap�lmas�n diye bu tan�mlamalar� yapt�k
                Transform child2 = bh.GetChild(ii);
                int stickKind = SelectStickKind(child1, child2);

                float angle = Mathf.Atan2(child2.position.y - child1.position.y, child2.position.x - child1.position.x) * 180 / Mathf.PI;

                //cubuk objelerini her nokta aras�nda bask�nl�k derecesine g�re olu�turur
                GameObject a = Instantiate(stick[stickKind], (child1.position + child2.position) / 2,
                    Quaternion.Euler(0,0, angle), sh);

                //cubukalr� noktalar�n aras�ndaki mesafeye g�re ayarlar
                a.GetComponent<RectTransform>().sizeDelta = new Vector2(Vector2.Distance(child1.localPosition, child2.localPosition), stickHeigth);
                a.GetComponent<StickProperties>().efctB1 = child1;
                a.GetComponent<StickProperties>().efctB2 = child2;
                a.GetComponent<StickProperties>().stickKind = stickKind;
                a.GetComponent<BoxCollider2D>().size = new Vector2(a.GetComponent<RectTransform>().sizeDelta.x - 360, stickHeigth);       //cubuklar�n collider�n� ayarlar

                if (stickKind == 1) angle += 30;        //��gen ise
                else angle += 45;                       //kare yada daire ise (farketmez)

                //bu fonksiyonu bir frame sonra ba�latt�k. Diger t�rl� UI cameran�n boyutunu de�i�tirdi�imiz i�in sondaki zincir halkas�nda sorun c�k�yordu
                StartCoroutine(ProductStickChain(a.transform, child1, child2, stickKind, angle));
            }
        }
    }
    IEnumerator ProductStickChain(Transform stickNumber, Transform child1, Transform child2, int stickKind, float angle)
    {
        yield return new WaitForEndOfFrame();
        float baloncuklarAras�ndakiMesafe = Vector2.Distance(child1.position, child2.position);

        if (zincirSayisi == 0)                                                  //zincir say�s� belli ise uzunluk hesaplanmas�n ve her cubugun sizncirleri e�it b�y�kl�kte olsun
        {
            zincirSayisi = Mathf.CeilToInt(baloncuklarAras�ndakiMesafe / zincirBoyutu);         //float say�y� bir �st int say�ya yuvarlar
            zincirBoyutu = Mathf.CeilToInt(baloncuklarAras�ndakiMesafe / zincirSayisi);         //float say�y� bir �st int say�ya yuvarlar
        }
        zincirSayisi = Mathf.CeilToInt(baloncuklarAras�ndakiMesafe / zincirBoyutu);         

        float xValue = (child1.position.x - child2.position.x) / zincirSayisi;
        float yValue = (child1.position.y - child2.position.y) / zincirSayisi;
        int kacinciZincirdeyim = 0;                                                           //ilk ve son ve aradaki zincirler i�in farkl� i�lemler yap�lacak
        Vector2 zincirSpawnPos;

        for (int i = 0; i < zincirSayisi; i++)
        {
            GameObject a;
            if (kacinciZincirdeyim == 0)        //ik zincir halkas�
            {
                zincirSpawnPos = new Vector2(child1.position.x - xValue * 0.5f, child1.position.y - yValue * 0.5f);
                a = Instantiate(stickChain[stickKind], zincirSpawnPos, Quaternion.Euler(0, 0, angle), stickNumber);
                a.AddComponent<DistanceJoint2D>().connectedAnchor = child1.position;
            }
            else if (kacinciZincirdeyim == zincirSayisi - 1)     //son zincir halkas�
            {
                zincirSpawnPos = new Vector2(child1.position.x - xValue * (0.5f + 1 * kacinciZincirdeyim), child1.position.y - yValue * (0.5f + 1 * kacinciZincirdeyim));
                a = Instantiate(stickChain[stickKind], zincirSpawnPos, Quaternion.Euler(0, 0, angle), stickNumber);
                a.AddComponent<DistanceJoint2D>().connectedAnchor = child2.position;
                a.GetComponent<HingeJoint2D>().connectedBody = secondHingeJoint.GetComponent<Rigidbody2D>();
            }
            else                //aradaki zincir halkalar�
            {
                zincirSpawnPos = new Vector2(child1.position.x - xValue * (0.5f + 1 * kacinciZincirdeyim), child1.position.y - yValue * (0.5f + 1 * kacinciZincirdeyim));
                a = Instantiate(stickChain[stickKind], zincirSpawnPos, Quaternion.Euler(0, 0, angle), stickNumber);
                a.GetComponent<HingeJoint2D>().connectedBody = secondHingeJoint.GetComponent<Rigidbody2D>();
            }
            a.GetComponent<RectTransform>().sizeDelta = new Vector2(zincirBoyutu * zincirBoyutuCarpan�, zincirBoyutu * zincirBoyutuCarpan�);

            secondHingeJoint = a;
            kacinciZincirdeyim++; 
        }
    }

    public void ButtonClicked(Transform bubbleObject)
    {
        int s = Random.Range(0, 3);
        if (s == 0)
            AudioManager.instance.PlaySound("bubble1");
        else if (s == 1)
            AudioManager.instance.PlaySound("bubble2");
        else
            AudioManager.instance.PlaySound("bubble3");


        if (InLevelBtn.bubbleZero)
        {
            lastBubble = null;
            PlayerPrefs.SetInt("BubbleZero", PlayerPrefs.GetInt("BubbleZero") - 1);
            GameObject.Find("InLevelBtns").GetComponent<InLevelBtn>().ZeroBtn();
            
            bubbleObject.GetComponent<BubbleController>().bubbleValue = 0;
            bubbleObject.GetComponent<BubbleController>().ShowBubbleValue();
            WinControl();
            return;
        }

        previousBubble = lastBubble;
        lastBubble = bubbleObject;
        
        int a = sh.childCount;
        for (int i = 0; i < a; i++)
        {
            if (!sh.GetChild(i).GetComponent<BoxCollider2D>().enabled)
            {
                if (sh.GetChild(i).GetComponent<StickProperties>().efctB1 == lastBubble && sh.GetChild(i).GetComponent<StickProperties>().efctB2 == previousBubble)
                {
                    sh.GetChild(i).GetComponent<CanvasGroup>().DOKill();
                    sh.GetChild(i).GetComponent<CanvasGroup>().DOFade(1, 1f);
                    lastBubble = null;                                                                                          //�nceki baloncuk ad�n� s�f�rlamak i�in
                    StickEffect(sh.GetChild(i), 1);
                }
                else if (sh.GetChild(i).GetComponent<StickProperties>().efctB1 == previousBubble && sh.GetChild(i).GetComponent<StickProperties>().efctB2 == lastBubble)
                {
                    sh.GetChild(i).GetComponent<CanvasGroup>().DOKill();
                    sh.GetChild(i).GetComponent<CanvasGroup>().DOFade(1, 1f);
                    lastBubble = null;
                    StickEffect(sh.GetChild(i), 1);
                }
            }
        }

        WinControl();
    }
    public void SliceStick(Transform stick)
    {
        stick.GetComponent<CanvasGroup>().DOKill();
        stick.GetComponent<CanvasGroup>().DOFade(0, 1.6f);
        StickEffect(stick, -1);
        previousBubble = null;
        lastBubble = null;
    }
    public void WinControl()
    {
        int a = bh.childCount;  
        for (int i = 0; i < a; i++)                                                             //baloncuklar� s�ras�yla kontrol eder ve hepsi 0 olunca kazan�rs�n
        {
            if (bh.GetChild(i).GetComponent<BubbleController>().bubbleValue != 0)
                return;
        }

        if (!winGame)                                                                            //oyunu kazand�n
            WinGame();
    }
    public void WinGame()                                                                        //level Pass jokeri i�in unu ayr� yapt�k
    {
        AudioManager.instance.PlaySound("winSound");
        winGame = true;

        if(PlayerPrefs.GetInt("currentLevel") <= int.Parse(SceneManager.GetActiveScene().name))
        {
            PlayerPrefs.SetInt("currentLevel", int.Parse(SceneManager.GetActiveScene().name) + 1);
        }
        winPnl.GetComponent<RectTransform>().DOScale(1, 0);
        winPnl.GetComponent<CanvasGroup>().DOFade(1, 1);

        if (int.Parse(SceneManager.GetActiveScene().name) % 5 == 0)
            AudioManager.instance.GetComponent<WiseWords>().Wingame(int.Parse(SceneManager.GetActiveScene().name) / 5 - 1);
    }


    void StickEffect(Transform a, int deger)                         //cubuklar aktif olunca baloncuklar� etkileme �ekilleri (a = cubuk)
    {
        if (deger == 1)
        {
            AudioManager.instance.PlaySound("stickAppear");
            a.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            a.GetComponent<BoxCollider2D>().enabled = false;
            AudioManager.instance.PlaySound("stickDisappear");
        }

        Transform bObject1 = a.GetComponent<StickProperties>().efctB1;
        Transform bObject2 = a.GetComponent<StickProperties>().efctB2;
        int bValue1 = bObject1.GetComponent<BubbleController>().bubbleValue;
        int bValue2 = bObject2.GetComponent<BubbleController>().bubbleValue;

        switch (a.GetComponent<StickProperties>().stickKind)
        {
            case 0:
                bValue1 -= deger;
                bValue2 -= deger;
                break;

            case 1:
                bValue1 += deger;
                bValue2 += deger;
                break;

            case 2:
                if (bObject1.GetComponent<BubbleController>().bubbleKind == 2)
                    bValue1 += deger;
                else
                    bValue1 -= deger;

                if (bObject2.GetComponent<BubbleController>().bubbleKind == 2)
                    bValue2 += deger;
                else
                    bValue2 -= deger;
                break;
        }

        bObject1.GetComponent<BubbleController>().bubbleValue = bValue1;
        bObject2.GetComponent<BubbleController>().bubbleValue = bValue2;
        bObject1.GetComponent<BubbleController>().ShowBubbleValue();
        bObject2.GetComponent<BubbleController>().ShowBubbleValue();
        WinControl();
    }
    int SelectStickKind(Transform c1, Transform c2)
    {
        if (c1.GetComponent<BubbleController>().bubbleKind == 2 || c2.GetComponent<BubbleController>().bubbleKind == 2)
            return 2;
        else if (c1.GetComponent<BubbleController>().bubbleKind == 1 || c2.GetComponent<BubbleController>().bubbleKind == 1)
            return 1;
        else
            return 0;
    }


    public void ButtonDownAnim(Transform bubble)
    {
        bubble.GetComponent<RectTransform>().DOScale(0.9f, 0.2f);
    }
    public void ButtonUpAnim(Transform bubble, Transform waveChild)
    {
        bubble.GetComponent<RectTransform>().DOScale(1.1f, 0.2f);
        bubble.GetComponent<RectTransform>().DOScale(1f, 0.15f).SetDelay(0.2f);

        if (true)
        {
            waveChild.GetComponent<RectTransform>().DOScale(1, 0);
            waveChild.GetComponent<Image>().DOFade(0.9f, 0);

            waveChild.GetComponent<RectTransform>().DOScale(1.5f, 1.3f);
            waveChild.GetComponent<Image>().DOFade(0, 1.5f);
        }
    }
    IEnumerator ChanceGravity()                                       //yer�ekimini s�rekli olarak de�i�tirir
    {
        yield return new WaitForSeconds(2f);
        float x = Random.Range(-4, 4);
        float y = Random.Range(-4, 4);
        Physics2D.gravity = new Vector2(x, y);
        Physics.gravity = new Vector2(x, y);
        StartCoroutine(ChanceGravity());
    }
}