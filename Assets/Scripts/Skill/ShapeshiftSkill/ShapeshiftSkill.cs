﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "ShapeshiftSkill", menuName = "Skills/ShapeshiftSkill")]
public class ShapeshiftSkill : Skill
{
    public float activeTime = 0.9f; // Thời gian kích hoạt Collider
    public override void Activate(GameObject user)
    {
        // Kích hoạt Collider tạm thời
        ActiveShapeShift collider = user.transform.parent.parent.GetComponentInChildren<ActiveShapeShift>();
        if (collider != null)
        {
            if (collider.CopiedPlayer == null)
            {
                Debug.Log("K có đối tượng!!");
                return;
            }
            //Debug.Log($"{collider.name} used!");
            collider.isFirstTimeUse = true;
            collider.StartCoroutine(collider.activeShapeShiftSkill(activeTime));
        }

        //Debug.Log($"{user.name} used Kick!");
    }
}
