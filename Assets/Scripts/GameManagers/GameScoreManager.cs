using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

// 해당 스크립트에서 스코어는 마스터 클라이언트에서만 관리함
public class GameScoreManager : MonoBehaviour, IPunObservable
{
    private static GameScoreManager _instance;
    public static GameScoreManager Instance { get { return _instance; } }
   
    public int bluePlayerScore = 0;
    public int pinkPlayerScore = 0;
    public int winScore = 3;
    // 점수가 2번 올라가는걸 방지하기위해 현재 게임이 진행되고 있는지를 확인하는 함수
    private bool playRound = true;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 소유자가 다른사람에게 데이터 보내기
            stream.SendNext(bluePlayerScore);
            stream.SendNext(pinkPlayerScore);
        }
        else
        {
            // 클라이언트가 데이터 받기
            this.bluePlayerScore = (int)stream.ReceiveNext();
            this.pinkPlayerScore = (int)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 GameScoreManager가 존재합니다!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Debug.Log($"Blue Score : {bluePlayerScore}");
        Debug.Log($"Pink Score : {pinkPlayerScore}");
    }

    public void BluePlayerScoreUP()
    {
        if(playRound == true)
        {
            bluePlayerScore++;
            Debug.Log("BluePlayer의 점수 UP !");
            playRound = false;
        }
    }

    public bool BluePlayerWinCheck()
    {
        if(bluePlayerScore >= winScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PinkPlayerScoreUp()
    {
        if (playRound == true)
        {
            pinkPlayerScore++;
            Debug.Log("PinkPlayer의 점수 UP !");
            playRound = false;
        }
    }

    public bool PinkPlayerWinCheck()
    {
        if (pinkPlayerScore >= winScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StartRound()
    {
        playRound = true;
    }

    public bool GetPlayRound()
    {
        return playRound;
    }
}
