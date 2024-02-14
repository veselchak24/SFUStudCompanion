//#define DEBUG

/* MMCSFEED
 �������� �����, ��� ��������� ������ � ����� https://mmcs.sfedu.ru/

��� ������������(������ � TemplateFeed):
    1) ������� ��������� MMCSFeed � ������� yield return StartCoroutine(mmcsObj.LoadNewsNodes(startIndexParsing, countNews));
    2) ������� List<MMCSFeed.NewsNode> � ��������� ��� mmcsObj.GetNewsNodes()
    
    �� �������� ������ ������ � ����� �������
    
    !���������!
    ������ ����� ������ ����� �������� ��������� �������:
    
    IEnumirator corutine = StartCorutine(newsNodeObj.GetFullNews());
    yield return corutine;
    NewsNode fullNews = (NewsNode)corutine.Current;
     
    !��������� 1.2!
    � ������ ������� ����� ���������� ���� �������� � ���� ����� = "���������: XX.XX.XX XX:XX"
 */

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �����, ��� ��������� ������ � ����� �������
/// </summary>
partial class MMCSFeed
{
    /// <summary>
    /// ������ ���� ���������� ������(���������: �� ����� � ������)
    /// </summary>
    private List<NewsNode> newsNodes = new List<NewsNode>();

    /// <summary>
    /// ���������� ������ ���� ���������� ������, ����. �� �������
    /// </summary>
    public partial List<NewsNode> GetNewsNodes();

    /// <summary>
    /// �����, ���������� ������ ����� � ����� MMCS
    /// </summary>
    public partial class NewsNode
    {
        public readonly string header, createDate, author, content;

        /// <summary>
        /// ������ �� ����������� �����
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
        /// ���������� ������ �������, ���� ������� ����� �����������(������ "���������")
        /// � ��������� ������ - null
        /// </summary>
        /// <returns>������ new NewsNode(this.header, modifydateNews != null ? modifydateNews : this.createDate, this.author, contentNews)</returns>
        public partial IEnumerator GetFullNews();

        /// <summary>
        /// ���������� html �������� ������ �������
        /// </summary>
        /// <exception cref="ArgumentException">������ ������� �� ����������(��� � ��� ������)</exception>
        /// <exception cref="System.Net.WebException">������ ��������� ������ �� �������(��������)</exception>
        private partial IEnumerator GetHrefHTML();
    }

    /// <summary>
    /// �������� HTML �� �������� ������-��������
    /// </summary>
    /// <param name="startNewsIndex">� ����� ������� ��������� ��������</param>
    /// <returns></returns>
    private partial IEnumerator GetHTMLFromMMCS(int startNewsIndex);

    /// <summary>
    /// �������������.�������� � ������������ ����� � ����� MMCS
    /// </summary>
    /// <param name="startIndex">����� ������� ����� � �����</param>
    /// <param name="count">���������� ������</param>
    public partial IEnumerator LoadNewsNodes(int startIndex = 1, int count = 9);
}