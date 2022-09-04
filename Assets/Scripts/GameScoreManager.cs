using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

// 해당 스크립트에서 스코어는 마스터 클라이언트에서만 관리함
public class GameScoreManager : MonoBehaviour, IPunObservable
{
    private static GameScoreManager _instance;
    public static GameScoreManager Instance { get { return _instance; } }

    PhotonView photonView;
    
    [SerializeField]
    private int leftPlayerScore = 0;
    [SerializeField]
    private int rightPlayerScore = 0;

    private bool playRound = true;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 소유자가 다른사람에게 데이터 보내기
            stream.SendNext(leftPlayerScore);
            stream.SendNext(rightPlayerScore);
        }
        else
        {
            // 클라이언트가 데이터 받기
            this.leftPlayerScore = (int)stream.ReceiveNext();
            this.rightPlayerScore = (int)stream.ReceiveNext();
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

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void LeftPlayerScoreUP()
    {
        if(playRound == true)
        {
            leftPlayerScore++;
            Debug.Log("LeftPlayer의 점수가 올랏다 !");
            playRound = false;
        }
    }

    public void RightPlayerScoreUp()
    {
        if (playRound == true)
        {
            rightPlayerScore++;
            Debug.Log("RightPlayer의 점수가 올랏다 !");
            playRound = false;
        }
    }

    public void StartRound()
    {
        playRound = true;
    }
}
