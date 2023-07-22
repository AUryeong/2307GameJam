using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{

    private Animator animator;

    private int hp = 3;
    public int Hp
    {
        get { return hp; }
        set
        {
            if (value < hp && value > 0)
            {
                UIManager.Instance.HitWarning();
                InGameManager.Instance.CameraShake(0.2f, 0.1f);
            }
            hp = value;
            UIManager.Instance.UpdateHp(value);
            InGameManager.Instance.TimerReset();
            if (hp <= 0)
                InGameManager.Instance.GameOver();
        }
    }

    private float exp;
    [HideInInspector] public int level = 1;
    public float Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
            if (exp >= maxExp)
            {
                exp -= maxExp;
                level++;
                maxExp = Mathf.Pow(level, 1.7f) * 100;
                InGameManager.Instance.LevelUp();
            }
            UIManager.Instance.UpdateLevel(level, exp, maxExp);
        }
    }
    private float maxExp = 100;

    private float skillGauge;
    public float SkillGauge
    {
        get
        {
            return skillGauge;
        }
        set
        {
            skillGauge = value;
            UIManager.Instance.UpdateSkill(skillGauge / 100f);
            if (skillGauge >= 100f)
            {
                skillGauge -= 100f;
                InGameManager.Instance.Fever();
            }
        }
    }

    protected override void OnCreated()
    {
        animator = GetComponent<Animator>();

        level = 1;
        Hp = 3;
        Exp = 0;
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
