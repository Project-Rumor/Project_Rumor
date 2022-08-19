using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    float moveSpeed = 10.0f;
    float hAxis = 0.0f;
    float vAxis = 0.0f;

    public PhotonView PV;
    public SpriteRenderer SR;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        SR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(PV.IsMine)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            vAxis = Input.GetAxisRaw("Vertical");

            transform.Translate(new Vector3(hAxis * Time.deltaTime * moveSpeed, vAxis * Time.deltaTime * moveSpeed, 0));

            if (hAxis != 0)
            {
                PV.RPC("FilpXRPC", RpcTarget.AllBuffered, hAxis);
            }
        }
    }

    [PunRPC]
    void FilpXRPC(float axis)
    {
        SR.flipX = (axis == -1);
    }
}
