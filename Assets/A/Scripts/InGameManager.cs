using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public bool isGaming = false;

    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private Enemy originEnemy;
    [SerializeField] private Enemy upgradeEnemy;

    public Material flashMaterial;

    private Vector3 defaultCameraPos;

    private float cameraShakePower = 0;

    private float timer;
    private float maxTimer = 2;

    private float feverDuration = 0;
    private float feverMaxDuration = 2;
    private float feverInvDuration = 0;

    [SerializeField] private ParticleSystem feverParticle;

    private int combo;
    public readonly KeyCode[] keyCodes = new KeyCode[]
    {
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L
    };
    public readonly string[] keyCodeStrings = new string[]
    {
        "A",
        "S",
        "D",
        "F",
        "G",
        "H",
        "J",
        "K",
        "L"
    };

    private void Start()
    {
        isGaming = true;
        combo = 0;

        for (int i = 0; i < 5; i++)
        {
            GetNewEnemy();
        }

        EnemyMoveUpdate();
        defaultCameraPos = Camera.main.transform.position;

        SoundManager.Instance.PlaySound("ingame", ESoundType.BGM);
    }

    private void Update()
    {
        if (!isGaming) return;

        TimerUpdate();
        KillUpdate();
        CameraUpdate();
        FeverUpdate();
    }

    public void LevelUp()
    {
        isGaming = false;
        UIManager.Instance.LevelUP();
        Player.Instance.Hp++;

        EnemyMoveUpdate();

        foreach(var enemy in enemies)
            enemy.UpdateDiretion();

        StartCoroutine(TextWaitCoroutine());
    }

    private IEnumerator TextWaitCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        isGaming = true;
    }

    public void TimerReset()
    {
        timer = 0;
        UIManager.Instance.UpdaeTimer(timer / maxTimer);
    }

    private void FeverUpdate()
    {
        if (feverDuration > 0)
        {
            Player.Instance.SkillGauge = feverDuration / feverMaxDuration * 100f;
            feverDuration -= Time.deltaTime;
            if (feverDuration <= 0)
            {
                isGaming = false;
                UIManager.Instance.DeActiveSkill();
                StartCoroutine(TextWaitCoroutine());
                feverParticle.Stop();
            }
        }
    }

    private void TimerUpdate()
    {
        if (feverDuration > 0) return;
        if (feverInvDuration > 0) return;

        timer += Time.deltaTime;
        UIManager.Instance.UpdaeTimer(timer / maxTimer);
        if (timer >= maxTimer)
        {
            timer -= maxTimer;
            Player.Instance.Hp--;
            CameraShake(0.2f, 0.1f);
        }
    }

    private void CameraUpdate()
    {
        if (cameraShakePower > 0)
        {
            Camera.main.transform.position = defaultCameraPos + (Vector3)Random.insideUnitCircle * cameraShakePower;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        isGaming = false;
        SoundManager.Instance.PlaySound("gameover", ESoundType.SFX, 3);
        UIManager.Instance.GameOver();
    }

    public void CameraShake(float power, float duration)
    {
        StartCoroutine(CameraShakeCoroutine(power, duration));
    }

    IEnumerator CameraShakeCoroutine(float power, float duration)
    {
        cameraShakePower += power;
        yield return new WaitForSeconds(duration);
        cameraShakePower -= power;
    }

    private void KillUpdate()
    {
        if (feverInvDuration > 0) return; ;

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                var enemy = enemies[0];
                float inputDirection = Mathf.RoundToInt(keyCodes.Length / (Player.Instance.level));
                if (Mathf.FloorToInt(i / inputDirection) == enemy.Direction || feverDuration > 0)
                {
                    enemy.OnHit();
                    timer = 0;
                    CameraShake(0.1f, 0.05f);

                    var slashEffect = PoolManager.Instance.Init("Slash Effect");
                    slashEffect.transform.SetParent(Player.Instance.transform);
                    slashEffect.transform.localPosition = Vector3.up;

                    Player.Instance.Attack();
                    Player.Instance.transform.DOMoveX(enemy.transform.position.x, 0.2f);
                }
                else
                {
                    combo = 0;
                    Player.Instance.Hp--;
                    SoundManager.Instance.PlaySound("hurt2");
                    SoundManager.Instance.PlaySound("player");
                }
            }
        }
    }

    private Enemy GetNewEnemy()
    {
        Enemy enemy;
        if (Random.Range(0, 10) == 0)
            enemy = PoolManager.Instance.Init("Upgrade Enemy").GetComponent<Enemy>();
        else
            enemy = PoolManager.Instance.Init("Enemy").GetComponent<Enemy>();
        enemy.Init();
        enemies.Add(enemy);
        return enemy;
    }

    private void EnemyMoveUpdate()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Move(i);
        }
    }

    public void KillEnemy(Enemy killEnemy)
    {
        enemies.Remove(killEnemy);

        var particle = PoolManager.Instance.Init("Hit Effect");
        particle.transform.position = killEnemy.transform.position;

        GetNewEnemy();

        Player.Instance.Exp += 10 + Random.Range(0f, 5f);
        if (feverDuration <= 0)
        {
            Player.Instance.SkillGauge += Random.Range(0f, 2f);
            maxTimer *= 0.999f;
        }


        combo++;
        UIManager.Instance.UpdateCombo(combo);

        EnemyMoveUpdate();
    }

    public void Fever()
    {
        isGaming = false;
        SoundManager.Instance.PlaySound("levelup2");

        StartCoroutine(TextWaitCoroutine());
        feverDuration = feverMaxDuration;
        UIManager.Instance.ActiveSkill();

        feverParticle.Play();
        feverParticle.gameObject.SetActive(true);
    }
}
