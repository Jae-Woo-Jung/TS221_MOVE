using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddRewardController : MonoBehaviour
{
    public FireStoreRewardList fireStroeRewardList;

    public GameObject rewardPrefab;
    [Tooltip("보상목록의 content")]
    public GameObject content;


    public TMP_InputField titleInput;
    public TMP_InputField contentInput;
    public TMP_InputField pointInput;


    public void addReward()
    {
        int point = 0;
        if (!System.Int32.TryParse(pointInput.text, out point)
            || point < 1 || point > 9999 || titleInput.text.Length == 0)
        {
            Debug.LogError("Point should be 1~9999");
            return;
        }

        GameObject reward = Instantiate(rewardPrefab, content.transform);

        RewardController script = reward.GetComponent<RewardController>();

        script.level.text = totalLevel().ToString();
        script.rewardTitle.text = titleInput.text;
        script.detailText.text = "-" + contentInput.text;
        script.GoalPoint.text = pointInput.text + "P";
        script.check.SetActive(false);

        reward.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f * (totalLevel() - 1));
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, reward.GetComponent<RectTransform>().rect.yMin);

        fireStroeRewardList.SendRewardList(totalLevel());


        titleInput.text = "";
        contentInput.text = "";
        pointInput.text = "";


    }

    private int totalLevel()
    {
        return content.transform.childCount - 1;
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
