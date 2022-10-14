using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

// �ش� ��ũ��Ʈ���� ���ھ�� ������ Ŭ���̾�Ʈ������ ������
public class GameScoreManager : MonoBehaviour, IPunObservable
{
    private static GameScoreManager _instance;
    public static GameScoreManager Instance { get { return _instance; } }
   
    public int bluePlayerScore = 0;
    public int pinkPlayerScore = 0;
    public int winScore = 3;

    private string gameSceneManagerName = "GameSceneManager";

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

            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning("���� �ΰ� �̻��� GameScoreManager�� �����մϴ�!");
            Destroy(gameObject);
        }
    }

    public void BluePlayerScoreUP()
    {
        if(GameObject.Find(gameSceneManagerName).GetComponent<GameSceneManager>().playRound == true)
        {
            bluePlayerScore++;
            Debug.Log("BluePlayer�� ���� UP !");
            GameObject.Find(gameSceneManagerName).GetComponent<GameSceneManager>().playRound = false;
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
        if (GameObject.Find(gameSceneManagerName).GetComponent<GameSceneManager>().playRound == true)
        {
            pinkPlayerScore++;
            Debug.Log("PinkPlayer�� ���� UP !");
            GameObject.Find(gameSceneManagerName).GetComponent<GameSceneManager>().playRound = false;
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
}
