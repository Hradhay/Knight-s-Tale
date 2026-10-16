using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour
{
    public SkillSO skillSO;

    public int currentLevel;
    public bool isUnlocked;

    public Image skillIcon;
    public Button skillButton;
    public TMP_Text skillLevelText;

    // Static events for SkillTreeManager
    public static System.Action<SkillSlot> OnAbilityPointSpent;
    public static System.Action<SkillSlot> OnSkillMaxed;

    private void Awake()
    {
        UpdateUI();
    }

    private void OnValidate()
    {
        if (skillSO != null && skillIcon != null && skillLevelText != null)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (skillSO == null || skillIcon == null || skillLevelText == null)
        {
            Debug.LogWarning($"SkillSlot {name}: Missing skillSO, skillIcon, or skillLevelText. Assign in Inspector.", this);
            return;
        }

        if (skillSO.skillIcon != null)
        {
            skillIcon.sprite = skillSO.skillIcon;
        }

        if (isUnlocked)
        {
            skillLevelText.text = currentLevel + "/" + skillSO.maxLevel;
            skillIcon.color = Color.white;
            if (skillButton != null)
            {
                skillButton.interactable = true;
            }
        }
        else
        {
            skillLevelText.text = "Locked";
            skillIcon.color = Color.grey;
            if (skillButton != null)
            {
                skillButton.interactable = false;
            }
        }
    }

    public void TryUpgradeSkill()
    {
        if (!isUnlocked || skillSO == null || currentLevel >= skillSO.maxLevel)
        {
            return;
        }

        currentLevel++;
        OnAbilityPointSpent?.Invoke(this);

        if (currentLevel >= skillSO.maxLevel)
        {
            OnSkillMaxed?.Invoke(this);
        }

        UpdateUI();
    }

    public bool CanUnlockSkill()
    {
        return skillSO != null && !isUnlocked;
    }

    public void Unlock()
    {
        isUnlocked = true;
        UpdateUI();
    }
}
