using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class PlayerDebuff : MonoBehaviour, IDebuff
{
    private PlayerState playerState;
    private SpriteRenderer spriteRenderer;
    private PhotonView photonView;

    private int damageCount;
    private bool isPoisonState = false;
    private float poisonDamageDelay = 0.5f;

    private float moveFreezeRunTime = 0f;
    private float moveFreezeTime;
    private bool isMoveFreezeState = false;

    [SerializeField]
    private Image freezeProgressBar;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        photonView = GetComponent<PhotonView>();
    }
    
    // ?Ѿ??? ?i?? ??ü?? ?ش? ?Լ??? ??????Ų??.
    public void StartPoison(float PoisonDamage, int DamageCount)
    {
        this.damageCount = DamageCount;
        if(isPoisonState == false)
        {
            StartCoroutine("Poison", PoisonDamage);
        }
    }

    IEnumerator Poison(float PoisionDamage)
    {
        while (damageCount > 0)
        {            
            photonView.RPC("ChangePoisonState", RpcTarget.All, 0f, 255f, 0f, true);
            playerState.GetDamage(PoisionDamage);

            damageCount--;

            if (playerState.HP <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(poisonDamageDelay);
        }

        photonView.RPC("ChangePoisonState", RpcTarget.All, 255f, 255f, 255f, false);
    }

    [PunRPC]
    void ChangePoisonState(float SpriteColorR, float SpriteColorG, float SpriteColorB, bool isPoisonState)
    {
        spriteRenderer.color = new Color(SpriteColorR, SpriteColorG, SpriteColorB);

        this.isPoisonState = isPoisonState;
    }

    public void StartMoveFreeze(float FreezeTime)
    {
        photonView.RPC("StartMoveFreezeRPC", RpcTarget.All, FreezeTime);
    }

    [PunRPC]
    void StartMoveFreezeRPC(float FreezeTime)
    {
        if(photonView.IsMine == false)
        {
            return;
        }

        moveFreezeTime = FreezeTime;
        if (isMoveFreezeState == false)
        {
            StartCoroutine(MoveFreeze());
        }
        else
        {
            moveFreezeRunTime = 0.0f;
        }
    }

    IEnumerator MoveFreeze()
    {
        isMoveFreezeState = true;
        GetComponent<PlayerMove>().canMove = false;
        moveFreezeRunTime = 0.0f;
        photonView.RPC("SettingfreezeProgressBarFillAmount", RpcTarget.All, 1.0f);
        freezeProgressBar.fillAmount = 1.0f;

        while (moveFreezeRunTime < moveFreezeTime)
        { 
            moveFreezeRunTime += Time.deltaTime;
            photonView.RPC("SettingfreezeProgressBarFillAmount", RpcTarget.All, 1.0f - moveFreezeRunTime / moveFreezeTime);
            //freezeProgressBar.fillAmount = 1.0f - moveFreezeRunTime / moveFreezeTime;
            yield return null;
        }

        GetComponent<PlayerMove>().canMove = true;
        photonView.RPC("SettingfreezeProgressBarFillAmount", RpcTarget.All, 0.0f);
        isMoveFreezeState = false;
    }

    [PunRPC]
    void SettingfreezeProgressBarFillAmount(float amount)
    {
        freezeProgressBar.fillAmount = amount;
    }
}
