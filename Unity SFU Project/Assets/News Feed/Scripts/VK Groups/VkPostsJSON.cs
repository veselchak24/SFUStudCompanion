/*
В этом классе содержаться классы для развёртывания JSON документа от vk.api и метода wall.get
 
В классах содержаться неполные ответы от api
По надобности добавляйте/удаляйте поля, в соответствии со структурой json-ответа VK

Тип прикреплений к посту из найденных:
video link photo poll album audio doc
*/

using System;

/// <summary>
/// Создаваемый класс для распаковки JSON методом JsonUtility.FromJson<VkPostsJSON>(json);
/// </summary>
[Serializable]
class VkPostsJSON
{
    /// <summary>
    /// Обёртка ответа от vk-api
    /// </summary>
    public Post.Response response;
}

///Пространство имён для инкапсуляции содержимого VK wall.get
///Каждый класс - слой в JSON от VK
namespace Post
{
    /// <summary>
    /// Класс, содержащий ответ от ВК
    /// </summary>
    [Serializable]
    public class Response
    {
        /// <summary>
        /// Количество вложений в ответе
        /// </summary>
        public int count;

        /// <summary>
        /// Массив постов
        /// </summary>
        public PostItem[] items;
    }

    /// <summary>
    /// Класс, содержащий данные о посте
    /// </summary>
    [Serializable]
    public class PostItem
    {
        /// <summary>
        /// Текст поста
        /// </summary> 
        public string text;

        /// <summary>
        /// Дата публикации поста в формате Unix-time
        /// </summary>
        public int date;

        /// <summary>
        /// Дата изменения поста в формате Unix-time(если она есть)
        /// </summary>
        public int edited;

        /// <summary>
        /// Массив прикриплений к посту
        /// </summary>
        public Attachment[] attachments;

        /// <summary>
        /// Массив прикреплённых к посту постов(может не существовать)
        /// </summary>
        public PostItem[] copy_history;
    }

    /// <summary>
    /// Класс, содержащий данные о прикриплении к посту доп. контента
    /// </summary>
    [Serializable]
    public class Attachment
    {
        /// <summary>
        /// Тип прикрепления(photo, video, link - аудиотрек/альбом, poll - опрос)
        /// </summary>
        public string type;

        /// <summary>
        /// Содержит данные о прикреплённом фото
        /// null, если тип прикрепления не photo
        /// </summary>
        public AttachmentTypes.Photo photo;

        /// <summary>
        /// Содержит данные о прикреплённом видео
        /// null, если тип прикрепления не video
        /// </summary>
        public AttachmentTypes.Video video;

        /// <summary>
        /// Содержит данные о прикреплённом аудиофайле
        /// null, если тип прикрепления не audio
        /// </summary>
        public AttachmentTypes.Audio audio;

        /// <summary>
        /// Содержит данные о прикреплённом аудио-альбоме
        /// null, если тип прикрепления не link
        /// </summary>
        public AttachmentTypes.Link link;

        /// <summary>
        /// Содержит данные о прикреплённом опросе
        /// null, если тип прикрепления не poll
        /// </summary>
        public AttachmentTypes.Poll poll;

        /// <summary>
        /// Содержит данные о прикреплённом фото-альбоме
        /// null, если тип прикрепления не album
        /// </summary>
        public AttachmentTypes.Album album;

        /// <summary>
        /// Содержит данные о прикреплённом документе
        /// null, если тип прикрепления не doc
        /// </summary>
        public AttachmentTypes.Document doc;

        public static class AttachmentTypes
        {
            /// <summary>
            /// Класс, содержащий данные о прикреплённом фото к посту
            /// </summary>
            [Serializable]
            public class Photo
            {
                /// <summary>
                /// Массив изображений в разном разрешении
                /// От наименьшего к наибольшему
                /// </summary>
                public Image[] sizes;

                /// <summary>
                /// Класс, содержащий данные о изображений
                /// </summary>
                [Serializable]
                public class Image
                {
                    /// <summary>
                    /// Ссылка на изображение
                    /// </summary>
                    public string url;

                    /// <summary>
                    /// Размеры изображения
                    /// </summary>
                    public int width, height;
                }
            }

            /// <summary>
            /// Класс, содержащий данные о прикреплённом видео к посту
            /// </summary>
            [Serializable]
            public class Video
            {
                /// <summary>
                /// Превью фото видеоролика
                /// !Чем больше индекс - тем больше изображение!
                /// </summary>
                public Photo.Image[] image;

                /// <summary>
                /// Название видео
                /// Можно использовать, если не грузится превью видео
                /// </summary>
                public string title;

                /// <summary>
                /// Тип видео (video, short_video)
                /// </summary>
                public string type;
            }

            /// <summary>
            /// Класс, содержащий данные о прикреплённом аудио к посту
            /// </summary>
            public class Audio
            {
                public string artist;
                public string title;
                public string url;
            }

            /// <summary>
            /// Класс, содержащий данные о прикреплённом аудио-альбоме к посту
            /// </summary> 
            [Serializable]
            public class Link
            {
                /// <summary>
                /// Тип прикрипления
                /// Playlist,
                /// </summary>
                public string description;

                /// <summary>
                /// Описание/название аудиотрека/альбома
                /// </summary>
                public string title;
            }

            /// <summary>
            /// Класс, содержащий данные о прикреплённом опросе к посту
            /// </summary>
            [Serializable]
            public class Poll
            {
                /// <summary>
                /// Вопрос опроса
                /// </summary>
                public string question;

                /// <summary>
                /// Массив вариантов ответа
                /// </summary>
                public Answer[] answers;

                /// <summary>
                /// Класс, содержащий данные об имеющихся вариантах ответа
                /// </summary>
                [Serializable]
                public class Answer
                {
                    /// <summary>
                    /// Процентное соотношение проголосовавших
                    /// </summary>
                    public int rate;

                    /// <summary>
                    /// Текст варианта ответа
                    /// </summary>
                    public string text;

                    /// <summary>
                    /// Количество голосов
                    /// </summary>
                    public int votes;
                }
            }

            /// <summary>
            /// Класс, содержащий данные о прикреплённом фото-альбоме к посту
            /// </summary>
            [Serializable]
            public class Album
            {
                public string title;
                public Photo.Image[] sizes;
            }

            /// <summary>
            /// Класс, содержащий данные о прикреплённом документу
            /// </summary>
            [Serializable]
            public class Document
            {
                /// <summary>
                /// Название документа
                /// </summary>
                public string title;

                /// <summary>
                /// Тип документа
                /// </summary>
                public string ext;

                /// <summary>
                /// Ссылка на документ от вк
                /// </summary>
                public string url;

                //Можно добавить фото-превью
                /*
                  "preview": {
                    "photo": {
                      "sizes": [] - Реализован как Photo.Image[]
                 */
            }
        };
    }
}