//#define DEBUG

/* MMCSFEED
 Содержит класс, для получения постов с сайта https://mmcs.sfedu.ru/

 Основные моменты:
    -После создания объекта класса, нужно вызвать obj.LoadNewsNodes(), чтобы класс собрал все посты со странички мехмата
    !ЗАМЕЧАНИЕ! Пока что собираются посты с первой страницы

    

 */

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Класс, для получения постов с сайта мехмата
/// </summary>
class MMCSFeed
{
    /// <summary>
    /// Список всех полученных постов(сортирова: от новых к старым)
    /// </summary>
    private List<NewsNode> newsNodes = new List<NewsNode>();

    /// <summary>
    /// Возвращает список всех полученных постов, сорт. от новизны
    /// </summary>
    public List<NewsNode> GetNewsNodes() => this.newsNodes;

    /// <summary>
    /// Класс, содержащий в себе данные поста с сайта MMCS
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
    /// Инициализатор. Получает и обрабатывает посты с сайта MMCS
    /// </summary>
    public void LoadNewsNodes()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://mmcs.sfedu.ru/"))
        {
            // Отправляем запрос
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
                new Exception("webRequest in progress. Так быть не должно");

            if (webRequest.result != UnityWebRequest.Result.Success)
                new Exception("Network error from conection to mmcs.sfedu.ru");

            ResponceHandler responce;

            if (webRequest.downloadHandler.text != "")
                responce = new ResponceHandler(webRequest.downloadHandler.text);
            else
                responce = new ResponceHandler(Resources.Load<TextAsset>("mmcs").text);

            //У меня в санатории не работал нормально интернет, пришлось написать костыль
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

                // Проверка, что мы не наткнулись на совершенно другой элемент(не пост, т.к. нет создателя(да, костыльно))
                if (!htmlNews.Contains("createby"))
                    continue;

                //Получчаю тексты в элементах и удаляю лишнние пробелы, которые остаются от html. [0] - т.к. htmlNews даёт только НУЖНЫЙ НАМ пост
                string headerNews = handlerNews.GetTextInElements(headerSelector)[0].Trim();
                string authorNews = handlerNews.GetTextInElements(authorSelector)[0].Trim();
                string dateNews = handlerNews.GetTextInElements(dateSelector)[0].Trim();
                string contentNews = handlerNews.GetTextInElements(contentSelector)[0].Trim();

                // Если что-то пустое в посте(не знаю, может ли такое быть), заканчивает с этим элементом
                if (headerNews.Length == 0 || dateNews.Length == 0 || authorNews.Length == 0 || contentNews.Length == 0)
                    continue;

                // Добавляем полученный пост в список
                // Очевидно, что элементы сортируются в порядке новизны
                this.newsNodes.Add(new NewsNode(headerNews, dateNews, authorNews, contentNews));
            }
        }
    }
}