using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    [SerializeField] Image SkillFillImage;
    [SerializeField] Image KillFillImage;
    [SerializeField] Text SkillCoolText;
    [SerializeField] Text KillCoolText;

    float skillCooltime;
    float killCooltime;
    float tempSkillCool;
    float tempKillCool;

    public void Setup(string _code)
    {
        skillCooltime = TitleData.instance.charDatas[_code].ability;
        killCooltime = TitleData.instance.charDatas[_code].cooltime;

        tempSkillCool = skillCooltime;
        tempKillCool = killCooltime;

        SkillFillImage.fillAmount = 1 - (tempSkillCool / skillCooltime);
        KillFillImage.fillAmount = 1 - (tempKillCool / killCooltime);

        SkillCoolText.text = tempSkillCool.ToString("N0");
        KillCoolText.text = tempKillCool.ToString("N0");
    }

    [ContextMenu("Skill")]
    public bool SkillAction()
    {
        if (tempSkillCool > 0)
            return false;

        tempSkillCool = skillCooltime;
        SkillFillImage.fillAmount = 1 - (tempSkillCool / skillCooltime);
        SkillCoolText.text = tempSkillCool.ToString("N0");

        return true;
    }

    [ContextMenu("Kill")]
    public bool KillAction()
    {
        if (tempKillCool > 0)
            return false;

        tempKillCool = killCooltime;
        KillFillImage.fillAmount = 1 - (tempKillCool / killCooltime);
        KillCoolText.text = tempKillCool.ToString("N0");

        return true;
    }

    void Update()
    {
        if (tempSkillCool >= 0)
        {
            tempSkillCool -= Time.deltaTime;
            SkillFillImage.fillAmount = 1 - (tempSkillCool / skillCooltime);
            SkillCoolText.gameObject.SetActive(true);
            if (tempSkillCool > 1)
            {
                SkillCoolText.text = tempSkillCool.ToString("N0");
            }
            else if (tempSkillCool > 0 && tempSkillCool <= 1)
            {
                SkillCoolText.text = tempSkillCool.ToString("F1");
            }
            else
            {
                tempSkillCool = 0;
                SkillFillImage.fillAmount = 1;
                SkillCoolText.gameObject.SetActive(false);
            }
        }


        if (tempKillCool >= 0)
        {
            tempKillCool -= Time.deltaTime;
            KillFillImage.fillAmount = 1 - (tempKillCool / killCooltime);
            KillCoolText.gameObject.SetActive(true);
            if (tempKillCool > 1)
            {
                KillCoolText.text = tempKillCool.ToString("N0");
            }
            else if (tempKillCool > 0 && tempKillCool <= 1)
            {
                KillCoolText.text = tempKillCool.ToString("F1");
            }
            else
            {
                tempKillCool = 0;
                KillFillImage.fillAmount = 1;
                KillCoolText.gameObject.SetActive(false);
            }
        }
    }
}
