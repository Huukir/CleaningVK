using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace DeleteFriendsVK
{

    internal class Program
    {

        static VkApi api = new VkApi();

        static int counter = 1;
        static string token = "vk1.a.tM6wcK9FVEO-QPgXXDSms4LxhkWLTbjSZLGFxy6a7jA9V7vVcPaDV7i3WhgjyZogG0NeWujwSDuK3yKBe9EslWemWOauVYXSFOUbDLbBZn56etnCffQJh7KcZGGAhkGxfYz5eTebyLOu-nAYsxVYoRXenpYOmlCSOHTdxOByxkcLVRLrizxo5IZodrvKP9vW";

        public static void GetAllFiendsLists()  // метод создания файла м с ид друзей
        {
            api.Authorize(new ApiAuthParams { AccessToken = token });

            VkCollection<User> ListFriend = api.Friends.Get(new FriendsGetParams { Count = 4300 });
            using (FileStream file = new FileStream("FriendsId.txt", FileMode.Create, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(file);
                Console.WriteLine(ListFriend.Count);
                file.Seek(0, 0);
                foreach (var friend in ListFriend)
                    writer.WriteLine(friend.Id);

                writer.Flush();
                writer.Close();
            }
        }

        public static void ClearAllFriends()   // метод очистки всех друзей
        {
            api.Authorize(new ApiAuthParams { AccessToken = token });
            List<long> list = new List<long>();
            using (FileStream file = new FileStream("FriendsId.txt", FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(file);
                while (!reader.EndOfStream)
                {
                    string id = reader.ReadLine();
                    try
                    {
                        api.Friends.Delete(Convert.ToInt32(id));
                        Console.WriteLine(counter + ")\t" + Convert.ToInt32(id) + "\tуспешно удалён!");
                        counter++;
                        //Thread.Sleep(50);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.TargetSite.ToString() + "\n" + ex.Message);
                        Console.ReadLine();
                    }
                }
                

            }
            }
            static void ClearBlackList()   // метод очистки блеклиста
            {
                api.Authorize(new ApiAuthParams { AccessToken = token });
                foreach (var BannedUsers in api.Account.GetBanned().Profiles)
                {
                    try
                    {
                        api.Account.Unban(BannedUsers.Id);
                        Console.WriteLine(BannedUsers.FirstName + " " + BannedUsers.LastName + " успешно удалён из черного списка!");
                        Console.WriteLine($"{counter} удалён осталось: {BannedUsers.Count}{BannedUsers.Count - counter}");
                        counter++;
                        Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.ReadLine();

                    }
                }
            }
            static void ClearAllLikesPhoto()   // метод отчиски лайков со всех фото
            {
                api.Authorize(new ApiAuthParams { AccessToken = token });

                Console.WriteLine("Всего лайков на фото: " + api.Fave.GetPhotos().Count);
                foreach (var photo in api.Fave.GetPhotos())
                {
                    try
                    {
                        api.Likes.Delete(LikeObjectType.Photo, Convert.ToInt64(photo.Id), photo.OwnerId);
                        Console.WriteLine($"лайк {counter}) успешно удалён!");
                        Thread.Sleep(8000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.TargetSite.ToString() + "\n" + ex.Message);
                        Console.ReadLine();
                    }
                }
            }
            static void Main(string[] args)
            {
                Console.WriteLine("Выберите действие: ");
                Console.WriteLine("1 - Очистить всех друзей");
                Console.WriteLine("2 - Убрать всех из черного списка");
                Console.WriteLine("3 - Убрать лайки со всех фото");
                Console.WriteLine("4 - Создать файл с айдишниками друзей\n");
                Console.Write("-> ");
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        ClearAllFriends();
                        break;
                    case 2:
                        ClearBlackList();
                        break;
                    case 3:
                        ClearAllLikesPhoto();
                        break;
                    case 4:
                        GetAllFiendsLists();
                        break;
                    default:
                        break;
                }
            }

        }
    }
