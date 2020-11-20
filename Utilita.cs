using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;

namespace Victorina
{
    class Utilita
    {
        public void AuthorizationAdmin(BD dateBase, VictorinaProgram Victorina)
        {
            try
            {
                string _login, _password;
                Write("\tВведите логин: ");
                _login = ReadLine();
                Write("\tВведите пароль: ");
                _password = ReadLine();

                List<string> mas = new List<string>();
                mas = dateBase.ReadDate("admin.txt");
                string[] word = mas[0].Split(' ');
                if (_login == word[0] && _password == word[1])
                {
                    Clear();                    
                    MenuAdmin(Victorina);
                }
                else
                {
                    throw new Exception("Вы ввели неверный пароль!");
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                Write("\tПовторить попытку, нажмите любую клавишу");
                ReadKey();
                Clear();
                AuthorizationAdmin(dateBase, Victorina);
            }
        }
        public void MenuAdmin(VictorinaProgram Victorina)
        {
            Clear();
            Write("\tВы авторизовались как админ: ");
            WriteLine("\n\n1. Добавить новую викторину\n" +
              "2. Редактировать существующую викторину\n");
            int chose2 = Convert.ToInt32(ReadLine());
            switch (chose2)
            {
                case 1:
                    AddNewVictorin(Victorina);
                    WriteLine("Нажмите любую клавишу, чтобы вернуться к МЕНЮ:");
                    ReadKey();                    
                    MenuAdmin(Victorina);
                    break;
                case 2:
                    EditVictorin(Victorina);
                    WriteLine("Нажмите любую клавишу, чтобы вернуться к МЕНЮ:");
                    ReadKey();
                    MenuAdmin(Victorina);
                    break;
                default:
                    WriteLine("Вы ввели неверный пункт меню!");
                    WriteLine("Нажмите любую клавишу, чтобы повторить попытку");
                    ReadKey();
                    MenuAdmin(Victorina);
                    break;
            }
        }
        public void AddNewVictorin(VictorinaProgram Victorina)
        {
            WriteLine("Введите название новой викторины: ");
            string name = ReadLine();
            WriteLine("Количество вопросов: ");
            int CountQ = Convert.ToInt32(ReadLine());
            Topic temp = new Topic(name);           
            for (int i=0;i< CountQ; i++)
            {
                WriteLine("Введите вопрос:");
                string Question = ReadLine();
                WriteLine("Количество верных ответов:");
                int CountAns = Convert.ToInt32(ReadLine());
                Dictionary<string, int> tempDic = new Dictionary<string, int>();
                WriteLine("Варианты ответов (Вариант-0/1) О-лож, 1-истина");
                string Ansver1 = ReadLine();
                int _Ansver1 = Convert.ToInt32(ReadLine());
                tempDic.Add(Ansver1, _Ansver1);

                string Ansver2 = ReadLine();
                int _Ansver2 = Convert.ToInt32(ReadLine());
                tempDic.Add(Ansver2, _Ansver2);

                string Ansver3= ReadLine();
                int _Ansver3 = Convert.ToInt32(ReadLine());
                tempDic.Add(Ansver3, _Ansver3);
                QuestionAnswer tempQA = new QuestionAnswer(Question, CountAns, tempDic);
                temp._questionAnswers.Add(tempQA);
            }
            Victorina._topic.Add(temp);
            WriteChangeTopic(Victorina);
            WriteChangeTopicView(temp, name);
            temp.Show1();

        }
        public void EditVictorin(VictorinaProgram Victorina)
        {
            foreach (Topic t in Victorina._topic)
            {
                WriteLine(t.name);
            }
            WriteLine("Укажите викторину, которую нужно отредактировать:\n");
            string tempNameTopic = ReadLine();
            Topic tempTopic=Victorina._topic.Find(x => x.name.Contains(tempNameTopic));
            tempTopic.Show1();
            WriteLine("Укажите номер вопроса, который нужно отредактировать:\n");
            int tempQuestion = Convert.ToInt32(ReadLine());
            tempTopic._questionAnswers[tempQuestion - 1].Show();

            WriteLine("Введите новый вопрос:");
            string QuestionNew = ReadLine();
            WriteLine("Количество верных ответов:");
            int CountAnsNew = Convert.ToInt32(ReadLine());
            Dictionary<string, int> tempDicNew = new Dictionary<string, int>();
            WriteLine("Варианты ответов (Вариант-0/1) О-лож, 1-истина");
            string Ansver1 = ReadLine();
            int _Ansver1 = Convert.ToInt32(ReadLine());
            tempDicNew.Add(Ansver1, _Ansver1);

            string Ansver2 = ReadLine();
            int _Ansver2 = Convert.ToInt32(ReadLine());
            tempDicNew.Add(Ansver2, _Ansver2);

            string Ansver3 = ReadLine();
            int _Ansver3 = Convert.ToInt32(ReadLine());
            tempDicNew.Add(Ansver3, _Ansver3);
            QuestionAnswer tempQA = new QuestionAnswer(QuestionNew, CountAnsNew, tempDicNew);
            tempTopic._questionAnswers[tempQuestion - 1]=tempQA;
            WriteLine("\t\tИзменения внесены:\n");
            tempTopic._questionAnswers[tempQuestion - 1].Show();
            WriteChangeTopicView(tempTopic, tempTopic.name);

        }
        public void WriteChangeTopic(VictorinaProgram Victorina)//Перезаписываем изменения в файле перечня Викторин 
        {
            string filePath = @"..\..\..\Victorina\bin\Debug\topic.txt";
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    foreach (Topic top in Victorina._topic)
                    {
                        sw.WriteLine(top.name);
                    }
                }
            }
        }
        public void WriteChangeTopicView(Topic _Topic, string TopicName)//Перезаписываем изменения в файле Викторин 
        {
            string filePath = @"..\..\..\Victorina\bin\Debug\"+TopicName + ".txt";
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                        foreach (QuestionAnswer qa in _Topic._questionAnswers)
                        {
                            string tempst = qa.Question + "?" + qa.CountAnsw;
                            foreach (KeyValuePair<string, int> d in qa.Answers)
                            {
                                tempst+="|"+d.Key+"-"+d.Value;                                
                            }
                             sw.WriteLine(tempst);
                    }   
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            VictorinaProgram Victorina = new VictorinaProgram();
            BD dateBase = new BD();
            dateBase.ReadTopic(Victorina, @"..\..\..\Victorina\bin\Debug\topic.txt");
            dateBase.ReadQuestionAnsver(Victorina, @"..\..\..\Victorina\bin\Debug\");

            Utilita Utl = new Utilita();
            WriteLine("Чтобы изменить данные в викторине нужно авторизоваться!!!");
            Utl.AuthorizationAdmin(dateBase, Victorina);
        }
    }
}
