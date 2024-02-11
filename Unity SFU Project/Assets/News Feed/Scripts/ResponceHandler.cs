using System.Collections.Generic;
using UnityEngine.Assertions;
using HtmlAgilityPack;
using System.Linq;

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
    /// �������� �������� �� html �� �����(������)
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
    /// ���� �������� ���� �� �����(������)
    /// </summary>
    /// <param name="key">��� �����: "//element //element2[@class="class1"] ..."</param>
    /// <returns>������  �������� ������ ResponceHandler ��� ��������� �����</returns>
    public List<ResponceHandler> SelectChildren(string key)
    {
        Assert.IsTrue(key != null && key.Length > 0);

        return this.doc.DocumentNode.HasChildNodes ? this.doc.DocumentNode.ChildNodes.Select(node => new ResponceHandler(node.OuterHtml)).ToList() : null;
    }

    /// <summary>
    /// ���� �������� ����� �� �����(������)
    /// </summary>
    /// <param name="key">��� �����: "//element //element2[@class="class1"] ..."</param>
    /// <returns>������ ������ ResponceHandler ��� ���������� ����</returns>
    public ResponceHandler SelectChild(string key)
    {
        Assert.IsTrue(key != null && key.Length > 0);

        var node = this.doc.DocumentNode.SelectSingleNode(key);

        return node is null ? null : new ResponceHandler(node.OuterHtml);
    }

    /// <summary>
    /// ���������� ���� �����, ������������ ������ �������� � ��� ��������
    /// </summary>
    /// <param name="key">��� �����: "//element //element2[@class="class1"] ..."</param>
    /// <returns>���������� ������ ������ ��������� ���������</returns>
    public string GetInnerText() => this.doc.DocumentNode.InnerText;

    /// <summary>
    ///  ���������� ����� � ��������� ��������
    /// </summary>
    /// <param name="attr">��� ���������</param>
    /// <returns></returns>
    public string GetAttributeValue(string attr)
    {
        Assert.IsTrue(attr != null && attr.Length > 0);

        var node = this.doc.DocumentNode;

        if (node.Name == "#document")
            node = node.FirstChild;

        return node.GetAttributeValue(attr, null);
    }

    /// <returns>���������� ����� ������ �������� ��������</returns>
    public string GetText() => doc.DocumentNode.InnerText;

    /// <returns>���������� html �������� ��������</returns>
    public string GetHtml() => doc.DocumentNode.OuterHtml;
}