using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TemplateFeed : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image templateNews;
    [SerializeField] private Canvas canvasParent;

    [Header("Test params")]
    [SerializeField] private int startIndexParsing = 1;
    [SerializeField] private int countNews = 9;

    private List<Image> NewsPosts;

    private void Start()
    {
        MMCSFeed feed = new MMCSFeed();
        this.NewsPosts = new List<Image>();

        feed.LoadNewsNodes(startIndexParsing,countNews);

        List<MMCSFeed.NewsNode> newsNodes = feed.GetNewsNodes();

        Assert.IsTrue(newsNodes != null && newsNodes.Count > 0, "newsNodes is empry or null");

        Text HeaderText = templateNews.GetComponentsInChildren<Text>()[0];
        Text AuthorText = templateNews.GetComponentsInChildren<Text>()[1];
        Text CreateDateText = templateNews.GetComponentsInChildren<Text>()[2];
        Text ContentText = templateNews.GetComponentsInChildren<Text>()[3];


        for (int i = 0; i < newsNodes.Count; i++)
        {
            MMCSFeed.NewsNode node = newsNodes[i];

            HeaderText.text = node.header;
            AuthorText.text = node.author;
            CreateDateText.text = node.createDate;
            ContentText.text = node.content;

            Image newNews = Instantiate(templateNews);
            newNews.rectTransform.SetParent(this.canvasParent.transform);
            newNews.rectTransform.localPosition = templateNews.rectTransform.localPosition - new Vector3(0, templateNews.rectTransform.rect.height * i, 0);

            this.NewsPosts.Add(newNews);
        }

    }

    private void Update()
    {
        scrollFeed();
    }

    private float y;
    private float OffsetScroll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            y = Input.mousePosition.y;
            return 0;
        }

        if (!Input.GetMouseButton(0))
            return 0;

        float offset = Input.mousePosition.y - y;
        y = Input.mousePosition.y;

        return offset;

    }

    void scrollFeed()
    {
        float offsetScroll = OffsetScroll();

        if (offsetScroll > 0)
        {
            RectTransform lastPostTransform = this.NewsPosts.Last().rectTransform;
            if (lastPostTransform.localPosition.y - lastPostTransform.rect.height / 2 >= -Screen.height / 2)
                return;
        }
        else
        {
            RectTransform firstPostTransform = this.NewsPosts.First().rectTransform;
            if (firstPostTransform.localPosition.y + firstPostTransform.rect.height / 2 <= Screen.height / 2)
                return;
        }

        if (offsetScroll == 0) return;

        foreach (Image post in this.NewsPosts)
            post.rectTransform.position += new Vector3(0, offsetScroll, 0);
    }
}
