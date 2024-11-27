using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour {
    
    internal static SystemManager Instance {set; get;}

   
    [Header("Single Rule")]
    [SerializeField] SingleRuleLSystemGenerator ruleOnelsystem;
    [SerializeField] Slider iterationSlider;
    [SerializeField] Slider angleSlider;
    [SerializeField] Slider lengthSlider;

    [Header("Multi Rule")]
    [SerializeField] MultiRuleLSystemGenerator multiruleSytem;
    [SerializeField] Slider multi_iterationSlider;
    [SerializeField] Slider multi_angleSlider;
    [SerializeField] Slider multi_lengthSlider;

    void Awake() {
        if(Instance == null) Instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            ruleOnelsystem.AutoStartGenerate(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
             ruleOnelsystem.AutoStartGenerate(1);
        }
         else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
             ruleOnelsystem.AutoStartGenerate(2);
        }
         else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
             multiruleSytem.AutoStartGenerate(3);
        }
         else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
             multiruleSytem.AutoStartGenerate(4);
        }
         else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
             multiruleSytem.AutoStartGenerate(5);
        }
         else if(Input.GetKeyDown(KeyCode.Alpha6))
        {
             ruleOnelsystem.AutoStartGenerate(6);
        }
         else if(Input.GetKeyDown(KeyCode.Alpha7))
        {
             ruleOnelsystem.AutoStartGenerate(7);
        }
    }

     public void UpdateSingleRuleSystemSliders(int iterations, float angle, float length)
    {
        iterationSlider.value = iterations;
        angleSlider.value = angle;
        angleSlider.maxValue = angle * 1.5f;
        lengthSlider.value = length;
    }

      public void UpdateMultieRuleSystemSliders(int iterations, float angle, float length)
    {
        multi_iterationSlider.value = iterations;
        multi_angleSlider.value = angle;
        angleSlider.maxValue = angle * 1.5f;
        multi_lengthSlider.value = length;
    }

    public void SetSingleRuleLSystemProperty()
    {
        ruleOnelsystem.iterations = (int)iterationSlider.value;
        ruleOnelsystem.angle = angleSlider.value;
        ruleOnelsystem.length = lengthSlider.value;
    }

      public void SetMultiRuleLSystemProperty()
    {
        multiruleSytem.iterations = (int)multi_iterationSlider.value;
        multiruleSytem.angle = multi_angleSlider.value;
        multiruleSytem.length = multi_lengthSlider.value;
    }

}