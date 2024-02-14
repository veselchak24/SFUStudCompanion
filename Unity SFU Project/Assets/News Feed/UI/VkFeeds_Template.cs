using System.Collections;
using System.Linq;
using UnityEngine;
using static VKFeeds;

public class VkFeeds_Template : MonoBehaviour
{

    [SerializeField] private VkGroups currentGroup = VkGroups.Самоделка_Мехмат;
    [SerializeField] private int offsetPosts = 0;
    [SerializeField] private int countPosts = 10;
    private IEnumerator Start()
    {
        VKFeeds feed = new VKFeeds();

        yield return StartCoroutine(feed.LoadJsonPosts(currentGroup, offsetPosts, countPosts));

        Post.Response responseData = feed.responseData;

        foreach (var item in responseData.items)
        {
            print(item.text);

            if (item.attachments != null)
                print($"Прикреплённых постов: {(item.copy_history != null ? item.copy_history.Length : "0")}; Attach: Count = {item.attachments.Length}; types: {string.Join(",", item.attachments.Select(attach => attach.type))}");
        }
    }
}
