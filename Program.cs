using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Victorina
{
     public class VictorinaProgram
    {
        public List<User> _user = new List<User>();
        public List<Topic> _topic = new List<Topic>();

        public void Authorization(VictorinaProgram Victorina) 
        {
           string _login, _password;
           Write("\tВведите логин: ");
           _login = ReadLine();
           Write("\tВведите пароль: ");
           _password = ReadLine();
           User temp = Victorina._user.Find(x => x.Login.Equals(_login));
            if (temp == null)
                {
                throw new Exception("Пользователь под таким логином не зарегистрирован!");
                }
                    if (temp.Password != _password)
                 {
                throw new Exception("Вы ввели неверный пароль!");
                  }            
            else
            { Clear();
                temp.MenuUser(Victorina);
            }            
        }
        public static void Registration(VictorinaProgram Victorina)
        {
            string _login, _password;
            bool fl = false;
            do
            {
                string LoginPattern = @"^[a-z0-9_-]{4}[0-9]{1}$";//Шаблон 4 символа+ 1 цифра вконце
                Write("\tВведите логин: ");
                _login = ReadLine();
                Regex regex = new Regex(LoginPattern);               
                if (regex.IsMatch(_login))
                {
                    WriteLine("Логин Ок!");
                    fl = true;                   
                }
                else WriteLine("Вы ввели неверный формат для логина");
            }
            while (fl == false);

            fl = false;
            do
            {
                string PasswodPattern = @"^[a-z0-9_-]{4}[0-9]{1}$";//Шаблон 4 символа+ 1 цифра вконце
                Write("\tВведите пароль: ");
                _password = ReadLine();
                Regex regex = new Regex(PasswodPattern);
                if (regex.IsMatch(_password))
                {
                    WriteLine("Пароль Ок!");
                    fl = true;
                }
                else WriteLine("Вы ввели неверный формат для пароля");
            }
            while (fl == false);

            fl = false;
            string[] d = new string[3];//массив для записи даты рождения
            do
            {
                string DataPattern = @"^[1-2]{1}[0-9]{3}\.[0,1]{1}[0-9]{1}\.[0-9]{2}$";
                Write("\tВведите дату в формате 1985.12.31: ");
                string _data = ReadLine();
                Regex regex = new Regex(DataPattern);
                if (regex.IsMatch(_data))
                {
                    d = _data.Split('.');
                    fl = true;
                }
                else WriteLine("Вы ввели неверный формат даты");
            }
            while (fl == false);
            Victorina._user.Add(new User(_login, _password, Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2])));
            User temp = Victorina._user[Victorina._user.Count - 1];
            BD.CreatDoc(temp);
            BD.WriteChangeUser(Victorina);
            Clear();
            temp.MenuUser(Victorina);
        }         
    }
    class Program
    {
        static void Main(string[] args)
        {
             VictorinaProgram Victorina = new VictorinaProgram();
            //Считываем все данные с файлов
            BD dateBase = new BD();
            dateBase.ReadAllInfo(Victorina);

            //Меню Авторизации/регистрации
            bool fl = false;
            do
            {
                try
                {
                    WriteLine("1 - Авторизоваться; 2- Пройти регистрацию");
                    int chose1 = Convert.ToInt32(ReadLine());
                    if (chose1 == 1)
                    {
                        Victorina.Authorization(Victorina);                    
                        fl = true;
                    }
                    else if (chose1 == 2)
                    {
                        VictorinaProgram.Registration(Victorina);
                        fl = true;
                    }
                    else
                    { throw new Exception("Вы ввели неверный пункт меню"); }
                }
                catch (Exception ex)
                {
                   WriteLine(ex.Message);
                }
            }
            while (fl == false);
        }
    }
}
