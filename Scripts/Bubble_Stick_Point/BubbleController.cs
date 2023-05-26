using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BubbleController : MonoBehaviour
{
    public PointHolder ph;
    public int bubbleName;                                                                            //kaçýncý sýradaki baloncuk olduðunu belirtir

    [Header("")]
    public int bubbleKind;
    public int bubbleValue;

    TextMeshProUGUI valueText;

    [Header("")]
    ParticleSystem starPS1;

    private void Start()
    {
        starPS1 = GetComponentInChildren<ParticleSystem>();

        GetComponent<Button>().onClick.AddListener( () => ph.ButtonClicked(gameObject.transform));


        EventTrigger.Entry entry1 = new() { eventID = EventTriggerType.PointerDown };                       //hem yeni bir event trigger giriþi tanýmladýk hemde türünü belirttik
        entry1.callback.AddListener(eventData => ph.ButtonDownAnim(gameObject.transform));                  //event trigger'a pointr down ekler ve fonksiyonunu belirtir
        GetComponentInParent<EventTrigger>().triggers.Add(entry1);

        EventTrigger.Entry entry2 = new() { eventID = EventTriggerType.PointerUp };
        entry2.callback.AddListener(eventData => ph.ButtonUpAnim(gameObject.transform, transform.GetChild(0)));         //0. cocuga ulas ve onu dalga yaymasý icin kaydet
        GetComponentInParent<EventTrigger>().triggers.Add(entry2);

        StartTMPFinder();                                                                                               //TMP objesini bulur ve saklar
    }

    public void ShowBubbleValue()
    {
        valueText.text = bubbleValue.ToString();

        if (bubbleValue == 0)                                                                                           //boluncugun degeri 0 olunca kucuk yýldýzlar oynamaya baslasýn
        {
            starPS1.Play(false);
            starPS1.GetComponentInChildren<ParticleSystem>().Play(false);
        }
        else
        {
            starPS1.Stop(false);
            starPS1.GetComponentInChildren<ParticleSystem>().Stop(false);
        }
    }

    void StartTMPFinder()
    {
        int a = transform.childCount;
        for (int i = 0; i < a; i++)
        {
            if (transform.GetChild(i).GetComponent<TextMeshProUGUI>())
            {
                valueText = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            }
        }
        valueText.text = bubbleValue.ToString();
    }
}
