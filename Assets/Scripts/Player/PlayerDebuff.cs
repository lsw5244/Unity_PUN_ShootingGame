using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerDebuff : MonoBehaviour
{
    private PlayerState playerState;
    private SpriteRenderer spriteRenderer;
    private PhotonView photonView;

    private int damageCount;
    private bool isPoisonState = false;

    private float poisonDamageDelay = 0.2f;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        photonView = GetComponent<PhotonView>();
    }
    
    public void StartPoisionEffect(float PoisionDamage, int DamageCount)
    {
        this.damageCount = DamageCount;
        if(isPoisonState == false)
        {
            // 코루틴 시작
        }
    }

    IEnumerator Poison(float PoisionDamage)
    {
        isPoisonState = true;
        spriteRenderer.color = new Color(0f, 255f, 0f);

        while (damageCount > 0)
        {
            if(playerState.HP <= 0)
            {
                break;
            }

            playerState.GetDamage(PoisionDamage);

            yield return new WaitForSeconds(poisonDamageDelay);
            damageCount--;
        }

        spriteRenderer.color = new Color(255f, 255f, 255f);
        isPoisonState = false;
    }
    /*
    public void StartPoisonEffect(float damage)
    {
        poisonDamageCount = 5;

        if (_isPoisonState == false)
        {
            StartCoroutine(Poison(damage));
        }
    }

    protected IEnumerator Poison(float damage)
    {
        _poisonParicle.SetActive(true);
        _isPoisonState = true;

        while (poisonDamageCount > 0)
        {
            if( _isAlive == false )
            {
                break;
            }

            currentHp -= damage;
            UIManager.Instance.UpdateMonsterHpbar(currentHp / _maxHP, gameObject.name);

            if (currentHp <= 0f && _isAlive == true)
            {
                Die();
                break;
            }

            yield return new WaitForSeconds(poisonDamageDelay);     // 0.5초에 한 번씩 실행되도록

            poisonDamageCount--;
        }

        _isPoisonState = false;
        _poisonParicle.SetActive(false);
    }
     */
}
