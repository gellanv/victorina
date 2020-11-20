using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;

namespace Victorina
{
    public class BD
    {
        public void ReadAllInfo(VictorinaProgram Vic)
        {
            //Считываем все данные с файлов           
            List<string> mas = new List<string>();

            //Считываем с файла перечень тем
            ReadTopic(Vic, "topic.txt");

            //Считываем с файла пользователей и информацию по кажому из них
            mas = ReadDate("users.txt");
            foreach (string item in mas)
            {
                string[] word = item.Split(' ');
                Vic._user.Add(new User(word[0], word[1], Convert.ToInt32(word[2]), Convert.ToInt32(word[3]), Convert.ToInt32(word[4])));
            }
            //Считываем с файла достижения по каждому пользователю
            foreach (User use in Vic._user)
            {
                mas = ReadDate(use.Login + ".txt");
                foreach (string item in mas)
                {
                    string[] word = item.Split(' ');
                    use.rez.Add(new Rezult(word[0], Convert.ToInt32(word[1]), Convert.ToInt32(word[2]), Convert.ToInt32(word[3]), Convert.ToInt32(word[4])));                        
                 } 
            }

            //Считываем с файла вопросы ответы по каждой викторине
            ReadQuestionAnsver(Vic, "");
        }

        //Считываем с файла список тем
        public void ReadTopic(VictorinaProgram Vic, string arg)
        {
            List<string> mas = new List<string>();          
            mas = ReadDate(arg);
            foreach (string item in mas)
            {
                Vic._topic.Add(new Topic(item));
            }
        }

        //Считываем с файла вопросы-ответы по каждой викторине
        public void ReadQuestionAnsver (VictorinaProgram Vic, string arg)
        {
            List<string> mas = new List<string>();
            foreach (Topic top in Vic._topic)
            {
                string temp = arg + top.name + ".txt";
                mas = ReadDate(temp);
                foreach (string item in mas)
                {
                    string[] word = item.Split('?');
                    string[] word2 = word[1].Trim().Split('|');
                    Dictionary<string, int> Answer = new Dictionary<string, int>();
                    for (int i = 1; i < 4; i++)
                    {
                        string[] word3 = word2[i].Split('-');
                        Answer.Add(word3[0], Convert.ToInt32(word3[1]));
                    }
                    top._questionAnswers.Add(new QuestionAnswer(word[0], Convert.ToInt32(word2[0]), Answer));
                }
            }
        }

        public List<string> ReadDate(string args)
        {
            string filePath = args;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Unicode);
                List<string> _list = new List<string>();
                while (!sr.EndOfStream)
                {
                    _list.Add(sr.ReadLine());
                }
                fs.Close();
                return _list;
            }
        }
        public static void CreatDoc(User NewUser)
        {
            string filePath1 = NewUser.Login + ".txt";
            using (FileStream fs = new FileStream(filePath1, FileMode.Create, FileAccess.Write)) { }
        }
        public static void WriteChangeUser(VictorinaProgram Victorina)//Перезаписываем изменения в файле пользователей 
        {
            string filePath = "users.txt";
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    foreach (User us in Victorina._user)
                    {
                        sw.WriteLine(us);
                    }
                }
            }
        }
        public static void WriteChangeUserRezult(User _Use)//Перезаписываем изменения в файле пользователя
        {
            string filePath = _Use.Login+".txt";
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    foreach (Rezult r in _Use.rez)
                    {
                        sw.WriteLine(r);
                    }
                }
            }
        }
    }
}
