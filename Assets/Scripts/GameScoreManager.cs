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

    private int leftPlayerScore = 0;
    private int rightPlayerScore = 0;

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
        leftPlayerScore++;
    }

    public void RightPlayerScoreUp()
    {
        rightPlayerScore++;
    }
}
