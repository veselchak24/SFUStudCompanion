/* VkFeeds
 * В этом классе содержаться методы получения постов от новостных групп ЮФУ
 
!ВНИМАНИЕ!
    В коде отсутствует переменная string service_access_key
    Вам необходимо самостоятельно завести ее в классе VKFeeds, присвоив ей сервисный ключ вашего приложения vk 
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
    /// Перечисление групп vk для ленты новостей
    /// </summary>
    public enum VkGroups
    {
        Мехмат,
        Самоделка_Мехмат,
        Цитатник_Мехмата,
        ОСО_ЮФУ,
        СИЦ_Мехмат
    }

    /// <summary>
    /// Отправляет запрос к vkApi и получает json список постов со стены группы ВК
    /// </summary>
    /// <param name="group">Группа, посты которой нужно запросить</param>
    /// <param name="offset">Сдвиг постов по новизне</param>
    /// <param name="count">Количество требуемых постов</param>
    /// <returns>Сначало - инструкция для ожидания ответа, затем - json документ</returns>
    public IEnumerator LoadJsonPosts(VkGroups group, int offset, int count)
    {
        string url = GenerateVkApiRequest(group, offset, count);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Отправляем запрос
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.InProgress)
                throw new WebException("webRequest in progress. Так быть не должно");

            if (webRequest.result != UnityWebRequest.Result.Success)
                throw new WebException("Network error from conection to vk.api");

            if (webRequest.downloadHandler.text.Length > 0)
                this.responseData = JsonUtility.FromJson<VkPostsJSON>(webRequest.downloadHandler.text).response;
            else
                throw new ArgumentNullException("Responce от vk.api пуст");
        }
    }

    /// <summary>
    /// Конвертирует выбранную группу ВК в её доменное имя
    /// </summary>
    /// <exception cref="ArgumentException">Попытка получить несуществующий домен</exception>
    private static string ConvertEnumToVkDomain(VkGroups group)
    {
        switch (group)
        {
            case VkGroups.Мехмат:
                return "mmcs.official";
            case VkGroups.Самоделка_Мехмат:
                return "mm_samodelka";
            case VkGroups.Цитатник_Мехмата:
                return "mmcs_quotes";
            case VkGroups.ОСО_ЮФУ:
                return "oso.sfedu";
            case VkGroups.СИЦ_Мехмат:
                return "sic_mmcs";

            default:
                throw new ArgumentException("Undefined vk group");
        }
    }

    /// <summary>
    /// Составляет запрос к vkApi на получение ленты новостей в виде json
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
 * Тест-запрос
 * Впишите свой токен в конец, вместо your_token
 * https://api.vk.com/method/wall.get?domain=mm_samodelka&offset=0&count=2&v=5.199&access_token=your_token
 * */