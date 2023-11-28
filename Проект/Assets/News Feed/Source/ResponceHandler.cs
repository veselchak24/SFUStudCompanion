using System.Collections.Generic;
using UnityEngine.Assertions;
using HtmlAgilityPack;

/// <summary>
/// Класс, для работы с html страницами
/// (аналог bs4)
/// </summary>
class ResponceHandler
{
    private HtmlDocument doc;
    
    public ResponceHandler(string html)
    {
        this.doc = new HtmlDocument();
        this.doc.LoadHtml(html);
    }

    /// <summary>
    /// Выбирает элементы из html по ключу(ключам) <br></br>
    /// </summary>
    /// <param name="key">Вид ключа: "//element //element2[@class="class1"] ..."</param>
    /// <returns>Список html найденных элементов</returns>
    public List<string> Select(string key)
    {
        Assert.IsTrue(key != null && key.Length > 0);

        List<string> listResults = new List<string>();
        foreach (HtmlNode node in doc.DocumentNode.SelectNodes(key))
        {
            string text = node.InnerHtml;

            if (text != null && text != "")
                listResults.Add(text);
        }

        return listResults;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key">Вид ключа: "//element //element2[@class="class1"] ..."</param>
    /// <returns>Возвращает список текста найденных элементов</returns>
    public List<string> GetTextInElements(string key)
    {
        Assert.IsTrue(key != null && key.Length > 0);

        List<string> listResults = new List<string>();

        var selectedNodes = doc.DocumentNode.SelectNodes(key);

        Assert.IsTrue(selectedNodes != null && selectedNodes.Count > 0, $"ResponceHandler.GetTextInElements: empty selector for key {key}");

        foreach (HtmlNode node in selectedNodes)
        {
            if (node == null) break;

            string text = node.InnerText;

            if (text != null && text != "" && text[0] != '<')
                listResults.Add(text);
        }

        return listResults;
    }

    /// <returns>Возвращает текст внутри текущего элемента</returns>
    public string GetText() => doc.DocumentNode.InnerText;

    /// <returns>Возвращает html текущего элемента</returns>
    public string GetHtml() => doc.DocumentNode.OuterHtml;
}