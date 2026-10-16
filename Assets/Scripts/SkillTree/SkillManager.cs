using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
  private void OnEnable(){
    SkillSlot.OnAbilityPointSpent += HandleAbilityPointsSpent;
  }

  private void OnDisable(){
    SkillSlot.OnAbilityPointSpent -= HandleAbilityPointsSpent;
  }

  private void HandleAbilityPointsSpent(SkillSlot slot){
    string skillName = slot.skillSO.skillName;

    switch(skillName)
    {
        case "Max Health Boost":
            StatsManager.Instance.UpdateMaxHealth(1);
            break;

        default:
            Debug.LogWarning("Unkown  skill:" + skillName);
            break;
    }
  }
}
