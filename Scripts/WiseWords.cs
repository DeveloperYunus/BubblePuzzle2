using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WiseWords : MonoBehaviour
{
    TextMeshProUGUI wiseTxt;
    public List<WW> ww;

    public void Wingame(int sceneName)
    {
        wiseTxt = GameObject.Find("WiseWordsTxt").GetComponent<TextMeshProUGUI>();
        wiseTxt.text = ww[sceneName].language[PlayerPrefs.GetInt("language")];
    }
}

[System.Serializable]
public class WW
{
    public List<string> language;
}
