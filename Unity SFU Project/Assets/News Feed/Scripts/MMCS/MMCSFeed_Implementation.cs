using HtmlAgilityPack;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

using IEnumerator = System.Collections.IEnumerator;
using WebException = System.Net.WebException;

//���������� MMCSFeed
partial class MMCSFeed
{
    public partial List<NewsNode> GetNewsNodes() => this.newsNodes;

    private partial IEnumerator GetHTMLFromMMCS(int startNewsIndex)
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

    public partial IEnumerator LoadNewsNodes(int startIndex = 1, int count = 9)
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
                IEnumerator htmlCoroutine = GetHTMLFromMMCS(startIndex);
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
                    ResponceHandler newsHandler = new ResponceHandler(htmlNews);

                    // ��������, ��� �� �� ���������� �� ���������� ������ �������(�� ����, �.�. ��� ���������(��, ���������))
                    if (!htmlNews.Contains("createby"))
                        continue;

                    //�������� ������ � ��������� � ������ ������� �������, ������� �������� �� html. [0] - �.�. htmlNews ��� ������ ������ ��� ����
                    string headerNews = newsHandler.SelectChild(headerSelector).GetInnerText().Trim();
                    string authorNews = newsHandler.SelectChild(authorSelector).GetInnerText().Trim();
                    string dateNews = newsHandler.SelectChild(dateSelector).GetInnerText().Trim();
                    string contentNews = newsHandler.SelectChild(contentSelector).GetInnerText().Trim();
                    string href = newsHandler.SelectChild(buttonSelector)?.GetAttributeValue("href");

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

// ���������� NewsNode
partial class MMCSFeed
{
    public partial class NewsNode
    {
        public partial IEnumerator GetFullNews()
        {
            var htmlCoroutine = this.GetHrefHTML();
            yield return htmlCoroutine;
            ResponceHandler responce = (ResponceHandler)htmlCoroutine.Current;

            string NewsSelector = "//div[@class=\"news_item_a\"]";
            string contentSelector = "//div[@class=\"newsitem_text\"]";
            string modifydateSelector = "//span[@class=\"modifydate\"]";

            ResponceHandler newsHandler = responce.SelectChild(NewsSelector);

            if (newsHandler == null)
                throw new HtmlWebException("Error download NewsHandler");

            string contentNews = newsHandler.SelectChild(contentSelector).GetInnerText().Trim();
            string modifydateNews = newsHandler.SelectChild(modifydateSelector)?.GetInnerText().Trim();

            yield return new NewsNode(this.header, modifydateNews != null ? modifydateNews : this.createDate, this.author, contentNews);
        }

        private partial IEnumerator GetHrefHTML()
        {
            if (this.href == null || this.href.Length == 0)
                throw new ArgumentException($"href content {this.header} not exists");

            Debug.Log("https://mmcs.sfedu.ru/" + this.href);

            //using (UnityWebRequest webRequest = new UnityWebRequest("https://mmcs.sfedu.ru/" + this.href))
            string st = "https://mmcs.sfedu.ru/148-news/2619-����-2024";

            using (UnityWebRequest webRequest = new UnityWebRequest(st))
            {
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(st);
                UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
                DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

                webRequest.uploadHandler = uH;
                webRequest.downloadHandler = dH;

                // ���������� ������
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.InProgress)
                    new WebException("webRequest in progress. ��� ���� �� ������");

                if (webRequest.result != UnityWebRequest.Result.Success)
                    new WebException("Network error from conection to mmcs.sfedu.ru");

                if (webRequest.downloadHandler.text != "")
                    yield return new ResponceHandler(webRequest.downloadHandler.text);
                else
                    new WebException($"�� ������� �������� href ������� ����� {this.header} �� ������� ..{this.href}");
            }
        }
    }
}