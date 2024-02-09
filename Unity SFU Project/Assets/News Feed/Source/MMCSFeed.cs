//#define DEBUG

/* MMCSFEED
 �������� �����, ��� ��������� ������ � ����� https://mmcs.sfedu.ru/

 �������� �������:
    -����� �������� ������� ������, ����� ������� obj.LoadNewsNodes(), ����� ����� ������ ��� ����� �� ��������� �������
    !���������! ���� ��� ���������� ����� � ������ ��������

    

 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

/// <summary>
/// �����, ��� ��������� ������ � ����� �������
/// </summary>
class MMCSFeed
{
    /// <summary>
    /// ������ ���� ���������� ������(���������: �� ����� � ������)
    /// </summary>
    private List<NewsNode> newsNodes = new List<NewsNode>();

    /// <summary>
    /// ���������� ������ ���� ���������� ������, ����. �� �������
    /// </summary>
    public List<NewsNode> GetNewsNodes() => this.newsNodes;

    /// <summary>
    /// �����, ���������� � ���� ������ ����� � ����� MMCS
    /// </summary>
    public class NewsNode
    {
        public readonly string header, createDate, author, content;

        /// <summary>
        /// ������ �� ����������� �����
        /// </summary>
        public readonly string? href;

        public NewsNode(string header, string createTime, string author, string content, string? href = null)
        {
            this.header = header;
            this.createDate = createTime;
            this.author = author;
            this.content = content;
            this.href = href;
        }
    }


    /// <summary>
    /// �������� HTML �� �������� ������-��������
    /// </summary>
    /// <param name="startNewsIndex">� ����� ������� ��������� ��������</param>
    /// <returns></returns>
    private IEnumerator GetHTMLFromMMCS(int startNewsIndex)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"https://mmcs.sfedu.ru/?start={startNewsIndex}"))
        {
            // ���������� ������
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.InProgress)
                new Exception("webRequest in progress. ��� ���� �� ������");

            if (webRequest.result != UnityWebRequest.Result.Success)
                new Exception("Network error from conection to mmcs.sfedu.ru");

            if (webRequest.downloadHandler.text != "")
                yield return new ResponceHandler(webRequest.downloadHandler.text);
            else
                yield return new ResponceHandler(Resources.Load<TextAsset>("mmcs").text);
        }
    }

    /// <summary>
    /// �������������.�������� � ������������ ����� � ����� MMCS
    /// </summary>
    /// <param name="startIndex">����� ������� ����� � �����</param>
    /// <param name="count">���������� ������</param>
    public IEnumerator LoadNewsNodes(int startIndex = 1, int count = 9)
    {
        Assert.IsTrue(startIndex > 0); // ����������: 0 - ������ ����� �������
        Assert.IsTrue(count > 0);

        // ������ �� ����� ������� ������������ 9 ��������
        int countPages = Mathf.CeilToInt(count / 9f);

        this.newsNodes.Clear();

        // ����������� �� �������� �� ���������: ������� ������� + 9
        for (int startNewsIndex = startIndex; startNewsIndex <= count + startIndex - 1; startNewsIndex += 9)
            if (startNewsIndex + startIndex - 1 > count)
                break;
            else
            {
                var htmlCoroutine = GetHTMLFromMMCS(startIndex);
                yield return htmlCoroutine;
                ResponceHandler responce = (ResponceHandler)htmlCoroutine.Current;

                string NewsSelector = "//div[@class=\"news_item_f\"]";
                string headerSelector = "//h2";
                string contentSelector = "//div[@class=\"newsitem_text\"]";
                string dateSelector = "//span[@class=\"createdate\"]";
                string authorSelector = "//span[@class=\"createby\"]";
                string buttonSelector = "a";

                List<string> ListSelectedNews = responce.Select(NewsSelector);

                Assert.IsTrue(ListSelectedNews.Count > 0, "Error download News. Mb u parse from undefined page");

                int countNews = (9 * (countPages - 1) + 1) - startIndex;

                if ((countNews < 9) && countPages != 1)
                    ListSelectedNews.RemoveRange(countNews, ListSelectedNews.Count - countNews + 1);

                //Fill newsNode
                foreach (var htmlNews in ListSelectedNews)
                {
                    ResponceHandler handlerNews = new ResponceHandler(htmlNews);

                    // ��������, ��� �� �� ���������� �� ���������� ������ �������(�� ����, �.�. ��� ���������(��, ���������))
                    if (!htmlNews.Contains("createby"))
                        continue;

                    //�������� ������ � ��������� � ������ ������� �������, ������� �������� �� html. [0] - �.�. htmlNews ��� ������ ������ ��� ����
                    string headerNews = handlerNews.SelectChild(headerSelector).GetInnerText().Trim();
                    string authorNews = handlerNews.SelectChild(authorSelector).GetInnerText().Trim();
                    string dateNews = handlerNews.SelectChild(dateSelector).GetInnerText().Trim();
                    string contentNews = handlerNews.SelectChild(contentSelector).GetInnerText().Trim();
                    string? href = handlerNews.SelectChild(buttonSelector)?.GetAttributeValue("href");

                    if (href != null) 
                        Debug.Log(href);

                    // ���� ���-�� ������ � �����(�� ����, ����� �� ����� ����), ����������� � ���� ���������
                    if (headerNews.Length == 0 || dateNews.Length == 0 || authorNews.Length == 0 || contentNews.Length == 0)
                        continue;

                    // ��������� ���������� ���� � ������
                    // ��������, ��� �������� ����������� � ������� �������
                    this.newsNodes.Add(new NewsNode(headerNews, dateNews, authorNews, contentNews, href));
                }
            }
    }
}