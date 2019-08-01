using System;
using System.Threading;

namespace Threads5
{
    class ThreadsRunners
    {
        public static TextPresentation tp = new TextPresentation();
        public static void Runner1()
        {
            Console.WriteLine("thread_1 run!");
            Console.WriteLine("thread_1 – calling TextPresentation.showText");
            tp.ShowText("*");
            Console.WriteLine("thread_1 stop!");
        }

        public static void Runner2()
        {
            Console.WriteLine("thread_2 run!");
            Console.WriteLine("thread_2 – calling TextPresentation.showText");
            tp.ShowText("|");
            Console.WriteLine("thread_2 stop!");
        }

        static void Main(string[] args)
        {
            ThreadStart runner1 = new ThreadStart(Runner1);
            ThreadStart runner2 = new ThreadStart(Runner2);

            Thread th1 = new Thread(runner1);
            Thread th2 = new Thread(runner2);

            th1.Start();
            th2.Start();
        }
    }

    class TextPresentation
    {
        public Mutex mutex;

        public TextPresentation()
        {
            mutex = new Mutex();
        }

        public void ShowText(string text)
        {
            int i;
            // Объект синхронизации в данном конкретном случае – представитель класса TextPresentation.
            // Для его обозначения используется первичное выражение this.
            //1. Блокировка кода монитором (начало) 
            Monitor.Enter(this);
            //2. Критическая секция кода (начало)   
            lock (this)
            {
                mutex.WaitOne();//3.Блокировка кода мьютексом (начало)//
                try
                {
                    Console.WriteLine("\n" + (char)31 + (char)31 + (char)31 + (char)31);
                    for (i = 0; i < 360; i++)
                    {
                        Console.Write(text);
                        throw new Exception();
                    }
                        Console.WriteLine("\n" + (char)30 + (char)30 + (char)30 + (char)30);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
                //3.Блокировка кода мьютексом (конец)//
                //2. Критическая секция кода (конец) // 
            }
            //1. Блокировка кода монитором (конец) 
            Monitor.Exit(this);
        }
    }
}