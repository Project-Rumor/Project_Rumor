using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterCtrl : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public float moveSpeed = 10.0f;
    float hAxis = 0.0f;
    float vAxis = 0.0f;

    public Light2D Sight; 

    public CharData chardata;
    public string target;

    public PhotonView PV;
    public SpriteRenderer SR;
    public SpriteRenderer CSR;
    public Animator Anim;
    public Animator ColorAnim;
    public SkillPanel skillPanel;

    public float attackRange = 5.0f;
    int reverse = 1;

    protected virtual void Start()
    {
        //moveSpeed = chardata.speed;

        PV = GetComponent<PhotonView>();
        SR = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
        skillPanel = GameObject.Find("Panel_Skill").GetComponent<SkillPanel>();
    }

    public void Setup(string _code)
    {
        chardata = TitleData.instance.charDatas[_code];
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

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ActiveSkill();
            }

            if(Input.GetKeyDown(KeyCode.F))
            {
                getClue();
            }
        }
    }

    public virtual void Move()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(hAxis * Time.deltaTime * moveSpeed * reverse, vAxis * Time.deltaTime * moveSpeed * reverse, 0));

        if (hAxis != 0)
        {
            PV.RPC("FilpXRPC", RpcTarget.AllBuffered, hAxis);
        }

        if (hAxis !=0 || vAxis != 0)
        {
            Anim.SetBool("IsMove", true);
            ColorAnim.SetBool("IsMove", true);
        }
        else
        {
            Anim.SetBool("IsMove", false);
            ColorAnim.SetBool("IsMove", false);
        }
    }

    public virtual void Kill()
    {
        if (skillPanel.KillAction() == false)
            return;

        GameObject otherPlayer = GetNearestPlayer(attackRange);

        if (otherPlayer != null)
        {
            if (otherPlayer.GetComponent<CharacterCtrl>().chardata.code == target)
            {
                SoundManager.instance.PlaySFX("Kill");
                otherPlayer.GetComponent<CharacterCtrl>().Die();

                if (chardata.resource == "Reaper")
                {
                    GetComponent<ReaperAbility>().ReaperPassive();
                }
            }
        }
    }

    public virtual void getClue()
    {
        GameObject clue = GetNearestClue(attackRange);

        if(clue != null)
        {
            clue.GetComponent<Clue>().Interactive();
        }
    }

    public virtual GameObject GetNearestClue(float range)
    {
        GameObject[] Clues = GameObject.FindGameObjectsWithTag("Clue");

        GameObject nearestClue = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject c in Clues)
        {
            float dist = Vector3.Distance(transform.position, c.transform.position);

            if (dist < minDist && dist < range)
            {
                nearestClue = c;
            }
        }

        return nearestClue;
    }

    public virtual void PassiveSkill()
    {

    }

    public virtual void ActiveSkill()
    {
        if (skillPanel.SkillAction() == false)
            return;

        GetComponent<Ability>().Active();
    }

    public virtual void Die()
    {
        InGameManager.instance.SomeOneDied(this);

        PV.RPC("DestroyPlayer", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void FilpXRPC(float axis)
    {
        SR.flipX = (axis == -1);
        CSR.flipX = (axis == -1);
    }

    [PunRPC]
    public void DestroyPlayer()
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

    public IEnumerator MoveSpeedChange(float time, float value)
    {
        moveSpeed += value;

        yield return new WaitForSeconds(time);

        moveSpeed -= value;
    }

    public IEnumerator SightChange(float time, float value)
    {
        Sight.intensity += value;

        yield return new WaitForSeconds(time);

        Sight.intensity -= value;
    }
}
