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
    
    [SerializeField]
    private int leftPlayerScore = 0;
    [SerializeField]
    private int rightPlayerScore = 0;

    private bool playRound = true;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����ڰ� �ٸ�������� ������ ������
            stream.SendNext(leftPlayerScore);
            stream.SendNext(rightPlayerScore);
        }
        else
        {
            // Ŭ���̾�Ʈ�� ������ �ޱ�
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
            Debug.LogWarning("���� �ΰ� �̻��� GameScoreManager�� �����մϴ�!");
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
            Debug.Log("LeftPlayer�� ������ �ö��� !");
            playRound = false;
        }
    }

    public void RightPlayerScoreUp()
    {
        if (playRound == true)
        {
            rightPlayerScore++;
            Debug.Log("RightPlayer�� ������ �ö��� !");
            playRound = false;
        }
    }

    public void StartRound()
    {
        playRound = true;
    }
}
