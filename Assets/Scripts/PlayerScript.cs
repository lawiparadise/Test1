using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    public SpriteRenderer SR;
    public Animator AN;
    public TMP_Text txt;

    void Update()
    {
        // transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * Time.deltaTime * 7, 0, 0));
        if (PV.IsMine)
        {
            float axis = Input.GetAxisRaw("Horizontal");
            transform.Translate(new Vector3(axis * Time.deltaTime * 7, 0, 0));

            if (axis != 0)
            {
                PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis); // AllBuffered로 하면 나중에 들어온 플레이어도 RPC를 받게 됨
                AN.SetBool("walk", true);
            }
            else AN.SetBool("walk", false);
        }
    }

    [PunRPC] // 방에 있는 모든 자신들아 이거 실행해!
    void FlipXRPC(float axis)
    {
        SR.flipX = axis == -1;
    }

    [ContextMenu("더하기")]
    public void Plus() => txt.text = (int.Parse(txt.text) + 1).ToString();

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(txt.text); // Object를 받을 수 있음
            // 2번째 넘겨주는건
        }
        else
        {
            txt.text = (string)stream.ReceiveNext();
            // 2번째에서 받아야 한다
        }
    }
}