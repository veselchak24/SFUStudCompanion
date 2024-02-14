/* VkFeeds
 * � ���� ������ ����������� ������ ��������� ������ �� ��������� ����� ���
 
!��������!
    � ���� ����������� ���������� string service_access_key
    ��� ���������� �������������� ������� �� � ������ VKFeeds, �������� �� ��������� ���� ������ ���������� vk 
 */

using System.Collections;

using UnityWebRequest = UnityEngine.Networking.UnityWebRequest;
using JsonUtility = UnityEngine.JsonUtility;

using ArgumentException = System.ArgumentException;
using ArgumentNullException = System.ArgumentNullException;
using WebException = System.Net.WebException;

public partial class VKFeeds
{
    public Post.Response responseData = null;

    /// <summary>
    /// ������������ ����� vk ��� ����� ��������
    /// </summary>
    public enum VkGroups
    {
        ������,
        ���������_������,
        ��������_�������,
        ���_���,
        ���_������
    }

    /// <summary>
    /// ���������� ������ � vkApi � �������� json ������ ������ �� ����� ������ ��
    /// </summary>
    /// <param name="group">������, ����� ������� ����� ���������</param>
    /// <param name="offset">����� ������ �� �������</param>
    /// <param name="count">���������� ��������� ������</param>
    /// <returns>������� - ���������� ��� �������� ������, ����� - json ��������</returns>
    public IEnumerator LoadJsonPosts(VkGroups group, int offset, int count)
    {
        string url = GenerateVkApiRequest(group, offset, count);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // ���������� ������
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.InProgress)
                throw new WebException("webRequest in progress. ��� ���� �� ������");

            if (webRequest.result != UnityWebRequest.Result.Success)
                throw new WebException("Network error from conection to vk.api");

            if (webRequest.downloadHandler.text.Length > 0)
                this.responseData = JsonUtility.FromJson<VkPostsJSON>(webRequest.downloadHandler.text).response;
            else
                throw new ArgumentNullException("Responce �� vk.api ����");
        }
    }

    /// <summary>
    /// ������������ ��������� ������ �� � � �������� ���
    /// </summary>
    /// <exception cref="ArgumentException">������� �������� �������������� �����</exception>
    private static string ConvertEnumToVkDomain(VkGroups group)
    {
        switch (group)
        {
            case VkGroups.������:
                return "mmcs.official";
            case VkGroups.���������_������:
                return "mm_samodelka";
            case VkGroups.��������_�������:
                return "mmcs_quotes";
            case VkGroups.���_���:
                return "oso.sfedu";
            case VkGroups.���_������:
                return "sic_mmcs";

            default:
                throw new ArgumentException("Undefined vk group");
        }
    }

    /// <summary>
    /// ���������� ������ � vkApi �� ��������� ����� �������� � ���� json
    /// </summary>
    /// <param name="group"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private string GenerateVkApiRequest(VkGroups group, int offset, int count)
    {
        string st = "https://api.vk.com/method/wall.get?";
        string domain = ConvertEnumToVkDomain(group);
        string version_api = "5.199";

        return st +
            $"domain={domain}" +
            $"&offset={offset}" +
            $"&count={count}" +
            $"&v={version_api}" +
            $"&access_token={service_access_key}";
    }
}
/*
 * ����-������
 * ������� ���� ����� � �����, ������ your_token
 * https://api.vk.com/method/wall.get?domain=mm_samodelka&offset=0&count=2&v=5.199&access_token=your_token
 * */