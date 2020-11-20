using System;
using System.Collections.Generic;
using static System.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Data;

namespace Victorina
{
     public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }

        public List<Rezult> rez=new List<Rezult>();//Массив результатов
        public User(string login, string password, int year, int month, int day)
        {
            Login = login;
            Password = password;
            BirthDay = new DateTime(year, month, day);
        }

        public void SetBirthDay(int year, int month, int day)
        {
            BirthDay = new DateTime(year, month, day);
        }
        public string GetBirthDay()
        {
            return BirthDay.Year + "." + BirthDay.Month + "." + BirthDay.Day;
        }
        public override string ToString()
        {
            return Login + " " + Password + " " + BirthDay.Year + " " + BirthDay.Month + " " + BirthDay.Day;
        }
        public void MenuUser(VictorinaProgram Victorina)
        {
            Clear();
            WriteLine($"Вы авторизовались как - {Login}\n\tМЕНЮ:");
            WriteLine("1. Начать новую викторину\n" +
                "2. Ваши предыдущие результаты\n" +
                "3. Топ-20\n" +
                "4. Изменить данные учетной записи\n" +
                "5. Выход\n");
            int chose2 = Convert.ToInt32(ReadLine());
            switch (chose2)
            {
                case 1:
                    StartNewVictorin(Victorina);
                    WriteLine("Нажмите любую клавишу, чтобы вернуться к МЕНЮ:");
                    ReadKey();                    
                    MenuUser(Victorina);
                    break;
                case 2:
                    ShowRezalt();
                    WriteLine("Нажмите любую клавишу, чтобы вернуться к МЕНЮ:");
                    ReadKey();
                    MenuUser(Victorina);
                    break;
                case 3:
                    ShowTop(Victorina);
                    WriteLine("Нажмите любую клавишу, чтобы вернуться к МЕНЮ:");
                    ReadKey();
                    MenuUser(Victorina);
                    break;
                case 4:
                    ChangeDate(Victorina);
                    WriteLine("Нажмите любую клавишу, чтобы вернуться к МЕНЮ:");
                    ReadKey();
                    MenuUser(Victorina);
                    break;
                case 5:
                    throw new Exception("Вы вышли из программы!");
                default:
                    WriteLine("Вы ввели неверный пункт меню!");
                    WriteLine("Нажмите любую клавишу, чтобы повторить попытку");
                    ReadKey();
                    MenuUser(Victorina);
                    break;
            }
        }
        void StartNewVictorin(VictorinaProgram Victorina)
        {
            WriteLine("\nДоступные викторины:");
            foreach (Topic t in Victorina._topic)
            {
                WriteLine(t.name);
            }
            WriteLine("\nУкажите Тему викторины или \"1\" если вы хотите играть в смешанную викторину:");
            string tema = ReadLine();
            Topic tempTopic=Victorina._topic.Find(x => x.name.Contains(tema));
            DateTime dt=DateTime.Now;                   
            if (tempTopic != null)
            {
                int r = tempTopic.VictorinaOpros();//записываем кол-во правильных ответов в переменную
                rez.Add(new Rezult(tema,(r*100/20), dt.Year ,dt.Month, dt.Day));
                BD.WriteChangeUserRezult(this);
                Write($"\n\t\tВаш РЕЗУЛЬТАТ  - {r * 100 / 20}%\n");
            }
            else
                if(Convert.ToInt32 (tema)== 1)
                {
                    List<QuestionAnswer> SmeshQA = new List<QuestionAnswer>();                
                    foreach (Topic t in Victorina._topic)
                    {
                        SmeshQA.AddRange(t._questionAnswers);
                    }
                    int len = SmeshQA.Count();                
                    Topic Smeshanuy = new Topic("Смешанный");
                    Random rand = new Random();
                    for (int i=0;i<20;i++)
                    {
                        int k = rand.Next(0, len);
                    if ((Smeshanuy._questionAnswers.Find(x => x.Question.Contains(SmeshQA[k].Question))) != null)//если такой вопрос уже есть в списке то берем другой
                    { i--; continue; }
                    else { Smeshanuy._questionAnswers.Add(SmeshQA[k]); }
                    }
                    int r = Smeshanuy.VictorinaOpros();//записываем кол-во правильных ответов в переменную
                    rez.Add(new Rezult("Смешанный", (r * 100 / 20), dt.Year, dt.Month, dt.Day));
                    BD.WriteChangeUserRezult(this);
                    Write($"\n\t\tВаш РЕЗУЛЬТАТ  - {r * 100 / 20}%\n");
            }
            else
            {
                WriteLine("Тема указана неверно!!!");
            }
        }
        void ShowRezalt()
        {
        foreach(Rezult r in rez)
            {
             WriteLine(r.ShowRez());
            }
        }
        void ChangeDate(VictorinaProgram Victorina)
        {
            WriteLine("Какие данные вы хотите изменить: 1- Пароль; 2 - Дату рождения.");
            int chose3 = Convert.ToInt32(ReadLine());
            switch(chose3)
            {
                case 1:
                    WriteLine("Введите новый пароль:");
                    Password = ReadLine();
                    BD.WriteChangeUser(Victorina);
                    WriteLine("Ваш пароль был изменен!!!");
                    break;
                case 2:
                    WriteLine("Введите дату рождения в формате 1985.12.31");
                    bool fl = false;
                    do
                    {
                        Write($"Текущая дата: {GetBirthDay()}");
                        string DataPattern = @"^[1-2]{1}[0-9]{3}\.[0,1]{1}[0-9]{1}\.[0-9]{2}$";//Шаблон 4 символа+ 1 цифра вконце
                        Write("\tВведите дату: ");
                        string _data = ReadLine();
                        Regex regex = new Regex(DataPattern);
                        if (regex.IsMatch(_data))
                        {
                            string[] d = _data.Split('.');
                            SetBirthDay(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]));
                            BD.WriteChangeUser(Victorina);
                            WriteLine("Изменения внесены!");
                            fl = true;
                        }
                        else WriteLine("Вы ввели неверный формат даты");
                    }
                    while (fl == false);
                    break;
                default:
                    break;
            }
        }
        void ShowTop(VictorinaProgram Victorina)
        {
            WriteLine("Укажите викторину, по которой хотите посмотреть ТОП!");
            foreach (Topic t in Victorina._topic)
            {
                WriteLine(t.name);
            }
            string tema = ReadLine();
           
            List<Lider> _Lid = new List<Lider>();           
            foreach (User u in Victorina._user)
            {
                foreach ( Rezult r in u.rez)
                {
                    if (r.NameTopic == tema)
                        _Lid.Add(new Lider(u.Login, r));
                }                
            }
            _Lid.Sort();
            int k = 1;
            foreach (Lider a in _Lid)
            {
                WriteLine($"{a.top.Mark}% {a.name} {a.top.TimeTest.ToShortDateString()}");
                k++;
                if (k > 20) break;
            }
        }
    }

    public class Lider: IComparable
    {
        public string name;
        public Rezult top;
        public Lider(string n, Rezult r)
        {
            name = n;
            top = r;
        }
        public int CompareTo(object obj)
        {
            if(obj is Lider)
            {
                return (obj as Lider).top.Mark.CompareTo(top.Mark);
            }
            throw new NotImplementedException();
        }
}
    public class Rezult
    {
        public string NameTopic { set; get; }
        public int Mark { set; get; }
        public DateTime TimeTest { set; get; }
        public Rezult (string n, int m, int year, int month, int day)
        {
            NameTopic = n;
            Mark = m;
            TimeTest= new DateTime(year, month, day);
        }
        public Rezult()
        {
            NameTopic = null;
            Mark = 0;
            TimeTest = new DateTime();
        }
        public string ShowRez()
        {
            return NameTopic +" "+ Mark.ToString()+"%\tДата: "+ TimeTest.ToShortDateString();
        }
        public override string ToString()
        {
            return NameTopic + " " + Mark.ToString() + " " + TimeTest.Year + " "+ TimeTest.Month+ " "+ TimeTest.Day;
        }
    }
}
