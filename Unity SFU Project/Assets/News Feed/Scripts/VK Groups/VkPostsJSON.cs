/*
� ���� ������ ����������� ������ ��� ������������ JSON ��������� �� vk.api � ������ wall.get
 
� ������� ����������� �������� ������ �� api
�� ���������� ����������/�������� ����, � ������������ �� ���������� json-������ VK

��� ������������ � ����� �� ���������:
video link photo poll album audio doc
*/

using System;

/// <summary>
/// ����������� ����� ��� ���������� JSON ������� JsonUtility.FromJson<VkPostsJSON>(json);
/// </summary>
[Serializable]
class VkPostsJSON
{
    /// <summary>
    /// ������ ������ �� vk-api
    /// </summary>
    public Post.Response response;
}

///������������ ��� ��� ������������ ����������� VK wall.get
///������ ����� - ���� � JSON �� VK
namespace Post
{
    /// <summary>
    /// �����, ���������� ����� �� ��
    /// </summary>
    [Serializable]
    public class Response
    {
        /// <summary>
        /// ���������� �������� � ������
        /// </summary>
        public int count;

        /// <summary>
        /// ������ ������
        /// </summary>
        public PostItem[] items;
    }

    /// <summary>
    /// �����, ���������� ������ � �����
    /// </summary>
    [Serializable]
    public class PostItem
    {
        /// <summary>
        /// ����� �����
        /// </summary> 
        public string text;

        /// <summary>
        /// ���� ���������� ����� � ������� Unix-time
        /// </summary>
        public int date;

        /// <summary>
        /// ���� ��������� ����� � ������� Unix-time(���� ��� ����)
        /// </summary>
        public int edited;

        /// <summary>
        /// ������ ������������ � �����
        /// </summary>
        public Attachment[] attachments;

        /// <summary>
        /// ������ ������������ � ����� ������(����� �� ������������)
        /// </summary>
        public PostItem[] copy_history;
    }

    /// <summary>
    /// �����, ���������� ������ � ������������ � ����� ���. ��������
    /// </summary>
    [Serializable]
    public class Attachment
    {
        /// <summary>
        /// ��� ������������(photo, video, link - ���������/������, poll - �����)
        /// </summary>
        public string type;

        /// <summary>
        /// �������� ������ � ������������ ����
        /// null, ���� ��� ������������ �� photo
        /// </summary>
        public AttachmentTypes.Photo photo;

        /// <summary>
        /// �������� ������ � ������������ �����
        /// null, ���� ��� ������������ �� video
        /// </summary>
        public AttachmentTypes.Video video;

        /// <summary>
        /// �������� ������ � ������������ ����������
        /// null, ���� ��� ������������ �� audio
        /// </summary>
        public AttachmentTypes.Audio audio;

        /// <summary>
        /// �������� ������ � ������������ �����-�������
        /// null, ���� ��� ������������ �� link
        /// </summary>
        public AttachmentTypes.Link link;

        /// <summary>
        /// �������� ������ � ������������ ������
        /// null, ���� ��� ������������ �� poll
        /// </summary>
        public AttachmentTypes.Poll poll;

        /// <summary>
        /// �������� ������ � ������������ ����-�������
        /// null, ���� ��� ������������ �� album
        /// </summary>
        public AttachmentTypes.Album album;

        /// <summary>
        /// �������� ������ � ������������ ���������
        /// null, ���� ��� ������������ �� doc
        /// </summary>
        public AttachmentTypes.Document doc;

        public static class AttachmentTypes
        {
            /// <summary>
            /// �����, ���������� ������ � ������������ ���� � �����
            /// </summary>
            [Serializable]
            public class Photo
            {
                /// <summary>
                /// ������ ����������� � ������ ����������
                /// �� ����������� � �����������
                /// </summary>
                public Image[] sizes;

                /// <summary>
                /// �����, ���������� ������ � �����������
                /// </summary>
                [Serializable]
                public class Image
                {
                    /// <summary>
                    /// ������ �� �����������
                    /// </summary>
                    public string url;

                    /// <summary>
                    /// ������� �����������
                    /// </summary>
                    public int width, height;
                }
            }

            /// <summary>
            /// �����, ���������� ������ � ������������ ����� � �����
            /// </summary>
            [Serializable]
            public class Video
            {
                /// <summary>
                /// ������ ���� �����������
                /// !��� ������ ������ - ��� ������ �����������!
                /// </summary>
                public Photo.Image[] image;

                /// <summary>
                /// �������� �����
                /// ����� ������������, ���� �� �������� ������ �����
                /// </summary>
                public string title;

                /// <summary>
                /// ��� ����� (video, short_video)
                /// </summary>
                public string type;
            }

            /// <summary>
            /// �����, ���������� ������ � ������������ ����� � �����
            /// </summary>
            public class Audio
            {
                public string artist;
                public string title;
                public string url;
            }

            /// <summary>
            /// �����, ���������� ������ � ������������ �����-������� � �����
            /// </summary> 
            [Serializable]
            public class Link
            {
                /// <summary>
                /// ��� ������������
                /// Playlist,
                /// </summary>
                public string description;

                /// <summary>
                /// ��������/�������� ����������/�������
                /// </summary>
                public string title;
            }

            /// <summary>
            /// �����, ���������� ������ � ������������ ������ � �����
            /// </summary>
            [Serializable]
            public class Poll
            {
                /// <summary>
                /// ������ ������
                /// </summary>
                public string question;

                /// <summary>
                /// ������ ��������� ������
                /// </summary>
                public Answer[] answers;

                /// <summary>
                /// �����, ���������� ������ �� ��������� ��������� ������
                /// </summary>
                [Serializable]
                public class Answer
                {
                    /// <summary>
                    /// ���������� ����������� ���������������
                    /// </summary>
                    public int rate;

                    /// <summary>
                    /// ����� �������� ������
                    /// </summary>
                    public string text;

                    /// <summary>
                    /// ���������� �������
                    /// </summary>
                    public int votes;
                }
            }

            /// <summary>
            /// �����, ���������� ������ � ������������ ����-������� � �����
            /// </summary>
            [Serializable]
            public class Album
            {
                public string title;
                public Photo.Image[] sizes;
            }

            /// <summary>
            /// �����, ���������� ������ � ������������ ���������
            /// </summary>
            [Serializable]
            public class Document
            {
                /// <summary>
                /// �������� ���������
                /// </summary>
                public string title;

                /// <summary>
                /// ��� ���������
                /// </summary>
                public string ext;

                /// <summary>
                /// ������ �� �������� �� ��
                /// </summary>
                public string url;

                //����� �������� ����-������
                /*
                  "preview": {
                    "photo": {
                      "sizes": [] - ���������� ��� Photo.Image[]
                 */
            }
        };
    }
}