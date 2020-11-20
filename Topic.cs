using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Victorina
{ 
     public class Topic
    {
        public string name { get; set; }
        public List<QuestionAnswer> _questionAnswers = new List<QuestionAnswer>();
        public List<string> Lideru = new List<string>();//Топ пользовователей по текущей викторине
        public Topic(string _name)
        {
            name = _name;
        }       
        public void Show1()
        {
            int k = 1;
            foreach(QuestionAnswer qa in _questionAnswers)
            {
                Write(k);
                qa.Show();
                k++;
            }
        }
        //Функция вывода вопросов и получения ответов
        public int VictorinaOpros()
        {
            int RezultCount = 0;//количество правильных ответов

            foreach (QuestionAnswer qa in _questionAnswers)
            {
                qa.Show();
                List<string> tempAnsver = new List<string>();   //Временный массив для ответов пользователя             
                Write("Укажите количество верных вариантов ответов (1/2/3): ");
                int a = Convert.ToInt32(ReadLine());               
                Write("\nНапишите Верный ответ:\n");
                for (int i=0; i<a;i++)
                {
                    tempAnsver.Add(ReadLine());
                }
                if (a == qa.CountAnsw)//если количество ответов указанов верно, проверяем на правильность ответов
                {
                    int b = 0;
                    foreach(string s in tempAnsver)
                    {
                        foreach (KeyValuePair<string, int> An in qa.Answers)
                        {
                            if (s == An.Key && An.Value == 1) b++;
                        }                           
                    }
                    if (a==b) //все ответы указали верно, ответ защитывается
                    {
                        RezultCount++;
                    }
                }         
            }
            return RezultCount;
        }


    }
    public class QuestionAnswer
    {
        public string Question;
        public int CountAnsw;//количество правильных ответов
        public Dictionary<string, int> Answers = new Dictionary<string, int>();
        
        public QuestionAnswer(string question, int count, Dictionary<string, int> answers)
        {
            Question = question;
            CountAnsw = count;
            Answers = answers;
        }
        public void Show()
        {
            WriteLine($"\n{Question}?");
            foreach (KeyValuePair<string, int> d in Answers)
            {
                WriteLine($" - {d.Key}");
            }
        }
     
    }
}
