using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelBtnCont : MonoBehaviour
{
    Transform btn;                                  //level select menusundeki butonalrda kullanýlýr.

    private void Start()
    {
        btn = transform;
        btn.GetChild(0).GetComponent<TextMeshProUGUI>().text = btn.name;
            
        if (int.Parse(btn.name) > PlayerPrefs.GetInt("currentLevel"))
            btn.GetComponent<Button>().interactable = false;

        GetComponent<Button>().onClick.AddListener(() => GoLevel(btn));
    }

    public void GoLevel(Transform btn)
    {
        AudioManager.instance.PlaySound("blueBtn");
        SceneManager.LoadScene(btn.name);
    }
}
