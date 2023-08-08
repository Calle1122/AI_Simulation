using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI BunnyTxt;
    [SerializeField]
    private TextMeshProUGUI WolfTxt;
    [SerializeField]
    private TextMeshProUGUI CarrotTxt;

    public int AliveBunnies;
    public int AliveWolves;
    public int AliveCarrots;

    public GameObject SelectedStatObject;
    public Image statObjectImage;
    public TextMeshProUGUI statObjectName;
    public TextMeshProUGUI statObjectHunger;
    public Slider statObjectLustMeter;
    public TextMeshProUGUI statObjectCurrentState;
    
    private static StatManager     sm_instance;

    #region Properties

    public static StatManager Instance => sm_instance;

    #endregion

    private void OnEnable()
    {
        sm_instance = this;
    }

    void Update()
    {
        BunnyTxt.text = "Bunnies: " + AliveBunnies;
        WolfTxt.text = "Wolves: " + AliveWolves;
        CarrotTxt.text = "Carrots: " + AliveCarrots;
    }
}
