using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Text;

public class Enemy : MonoBehaviour
{
    private int direction;
    public int Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }

    private Material originMaterial;

    private StringBuilder canKeyList = new StringBuilder();
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
            return spriteRenderer;
        }
    }
    [SerializeField] private Text indexText;

    private int hp = 1;
    public int maxHp = 1;

    private readonly float[] distanceYPos = new float[]
    {
        -2.1f,
        -0.05f,
        1.45f,
        2.6f,
        3.45f
    };

    private readonly float[] distanceSize = new float[]
    {
        1.4f,
        1.1f,
        0.8f,
        0.5f,
        0.2f
    };

    private readonly Color[] distanceColors = new Color[]
    {
        Color.white,
        new Color(166/255f,166/255f,176/255f),
        new Color(133/255f,133/255f,143/255f),
        new Color(99/255f,99/255f,109/255f),
        new Color(72/255f,72/255f,82/255f),
    };

    private void Awake()
    {
        originMaterial = SpriteRenderer.material;
    }

    public void Init()
    {
        gameObject.SetActive(true);

        if (originMaterial != null)
            SpriteRenderer.material = originMaterial;

        hp = maxHp;

        Direction = Random.Range(0, Player.Instance.level);

        float directionByPos = 18f / (Player.Instance.level + 1);
        float x = (Direction + 1) * directionByPos - 9f;

        transform.position = new Vector3(x, 4);
        transform.localScale = Vector3.zero;

        UpdateDiretion();

        SpriteRenderer.color = Color.black;
    }

    public void UpdateDiretion()
    {
        canKeyList.Clear();

        if (Player.Instance.level > 1)
        {
            bool isAddComma = false;
            for (int i = 0; i < InGameManager.Instance.keyCodes.Length; i++)
            {
                float inputDirection = Mathf.RoundToInt(InGameManager.Instance.keyCodes.Length / Player.Instance.level);
                if (Mathf.FloorToInt(i / inputDirection) == Direction)
                {
                    if (isAddComma)
                        canKeyList.Append(", ");

                    canKeyList.Append(InGameManager.Instance.keyCodeStrings[i]);
                    isAddComma = true;
                }
            }
        }
        indexText.text = canKeyList.ToString();
    }
    public void OnHit()
    {
        if (hp > 0)
        {
            SoundManager.Instance.PlaySound("hurt", ESoundType.SFX);
            StartCoroutine(HitCoroutine());
            hp--;
            if (hp <= 0)
                Die();
        }
    }

    IEnumerator HitCoroutine()
    {
        spriteRenderer.material = InGameManager.Instance.flashMaterial;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = originMaterial;
    }

    public void Move(int index)
    {
        float moveDuration = 0.2f;

        transform.DOKill();
        spriteRenderer.DOKill();

        float directionByPos = 18f / (Player.Instance.level + 1);
        float x = (Direction + 1) * directionByPos - 9f;

        transform.DOMoveX(x, 0.5f);
        transform.DOMoveY(distanceYPos[index], 0.5f);
        transform.DOScale(distanceSize[index], moveDuration);

        SpriteRenderer.DOColor(distanceColors[index], moveDuration);
    }

    public void Die()
    {
        SoundManager.Instance.PlaySound("enemy", ESoundType.SFX);
        SoundManager.Instance.PlaySound("enemy 1", ESoundType.SFX, 0.6f);
        InGameManager.Instance.KillEnemy(this);
        SpriteRenderer.DOFade(0, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
