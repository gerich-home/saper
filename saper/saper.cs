using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace saper
{
    public class Sapper
    {
        public static void Main()
        {
            Sapper s = new Sapper(10, 15);
            s.Run();
        }

        public Sapper(int n, int nmines)
        {
            Console.ForegroundColor = ConsoleColor.White;
            this.n = n;
            this.x = n / 2;
            this.y = n / 2;
            this.nmines = nmines;
            mines = new int[n + 2, n + 2];
            numbers = new int[n, n];
            visible = new bool[n, n];
            visiblecount = 0;
            detected = new bool[n, n];
            InitField();
        }

        private void PrintField()
        {
            PrintField(false);
        }

        private void PrintField(bool showall)
        {
            Console.Clear();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i == x) && (j == y))
                        Console.ForegroundColor=ConsoleColor.Green;
                    if (showall)
                        if (mines[i + 1, j + 1] == 1)
                            Console.Write("*");
                        else if (numbers[i, j] == 0)
                            Console.Write(" "); 
                        else
                            Console.Write("{0}", numbers[i, j]);
                    else if (detected[i, j])
                        Console.Write("x"); 
                    else if (visible[i, j])
                        if (numbers[i, j] == 0)
                            if ((i == x) && (j == y))
                                Console.Write('\u2591');
                            else
                                Console.Write(" ");
                        else
                            Console.Write("{0}", numbers[i, j]);
                    else
                        Console.Write("#");
                    if ((i == x) && (j == y))
                        Console.ForegroundColor=ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }

        private void InitField()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int i, j;
            for (i = 0; i < nmines; i++)
            {
                int rx;
                int ry;
                do
                {
                    rx = rnd.Next(n) + 1;
                    ry = rnd.Next(n) + 1;
                }
                while (mines[rx, ry] == 1);
                mines[rx, ry] = 1;
            }
            for (i = 0; i < n; i++)
                for (j = 0; j < n; j++)
                {
                    numbers[i, j] = mines[i, j] + mines[i + 1, j] + mines[i + 2, j] + 
                        mines[i, j + 1] + mines[i + 2, j + 1] + mines[i, j + 2] + mines[i + 1, j + 2] + mines[i + 2, j + 2];
                }
        }

        int n;
        int nmines;
        int[,] mines;
        int[,] numbers;
        bool[,] visible;
        bool[,] detected;
        int visiblecount;
        int x, y;

        public void Run()
        {
            Console.Write("Привет! Как тебя зовут? \n>");
            string name = Console.ReadLine();
            Console.WriteLine("Ну что, {0}, поиграем в сапёра? \n На поле {1} мин. Удачи!;-)", name, nmines);
            Console.ReadLine();
            do
            {
                PrintField();
                ConsoleKeyInfo ki = Console.ReadKey(true);
                switch (ki.Key)
                {
                    case ConsoleKey.LeftArrow:
                        y = (y == 0) ? (n - 1): (y - 1);
                        break;
                    case ConsoleKey.RightArrow:
                        y = (y == n - 1) ? (0) : (y + 1);
                        break;
                    case ConsoleKey.DownArrow:
                        x = (x == n - 1) ? (0) : (x + 1);
                        break;
                    case ConsoleKey.UpArrow:
                        x = (x == 0) ? (n - 1) : (x - 1);
                        break;
                    case ConsoleKey.Enter:
                        if (mines[x + 1, y + 1] == 1)
                        {
                            PrintField(true);
                            Console.WriteLine("\nТы Looser!");
                            Console.ReadLine();
                            return;
                        }
                        else
                        {
                            Show(x, y);
                            if (visiblecount == n * n - nmines)
                            {
                                PrintField(true);
                                Console.WriteLine("\nБраво!");
                                Console.ReadLine();
                                return;
                            }
                        }
                        break;
                    case ConsoleKey.X:
                        detected[x, y] = !detected[x, y];
                        break;
                }
            }
            while (true);
        }

        private void Show(int x, int y)
        {
            if ((x < 0) || (x >= n) || (y < 0) || (y >= n))
                return;
            bool b = !visible[x, y];
            visible[x, y] = true;
            if (b)
                visiblecount++;
            if ((numbers[x, y] == 0) && b)
            {
                Show(x - 1, y - 1);
                Show(x - 1, y);
                Show(x - 1, y + 1);
                Show(x, y - 1);
                Show(x, y + 1);
                Show(x + 1, y - 1);
                Show(x + 1, y);
                Show(x + 1, y + 1);
            }
            
        }
    }
}
