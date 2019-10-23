using System;
using System.Threading;


namespace lab3
{
    class Program
    {
        private const int CountSream = 6;//Количество потоков

        static void Main(string[] args)
        {
            for (int i = 1; i < CountSream; i++)
            {
                Reader reader = new Reader(i);
            }
            Console.ReadKey();
        }
    }
    class Reader
    {
        // создаем семафор
        static Mutex m= new Mutex();
        Thread myThread;

        public Reader(int i)//создаем потоки и нумеруим их
        {
            myThread = new Thread(Read);
            myThread.Name = "Читатель " + i.ToString();
            myThread.Start();
        }

        public void Read()
        {
            
            m.Lock();//закрыли вход
            Console.WriteLine("{0} входит в библиотеку", Thread.CurrentThread.Name);
            Console.WriteLine("{0} читает", Thread.CurrentThread.Name);           
            Thread.Sleep(1000);
            Console.WriteLine("{0} покидает библиотеку", Thread.CurrentThread.Name);
            m.UnLock();//открыли вход
            Thread.Sleep(1000);
        }
    }
    class Mutex
    {
        public int ID = -1;
        public void Lock()
        {
            while ((Interlocked.CompareExchange(ref ID, Thread.CurrentThread.ManagedThreadId, -1) != -1))//Ожидание потоками открытия семафора
            {
                Thread.Yield();
            }
        }
        public  void UnLock()//Разблокировка входа
        {
            Interlocked.CompareExchange(ref ID, -1, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
