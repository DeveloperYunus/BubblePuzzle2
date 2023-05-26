using UnityEngine.UI;
using UnityEngine;

public class BGFloatingSetting : MonoBehaviour
{
    public ParticleSystem bgPs;
    //public Image playBtnImg;

    void Start()
    {
        bgPs = GetComponent<ParticleSystem>();

        float a = Random.Range(0, 3);
        if (a == 1) a = 0.3333f;
        ParticleSystem.TextureSheetAnimationModule psHolder = bgPs.textureSheetAnimation;
        psHolder.startFrame = new(a, a);
        bgPs.Play(false);

        /*
        if (GameObject.Find("PlayBtn"))
        {
            playBtnImg = GameObject.Find("PlayBtn").GetComponent<Image>();
            playBtnImg.color = bgPs.main.startColor.color;
            playBtnImg.color = new Color(playBtnImg.color.r, playBtnImg.color.g, playBtnImg.color.b, 1);

            GameObject.Find("PlayImage").GetComponent<Image>().color = GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor;
        }*/
    }
}