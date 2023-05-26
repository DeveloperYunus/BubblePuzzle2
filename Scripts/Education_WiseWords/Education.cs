using TMPro;
using UnityEngine;
using DG.Tweening;

public class Education : MonoBehaviour
{
    public int whichFunction;


    private void Start()
    {
        switch (whichFunction)
        {
            //yeþil baloncuk eðitim textini güncelle
            case 0:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "Amaç: Tüm baloncuklarýn üzerinde '0' yazmasý.\n<color=green>Yeþil</color>  baloncuklar kendisine ve baðlý olduðu baloncuða '-1' ekler.";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "Purpose: All bubbles have '0' written on them.\n <color=green>Green</color> bubbles add '-1' to itself and to the bubble to which it is attached.";
                break;

            //hand objesini animasyon et
            case 1:
                InvokeRepeating(nameof(HandAnim), 0, 1);
                break;

            //Kýrmýzý baloncuk eðitim textini güncelle
            case 2:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "<color=red>Kýrmýzý</color>  baloncuklar kendisine ve baðlý olduðu baloncuða '+1' ekler.";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "<color=red>Red</color> bubbles add '+1' to itself and to the bubble to which it is attached.";
                break;

            //mavi baloncuk eðitim textini güncelle
            case 3:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "<color=blue>Mavi</color>  baloncuklar kendisine ve baðlandýðý <color=blue>mavilere</color> '+1', diðer <color=green>renk</color><color=red>teki</color> baloncuklara ise '-1' ekler.";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "<color=blue>Blue</color> bubbles add '+1' to itself and the <color=blue>blues</color> to which it is attached, and '-1' to bubbles of the other <color=green>col</color><color=red>or</color>.";
                break;

            //levelPass joker eðitim textini güncelle
            case 4:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "1. Ýpucu : Bölümü geçmeni saðlar. (>>)";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "Tip 1 : Allows you to pass the level. (>>)";
                break;

            //zeroBtn joker eðitim textini güncelle
            case 5:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "2. Ýpucu : Herhangi bir baloncuðu sýfýrlar. Üzerinde 0 yazan yeþil ipucu butonuna bastýkdan sonra bir baloncuða týkla.";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "Tip 2 : Equalize any bubble to zero. Click on the green hint button with 0 on it, then click on a bubble.";
                break;

            default:
                print("deðer hatasý");
                break;
        }               
    }



    void HandAnim()
    {
        gameObject.GetComponent<RectTransform>().DOScale(1.1f, 0.5f);
        gameObject.GetComponent<RectTransform>().DOScale(1f, 0.5f).SetDelay(0.5f);
    }
}
