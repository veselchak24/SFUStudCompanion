using System.Collections.Generic;
using UnityEngine.Assertions;
using HtmlAgilityPack;
using System.Linq;

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
    /// Выбирает элементы из html по ключу(ключам)
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
    /// Ищет дочерние узлы по ключу(ключам)
    /// </summary>
    /// <param name="key">Вид ключа: "//element //element2[@class="class1"] ..."</param>
    /// <returns>Список  объектов класса ResponceHandler для найденных узлов</returns>
    public List<ResponceHandler> SelectChildren(string key)
    {
        Assert.IsTrue(key != null && key.Length > 0);

        return this.doc.DocumentNode.HasChildNodes ? this.doc.DocumentNode.ChildNodes.Select(node => new ResponceHandler(node.OuterHtml)).ToList() : null;
    }

    /// <summary>
    /// Ищет дочерний узлел по ключу(ключам)
    /// </summary>
    /// <param name="key">Вид ключа: "//element //element2[@class="class1"] ..."</param>
    /// <returns>объект класса ResponceHandler для найденного узла</returns>
    public ResponceHandler SelectChild(string key)
    {
        Assert.IsTrue(key != null && key.Length > 0);

        var node = this.doc.DocumentNode.SelectSingleNode(key);

        return node is null ? null : new ResponceHandler(node.OuterHtml);
    }

    /// <summary>
    /// Возвращает весь текст, содержащийся внутри элемента и его дочерних
    /// </summary>
    /// <param name="key">Вид ключа: "//element //element2[@class="class1"] ..."</param>
    /// <returns>Возвращает список текста найденных элементов</returns>
    public string GetInnerText() => this.doc.DocumentNode.InnerText;

    /// <summary>
    ///  Возвращает текст в аттрибуте элемента
    /// </summary>
    /// <param name="attr">Имя аттрибута</param>
    /// <returns></returns>
    public string GetAttributeValue(string attr)
    {
        Assert.IsTrue(attr != null && attr.Length > 0);

        var node = this.doc.DocumentNode;

        if (node.Name == "#document")
            node = node.FirstChild;

        return node.GetAttributeValue(attr, null);
    }

    /// <returns>Возвращает текст внутри текущего элемента</returns>
    public string GetText() => doc.DocumentNode.InnerText;

    /// <returns>Возвращает html текущего элемента</returns>
    public string GetHtml() => doc.DocumentNode.OuterHtml;
}