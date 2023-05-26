using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class InLevelBtn : MonoBehaviour
{
    public GameObject hintBtnSlider;                                                    //1. 2. gibi seviyelerdeki restart, level select menu butonlarýný kontrol eder
    public PointHolder ph;
    public TextMeshProUGUI passTxt, zeroText;

    public static bool bubbleZero;                                                             //joker aktifmi dye kontrol eder
    bool hintPanelOpen;

    [Header("InLevelAnim")]
    public ParticleSystem zeroBtnPS;
    public TextMeshProUGUI zeroTxt;


    void Start()
    {
        ph = GameObject.Find("PointHolder").GetComponent<PointHolder>();
        bubbleZero = false;
        hintPanelOpen = false;

        passTxt.text = PlayerPrefs.GetInt("levelPass").ToString();
        zeroText.text = PlayerPrefs.GetInt("BubbleZero").ToString();

        GetComponent<CanvasGroup>().DOFade(0,0f);
        GetComponent<CanvasGroup>().DOFade(1,2f);
        hintBtnSlider.GetComponent<RectTransform>().DOScale(0, 0);
        hintBtnSlider.GetComponent<RectTransform>().DOLocalMove(new Vector2(840, 0), 0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GolevelSelectMenu();
        }
    }

    public void GolevelSelectMenu()
    {
        AudioManager.instance.PlaySound("blueBtn");
        SceneManager.LoadScene("LevelMenu");   
    }
    public void RestartLevel()
    {
        AudioManager.instance.PlaySound("blueBtn");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void HintBtn(GameObject btn)
    {
        AudioManager.instance.PlaySound("blueBtn");

        if (!hintPanelOpen)
        {
            hintPanelOpen = true;
            btn.GetComponent<RectTransform>().DOScale(1.15f, 0.2f);
            btn.GetComponent<RectTransform>().DOScale(1f, 0.2f).SetDelay(0.2f);
            hintBtnSlider.GetComponent<RectTransform>().DOKill();
            hintBtnSlider.GetComponent<RectTransform>().DOLocalMove(new Vector2(170, 0), 1.2f).SetEase(Ease.OutBack);             //200 (görünüor), 800 (görünmüyor)
            hintBtnSlider.GetComponent<RectTransform>().DOScale(1, 0);
        }
        else
        {
            btn.GetComponent<RectTransform>().DOScale(1.08f, 0.2f);
            btn.GetComponent<RectTransform>().DOScale(1f, 0.2f).SetDelay(0.2f);
            hintPanelOpen = false;
            hintBtnSlider.GetComponent<RectTransform>().DOKill();
            hintBtnSlider.GetComponent<RectTransform>().DOLocalMove(new Vector2(840, 0), 0.7f).SetEase(Ease.InBack);
            hintBtnSlider.GetComponent<RectTransform>().DOScale(0, 0).SetDelay(0.7f);
        }
    }

    public void ZeroBtn()
    {
        AudioManager.instance.PlaySound("jokerBtn");
        RectTransform btn = zeroBtnPS.transform.parent.GetComponent<RectTransform>();

        btn.DOScale(1.15f, 0.2f);
        btn.DOScale(1f, 0.2f).SetDelay(0.2f);

        if (PlayerPrefs.GetInt("BubbleZero") > 0)
        {
            if (!bubbleZero)
            {
                btn.GetComponent<Animator>().SetBool("isActive", true);
                zeroBtnPS.Play(false);
                bubbleZero = true;
            }
            else
            {
                btn.GetComponent<Animator>().SetBool("isActive", false);
                zeroBtnPS.Stop(false);
                bubbleZero = false;
            }
        }
        zeroText.text = PlayerPrefs.GetInt("BubbleZero").ToString();
    }

    public void LevelPass(GameObject btn)
    {
        AudioManager.instance.PlaySound("jokerBtn");
        btn.GetComponent<RectTransform>().DOScale(1.15f, 0.2f);
        btn.GetComponent<RectTransform>().DOScale(1f, 0.2f).SetDelay(0.2f);

        if (PlayerPrefs.GetInt("levelPass") > 0)
        {
            ph.WinGame();
            PlayerPrefs.SetInt("levelPass", PlayerPrefs.GetInt("levelPass") - 1);
            passTxt.text = PlayerPrefs.GetInt("levelPass").ToString();
        }
    }
    public void WatchADS(GameObject btn)
    {
        AudioManager.instance.PlaySound("blueBtn");
        btn.GetComponent<RectTransform>().DOScale(1.15f, 0.2f);
        btn.GetComponent<RectTransform>().DOScale(1f, 0.2f).SetDelay(0.2f);

        GetComponent<UnityADS>().ShowRewardedAD();
    }
    public void GoNextLevel()
    {
        AudioManager.instance.PlaySound("blueBtn");
        if (int.Parse(SceneManager.GetActiveScene().name) % GetComponent<UnityADS>().GecisReklamiBolum == 0)
        {
            GetComponent<UnityADS>().ShowInterstitial();
        }
        else
        {
            SceneManager.LoadScene((int.Parse(SceneManager.GetActiveScene().name) + 1).ToString());
        }
    }

    public void GiveRewardForADS(int amount)
    {
        AudioManager.instance.PlaySound("rewardAD");
        int zero = PlayerPrefs.GetInt("BubbleZero");
        int pass = PlayerPrefs.GetInt("levelPass");

        zero += amount;
        pass += amount;

        PlayerPrefs.SetInt("BubbleZero", zero);
        PlayerPrefs.SetInt("levelPass", pass);

        zeroTxt.text = PlayerPrefs.GetInt("BubbleZero").ToString();
        passTxt.text = PlayerPrefs.GetInt("levelPass").ToString();
    }
}
