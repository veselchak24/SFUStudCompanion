//#define DEBUG

/* MMCSFEED
 �������� �����, ��� ��������� ������ � ����� https://mmcs.sfedu.ru/

 �������� �������:
    -����� �������� ������� ������, ����� ������� obj.LoadNewsNodes(), ����� ����� ������ ��� ����� �� ��������� �������
    !���������! ���� ��� ���������� ����� � ������ ��������

    

 */

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
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

        public NewsNode(string header, string createTime, string author, string content)
        {
            this.header = header;
            this.createDate = createTime;
            this.author = author;
            this.content = content;
        }
    }

    /// <summary>
    /// �������������. �������� � ������������ ����� � ����� MMCS
    /// </summary>
    public void LoadNewsNodes()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://mmcs.sfedu.ru/"))
        {
            // ���������� ������
            webRequest.SendWebRequest();

            {
                int i = 0;
                while (!webRequest.isDone)
                {
                    if (i >= 5)
                    {
                        UnityEngine.Debug.LogError("webRequest to mmcs.sfedu.ru is failed.\nWe take a resource-page");
                        break;
                    }

                    ++i;
                    new WaitForSeconds(1);
                }
            }

            if (webRequest.result == UnityWebRequest.Result.InProgress)
                new Exception("webRequest in progress. ��� ���� �� ������");

            if (webRequest.result != UnityWebRequest.Result.Success)
                new Exception("Network error from conection to mmcs.sfedu.ru");

            ResponceHandler responce;

            if (webRequest.downloadHandler.text != "")
                responce = new ResponceHandler(webRequest.downloadHandler.text);
            else
                responce = new ResponceHandler(Resources.Load<TextAsset>("mmcs").text);

            //� ���� � ��������� �� ������� ��������� ��������, �������� �������� �������
            //ResponceHandler responce = new ResponceHandler(File.ReadAllText("Assets/News Feed/Source/mmcs.html"));

            string NewsSelector = "//div[@class=\"news_item_f\"]";
            string headerSelector = "//h2";
            string contentSelector = "//div[@class=\"newsitem_text\"] //p";
            string dateSelector = "//span[@class=\"createdate\"]";
            string authorSelector = "//span[@class=\"createby\"]";

            //Fill newsNode
            foreach (var htmlNews in responce.Select(NewsSelector))
            {
                ResponceHandler handlerNews = new ResponceHandler(htmlNews);

                // ��������, ��� �� �� ���������� �� ���������� ������ �������(�� ����, �.�. ��� ���������(��, ���������))
                if (!htmlNews.Contains("createby"))
                    continue;

                //�������� ������ � ��������� � ������ ������� �������, ������� �������� �� html. [0] - �.�. htmlNews ��� ������ ������ ��� ����
                string headerNews = handlerNews.GetTextInElements(headerSelector)[0].Trim();
                string authorNews = handlerNews.GetTextInElements(authorSelector)[0].Trim();
                string dateNews = handlerNews.GetTextInElements(dateSelector)[0].Trim();
                string contentNews = handlerNews.GetTextInElements(contentSelector)[0].Trim();

                // ���� ���-�� ������ � �����(�� ����, ����� �� ����� ����), ����������� � ���� ���������
                if (headerNews.Length == 0 || dateNews.Length == 0 || authorNews.Length == 0 || contentNews.Length == 0)
                    continue;

                // ��������� ���������� ���� � ������
                // ��������, ��� �������� ����������� � ������� �������
                this.newsNodes.Add(new NewsNode(headerNews, dateNews, authorNews, contentNews));
            }
        }
    }
}