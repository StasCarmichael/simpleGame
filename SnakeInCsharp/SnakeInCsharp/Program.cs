using System;
using System.Linq;
using System.Threading;


namespace SnakeInCsharp
{


    class Program
    {

        ConsoleKey dir;
        ConsoleKey prevdir;


        bool gameOver;
        bool win;

        bool whileInput;

        const int WIDTH = 40;
        const int HEIGHT = 20;

        int[] tailX = new int[100];
        int[] tailY = new int[100];
        int nTail;

        int x, y;
        int fruit_x, fruit_y;
        int score;



        static void Main()
        {
            var program = new Program();
            program.Setup();



            program.whileInput = true;
            Thread myThread = new Thread(new ThreadStart(program.Input));
            myThread.Start();



            while (!program.gameOver)
            {

                Thread myThread2 = new Thread(new ThreadStart(program.Logic));
                myThread2.Start();



                program.Draw();
                Thread.Sleep(5);



                myThread2.Join();


            }

            program.whileInput = false;
            myThread.Abort();


            if (program.win) { Console.WriteLine("\n\t\tYOU WIN"); }
            else { Console.WriteLine("\t\tYOU LOSE"); }


        }



        void Setup()
        {
            gameOver = false;
            win = false;

            Random rand = new Random();

            fruit_x = rand.Next(1, WIDTH - 3);
            fruit_y = rand.Next(1, HEIGHT - 3);

            x = WIDTH / 2;
            y = HEIGHT / 2;

            score = 0;

            dir = ConsoleKey.W;
            prevdir = ConsoleKey.W;

            nTail = 0;
        }
        void Draw()
        {
            Console.Clear();


            for (int i = 0; i < WIDTH; i++) { Console.Write("#"); ; }
            Console.WriteLine();

            for (int h = 1; h < HEIGHT - 1; h++)
            {
                Console.SetCursorPosition(0, h);
                Console.Write("#");
                Console.SetCursorPosition(WIDTH - 1, h);
                Console.Write("#");
            }

            Console.SetCursorPosition(fruit_x, fruit_y);
            Console.Write("X");

            Console.SetCursorPosition(x, y);
            Console.Write("0");

            for (int i = 0; i < nTail; i++)
            {
                Console.SetCursorPosition(tailX[i], tailY[i]);
                Console.Write("o");
            }


            Console.SetCursorPosition(0, HEIGHT - 1);
            for (int i = 0; i < WIDTH; i++) { Console.Write("#"); ; }
            Console.WriteLine();


            Console.WriteLine("\t\tScore = " + score);
        }
        void Input()
        {
            while (whileInput)
            {
                prevdir = dir;

                dir = Console.ReadKey(true).Key;

                switch (dir)
                {
                    case ConsoleKey.W: { if (prevdir == ConsoleKey.S) { dir = ConsoleKey.S; } else { dir = ConsoleKey.W; } break; }

                    case ConsoleKey.S: { if (prevdir == ConsoleKey.W) { dir = ConsoleKey.W; } else { dir = ConsoleKey.S; } break; }

                    case ConsoleKey.D: { if (prevdir == ConsoleKey.A) { dir = ConsoleKey.A; } else { dir = ConsoleKey.D; } break; }

                    case ConsoleKey.A: { if (prevdir == ConsoleKey.D) { dir = ConsoleKey.D; } else { dir = ConsoleKey.A; } break; }

                    case ConsoleKey.X: { gameOver = true; break; }
                }

                Thread.Sleep(25);

                
            }
        }
        void Logic()
        {
            int prevX = tailX[0];
            int prevY = tailY[0];
            int prev2X, prev2Y;
            tailX[0] = x;
            tailY[0] = y;

            for (int i = 1; i < nTail; i++)
            {
                prev2X = tailX[i];
                prev2Y = tailY[i];

                tailX[i] = prevX;
                tailY[i] = prevY;

                prevX = prev2X;
                prevY = prev2Y;
            }


            switch (dir)
            {
                case ConsoleKey.A:
                    x--;
                    break;

                case ConsoleKey.D:
                    x++;
                    break;

                case ConsoleKey.W:
                    y--;
                    break;

                case ConsoleKey.S:
                    y++;
                    break;
            }


            if (x <= 0) { x = WIDTH - 2; }
            else if (x >= (WIDTH - 1)) { x = 1; }
            else if (y <= 0) { y = HEIGHT - 2; }
            else if (y >= (HEIGHT - 1)) { y = 1; }


            if (fruit_x == x && fruit_y == y)
            {
                Random rand = new Random();

                fruit_x = rand.Next(1, WIDTH - 3);
                fruit_y = rand.Next(1, HEIGHT - 3);
                score += 10;
                nTail++;
            }
            if (score == 250) { win = true; gameOver = true; }
            for (int i = 0; i < nTail; i++) { if (x == tailX[i] && y == tailY[i]) { gameOver = true; } }



        }

    }
}
