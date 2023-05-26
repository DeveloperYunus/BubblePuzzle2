using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public PointHolder ph;
    public ScreenController sCont;
    public Transform UICamera;
    public bool isSharp;                                                                               //eðer baloncuklarýn içinde ise keskinliðini kaybetsin

    [Header("Particle System")]
    public ParticleSystem psTrail;
    public ParticleSystem psBubble;
    public List<Color> knifeColor = new();


    private void Start()
    {
        isSharp = true;
        if(GameObject.Find("PointHolder"))
            ph = GameObject.Find("PointHolder").GetComponent<PointHolder>();

        sCont = GameObject.Find("Main Camera").GetComponent<ScreenController>();
        UICamera = GameObject.Find("UI Camera").transform;

        PSystemColor(sCont.GetComponent<ScreenController>().whichColor);
        
        UICamera = GameObject.Find("UI Camera").transform;                                                          //bu 3 satýrý yazmayýnca býcak, mausu düzgün takip etmiyor
        UICamera.position = new Vector3(Screen.width/2, Screen.height/2, UICamera.position.z); 
        UICamera.GetComponent<Camera>().orthographicSize = Screen.height / 2;
    }

    void PSystemColor(int colorIndex)
    {
        ParticleSystem.MainModule trailMain = psTrail.main;
        ParticleSystem.MainModule bubbleMain = psBubble.main;
        ParticleSystem.MainModule floatingBubbleMain = GameObject.Find("BGBubblePS").GetComponent<ParticleSystem>().main;

        ParticleSystem.MinMaxGradient colorr = new(
            new Color(knifeColor[colorIndex].r + Random.Range(0, 0.31f), 
            knifeColor[colorIndex].g + Random.Range(0, 0.31f), 
            knifeColor[colorIndex].b + Random.Range(0, 0.31f), 0.3f),
            new Color(knifeColor[colorIndex].r + Random.Range(-0.3f, 0.1f), 
            knifeColor[colorIndex].g + Random.Range(-0.3f, 0.1f), 
            knifeColor[colorIndex].b + Random.Range(-0.3f, 0.1f), 0.3f));

        trailMain.startColor = colorr;
        floatingBubbleMain.startColor = colorr;
        bubbleMain.startColor = colorr;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !PointHolder.winGame)
        {
            transform.position = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            psTrail.Stop(false);
            AudioManager.instance.StopSound("onGoingKnife");
        }
        if (Input.GetMouseButtonDown(0))
        {
            psTrail.Play(false);
            psBubble.Play(false);
            AudioManager.instance.PlaySound("onGoingKnife");
        }


        if (Input.touchCount > 0)
        {
            transform.position = Input.GetTouch(0).position;

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                psTrail.Stop(false);
                AudioManager.instance.StopSound("onGoingKnife");
            }
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                psTrail.Play(false);
                psBubble.Play(false);
                AudioManager.instance.PlaySound("onGoingKnife");
            }
        }        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stick"))
        {
            if (isSharp)
            {
                ph.SliceStick(other.transform);
                psBubble.Play(false);
            }
        }

        if (other.CompareTag("Bubble"))
        {
            isSharp = false;
            psTrail.Play(false);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            isSharp = false;
            psTrail.Stop(false);
            AudioManager.instance.StopSound("onGoingKnife");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            isSharp = true;
            psTrail.Play(false);
            AudioManager.instance.PlaySound("onGoingKnife");
        }
    }
}
