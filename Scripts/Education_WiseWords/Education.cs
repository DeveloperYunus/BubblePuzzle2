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
            //ye�il baloncuk e�itim textini g�ncelle
            case 0:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "Ama�: T�m baloncuklar�n �zerinde '0' yazmas�.\n<color=green>Ye�il</color>  baloncuklar kendisine ve ba�l� oldu�u baloncu�a '-1' ekler.";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "Purpose: All bubbles have '0' written on them.\n <color=green>Green</color> bubbles add '-1' to itself and to the bubble to which it is attached.";
                break;

            //hand objesini animasyon et
            case 1:
                InvokeRepeating(nameof(HandAnim), 0, 1);
                break;

            //K�rm�z� baloncuk e�itim textini g�ncelle
            case 2:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "<color=red>K�rm�z�</color>  baloncuklar kendisine ve ba�l� oldu�u baloncu�a '+1' ekler.";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "<color=red>Red</color> bubbles add '+1' to itself and to the bubble to which it is attached.";
                break;

            //mavi baloncuk e�itim textini g�ncelle
            case 3:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "<color=blue>Mavi</color>  baloncuklar kendisine ve ba�land��� <color=blue>mavilere</color> '+1', di�er <color=green>renk</color><color=red>teki</color> baloncuklara ise '-1' ekler.";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "<color=blue>Blue</color> bubbles add '+1' to itself and the <color=blue>blues</color> to which it is attached, and '-1' to bubbles of the other <color=green>col</color><color=red>or</color>.";
                break;

            //levelPass joker e�itim textini g�ncelle
            case 4:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "1. �pucu : B�l�m� ge�meni sa�lar. (>>)";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "Tip 1 : Allows you to pass the level. (>>)";
                break;

            //zeroBtn joker e�itim textini g�ncelle
            case 5:
                if (PlayerPrefs.GetInt("language") == 0)
                    GetComponent<TextMeshProUGUI>().text = "2. �pucu : Herhangi bir baloncu�u s�f�rlar. �zerinde 0 yazan ye�il ipucu butonuna bast�kdan sonra bir baloncu�a t�kla.";
                else if (PlayerPrefs.GetInt("language") == 1)
                    GetComponent<TextMeshProUGUI>().text = "Tip 2 : Equalize any bubble to zero. Click on the green hint button with 0 on it, then click on a bubble.";
                break;

            default:
                print("de�er hatas�");
                break;
        }               
    }



    void HandAnim()
    {
        gameObject.GetComponent<RectTransform>().DOScale(1.1f, 0.5f);
        gameObject.GetComponent<RectTransform>().DOScale(1f, 0.5f).SetDelay(0.5f);
    }
}
