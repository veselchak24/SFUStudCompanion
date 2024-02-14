//#define DEBUG

/* MMCSFEED
 Содержит класс, для получения постов с сайта https://mmcs.sfedu.ru/

Как использовать(пример в TemplateFeed):
    1) Создать экземпляр MMCSFeed и вызвать yield return StartCoroutine(mmcsObj.LoadNewsNodes(startIndexParsing, countNews));
    2) создать List<MMCSFeed.NewsNode> и присвоить ему mmcsObj.GetNewsNodes()
    
    Вы получили список постов с сайта мехмата
    
    !ЗАМЕЧАНИЕ!
    Полный текст постов можно получить следующим образом:
    
    IEnumirator corutine = StartCorutine(newsNodeObj.GetFullNews());
    yield return corutine;
    NewsNode fullNews = (NewsNode)corutine.Current;
     
    !Замечание 1.2!
    У полной новости может отличаться дата создания и быть равна = "Обновлено: XX.XX.XX XX:XX"
 */

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Класс, для получения постов с сайта мехмата
/// </summary>
partial class MMCSFeed
{
    /// <summary>
    /// Список всех полученных постов(сортирова: от новых к старым)
    /// </summary>
    private List<NewsNode> newsNodes = new List<NewsNode>();

    /// <summary>
    /// Возвращает список всех полученных постов, сорт. от новизны
    /// </summary>
    public partial List<NewsNode> GetNewsNodes();

    /// <summary>
    /// Класс, содержащий данные поста с сайта MMCS
    /// </summary>
    public partial class NewsNode
    {
        public readonly string header, createDate, author, content;

        /// <summary>
        /// Ссылка на расширенный текст
        /// </summary>
        public readonly string href = "";

        public NewsNode(string header, string createTime, string author, string content, string href = null)
        {
            this.header = header;
            this.createDate = createTime;
            this.author = author;
            this.content = content;
            this.href = href;
        }

        /// <summary>
        /// Возвращает полную новость, если имеется такая возможность(кнопка "Подробнее")
        /// В противном случае - null
        /// </summary>
        /// <returns>объект new NewsNode(this.header, modifydateNews != null ? modifydateNews : this.createDate, this.author, contentNews)</returns>
        public partial IEnumerator GetFullNews();

        /// <summary>
        /// Возвращает html страницу полной новости
        /// </summary>
        /// <exception cref="ArgumentException">Полной новости не существует(она и так полная)</exception>
        /// <exception cref="System.Net.WebException">Ошибка получения ответа от сервера(страницы)</exception>
        private partial IEnumerator GetHrefHTML();
    }

    /// <summary>
    /// Полечает HTML со страницы Мехмат-новостей
    /// </summary>
    /// <param name="startNewsIndex">с какой новости загружать страницу</param>
    /// <returns></returns>
    private partial IEnumerator GetHTMLFromMMCS(int startNewsIndex);

    /// <summary>
    /// Инициализатор.Получает и обрабатывает посты с сайта MMCS
    /// </summary>
    /// <param name="startIndex">Номер первого поста с сайта</param>
    /// <param name="count">Количество постов</param>
    public partial IEnumerator LoadNewsNodes(int startIndex = 1, int count = 9);
}