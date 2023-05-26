using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsPanel : MonoBehaviour
{
    public GameObject canvas, settingsPanel;
    public float setPanelActvTime;
    public bool isSetPnlActive;
    public RectTransform levelContent;

    [Header("TextMP")]
    public TextMeshProUGUI language;
    public TextMeshProUGUI sound;
    public TextMeshProUGUI otherGames;

    [Header("Color")]
    public List<Color> flagColor = new();
    public Image sliderFill, sliderHandle;
    public Slider slider;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("firstOpen"))                       //Bu key yok ise oyunu ilk kez açýyom demektir.
        {
            PlayerPrefs.SetFloat("globalVolume", 0.5f);
            PlayerPrefs.SetInt("firstOpen", 0);
            PlayerPrefs.SetInt("currentLevel", 1);
            PlayerPrefs.SetInt("BubbleZero", 4);                    //baloncuk sýfýrlama jokeri
            PlayerPrefs.SetInt("levelPass", 2);                     //level geçme jokeri
            FindSystemLanguage();
        }
    }

    void Start()
    {
        float  value = Mathf.Floor((PlayerPrefs.GetInt("currentLevel") - 1) / 5) * 355;                  //sahne acýldýgýnda hangi levelde kaldýysa o en basa gelsin
        levelContent.offsetMax = new Vector2(levelContent.offsetMax.x, value);
        levelContent.offsetMin = new Vector2(levelContent.offsetMin.x, levelContent.offsetMin.y + value);

        isSetPnlActive = false;
        canvas.GetComponent<CanvasGroup>().DOFade(0, 0f);
        canvas.GetComponent<CanvasGroup>().DOFade(1, 2f);

        settingsPanel.GetComponent<RectTransform>().DOScale(0, 0);
        settingsPanel.GetComponent<CanvasGroup>().DOFade(0, 0);

        slider.value = PlayerPrefs.GetFloat("globalVolume") * 10;
        LanguageBtn(PlayerPrefs.GetInt("language"));
        AudioManager.instance.StopSound("slider");
        AudioManager.instance.StopSound("languageBtn");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SettingsBtn();
    }


    public void SettingsBtn()
    {
        AudioManager.instance.PlaySound("blueBtn");
        GetComponent<CanvasGroup>().DOKill();
        settingsPanel.GetComponent<RectTransform>().DOKill();

        if (isSetPnlActive)//Ayarlar paneli kapanacak.
        {
            GetComponent<CanvasGroup>().DOFade(1f, setPanelActvTime);
            settingsPanel.GetComponent<RectTransform>().DOScale(0f, 0f).SetDelay(setPanelActvTime);
            settingsPanel.GetComponent<CanvasGroup>().DOFade(0f, setPanelActvTime);
        }
        else
        {
            GetComponent<CanvasGroup>().DOFade(0f, setPanelActvTime);
            settingsPanel.GetComponent<RectTransform>().DOScale(1f, 0f);
            settingsPanel.GetComponent<CanvasGroup>().DOFade(1f, setPanelActvTime);
        }
        //Button sesi
        isSetPnlActive = !isSetPnlActive;
    }
    public void LanguageBtn(int languageValue)
    {
        AudioManager.instance.PlaySound("languageBtn");
        switch (languageValue)
        {
            case 0:
                PlayerPrefs.SetInt("language", 0);                  //Dil türkce oldu
                language.text = "Dil";
                sound.text = "Ses";
                otherGames.text = "Beðendiysen bana 5 yýldýz ver.";
                break;

            case 1:
                PlayerPrefs.SetInt("language", 1);                  //Dil ingilizce oldu
                language.text = "Language";
                sound.text = "Sound";
                otherGames.text = "If you enjoy give me 5 star.";
                break;
        }
        sliderFill.color = flagColor[languageValue];
        sliderHandle.color = flagColor[languageValue];
    }
    public void SetGlobalVolume(Slider slider)
    {
        PlayerPrefs.SetFloat("globalVolume", slider.value * 0.1f);
        AudioManager.instance.SetGV(slider.value * 0.1f);
    }
    public void GoPlayStorePage()
    {
        AudioManager.instance.PlaySound("blueBtn");
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.KehribarinAtolyesi.com.unity.BubblePuzzle2&hl=tr&gl=US");
    }
    void FindSystemLanguage()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Turkish:
                PlayerPrefs.SetInt("language", 0);                  //Dil türkce oldu
                break;

            default:
                PlayerPrefs.SetInt("language", 1);                  //Dil ingilizce oldu
                break;
        }        
    }
}
