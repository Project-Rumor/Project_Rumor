﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterCtrl : MonoBehaviourPunCallbacks
{
    [SerializeField]
    float moveSpeed = 10.0f;
    float hAxis = 0.0f;
    float vAxis = 0.0f;

    CharData chardata;

    public PhotonView PV;
    public SpriteRenderer SR;

    public float AttackRange = 5.0f;

    protected virtual void Start()
    {
        //chardata = TitleData.instance.charDatas["Char_Gumiho"];

        //moveSpeed = chardata.speed;

        PV = GetComponent<PhotonView>();
        SR = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (PV.IsMine)
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Kill();
            }
        }
    }

    public virtual void Move()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(hAxis * Time.deltaTime * moveSpeed, vAxis * Time.deltaTime * moveSpeed, 0));

        if (hAxis != 0)
        {
            PV.RPC("FilpXRPC", RpcTarget.AllBuffered, hAxis);
        }
    }

    public virtual void Kill()
    {
        GameObject otherPlayer = GetNearestPlayer(AttackRange);

        if (otherPlayer != null)
        {
            otherPlayer.GetComponent<CharacterCtrl>().Die();
        }
    }

    public virtual void PassiveSkill()
    {

    }

    public virtual void ActiveSkill()
    {

    }

    public virtual void Die()
    {
        PV.RPC("DestroyPlayer", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void FilpXRPC(float axis)
    {
        SR.flipX = (axis == -1);
    }

    [PunRPC]
    void DestroyPlayer()
    {
        Destroy(this.gameObject);
    }

    GameObject GetNearestPlayer(float range)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearestPlayer = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject otherPlayer in players)
        {
            if (otherPlayer == gameObject)
                continue;

            float dist = Vector3.Distance(transform.position, otherPlayer.transform.position);

            if (dist < minDist && dist < range)
            {
                nearestPlayer = otherPlayer;
            }
        }

        return nearestPlayer;
    }
}