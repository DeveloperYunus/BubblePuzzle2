using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public List<Color> bgColor = new();

    [HideInInspector]
    public int whichColor;                                                                         //hangi renk olduðunu saklar cunkü zýddýný knife' e verecez


    private void Awake()
    {
        whichColor = Random.Range(0, bgColor.Capacity);
        GetComponent<Camera>().backgroundColor = bgColor[whichColor];
    }
}
