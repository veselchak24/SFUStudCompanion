using System.Collections.Generic;
using UnityEngine.Assertions;
using HtmlAgilityPack;

/// <summary>
/// �����, ��� ������ � html ����������
/// (������ bs4)
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
    /// �������� �������� �� html �� �����(������) <br></br>
    /// </summary>
    /// <param name="key">��� �����: "//element //element2[@class="class1"] ..."</param>
    /// <returns>������ html ��������� ���������</returns>
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
    /// <param name="key">��� �����: "//element //element2[@class="class1"] ..."</param>
    /// <returns>���������� ������ ������ ��������� ���������</returns>
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

    /// <returns>���������� ����� ������ �������� ��������</returns>
    public string GetText() => doc.DocumentNode.InnerText;

    /// <returns>���������� html �������� ��������</returns>
    public string GetHtml() => doc.DocumentNode.OuterHtml;
}