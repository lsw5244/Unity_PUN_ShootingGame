using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

// �ش� ��ũ��Ʈ���� ���ھ�� ������ Ŭ���̾�Ʈ������ ������
public class GameScoreManager : MonoBehaviour, IPunObservable
{
    private static GameScoreManager _instance;
    public static GameScoreManager Instance { get { return _instance; } }

    PhotonView photonView;
    
    public int bluePlayerScore = 0;
    public int pinkPlayerScore = 0;
    public int winScore = 3;

    private bool playRound = true;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����ڰ� �ٸ�������� ������ ������
            stream.SendNext(bluePlayerScore);
            stream.SendNext(pinkPlayerScore);
        }
        else
        {
            // Ŭ���̾�Ʈ�� ������ �ޱ�
            this.bluePlayerScore = (int)stream.ReceiveNext();
            this.pinkPlayerScore = (int)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;

            photonView = gameObject.AddComponent<PhotonView>();

            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning("���� �ΰ� �̻��� GameScoreManager�� �����մϴ�!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //photonView = GetComponent<PhotonView>();
    }

    public void BluePlayerScoreUP()
    {
        if(playRound == true)
        {
            bluePlayerScore++;
            Debug.Log("BluePlayer�� ���� UP !");
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
            Debug.Log("PinkPlayer�� ���� UP !");
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
