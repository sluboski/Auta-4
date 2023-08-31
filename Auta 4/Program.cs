using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

struct Object
{
    public int x;
    public int y;
    public char c;
    public ConsoleColor color;
}

class Program
{   //Basic functions
    static void PrintObjects(int x, int y, char c,
        ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
    }
    static void PrintResults(int x, int y, string str,
        ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(str);
    }
    static void Main()
    {
        //default values
        double speed = 100.0;
        double acceleration = 0.5;
        int playfieldWidth = 5;
        int livesCount = 5;
        Console.BufferHeight = Console.WindowHeight = 25;
        Console.BufferWidth = Console.WindowWidth = 70;
        Object userCar = new Object();
        userCar.x = 4;
        userCar.y = Console.WindowHeight - 2;
        userCar.c = '\u0001';
        userCar.color = ConsoleColor.Yellow;
        
        //object generator 15% chance slow, 15% chance bonus life, 70% chance for barrier
        Random randomGenerator = new Random();
        List<Object> objects = new List<Object>();
        while (true)
        {
            speed += acceleration;
            if (speed > 400)
            {
                speed = 400; //max speed
            }

            bool hit = false;
            {
                int chance = randomGenerator.Next(0, 100);
                if (chance < 15)
                {
                    Object newObject = new Object();
                    newObject.color = ConsoleColor.Cyan;
                    newObject.c = 'v';
                    newObject.x = randomGenerator.Next(0, playfieldWidth);
                    newObject.y = 0;
                    objects.Add(newObject);
                }
                else if (chance < 30)
                {
                    Object newObject = new Object();
                    newObject.color = ConsoleColor.Cyan;
                    newObject.c = '+';
                    newObject.x = randomGenerator.Next(0, playfieldWidth);
                    newObject.y = 0;
                    objects.Add(newObject);
                }
                else
                {
                    Object newObject = new Object();
                    newObject.color = ConsoleColor.Red;
                    newObject.x = randomGenerator.Next(0, playfieldWidth);
                    newObject.y = 0;
                    newObject.c = '-';
                    objects.Add(newObject);
                }
            }

            //car navigation
            while (Console.KeyAvailable) 
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    if (userCar.x - 1 >= 0)
                    {
                        userCar.x = userCar.x - 1;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (userCar.x + 1 < playfieldWidth)
                    {
                        userCar.x = userCar.x + 1;
                    }
                }
            }

            //game logic
            List<Object> newList = new List<Object>();
            for (int i = 0; i < objects.Count; i++)
            {
                Object oldCar = objects[i];
                Object newObject = new Object();
                newObject.x = oldCar.x;
                newObject.y = oldCar.y + 1;
                newObject.c = oldCar.c;
                newObject.color = oldCar.color;
                if (newObject.c == 'v' && newObject.y == userCar.y && newObject.x == userCar.x)
                {
                    speed -= 20;
                }
                if (newObject.c == '+' && newObject.y == userCar.y && newObject.x == userCar.x)
                {
                    livesCount++;
                }
                if (newObject.c == '-' && newObject.y == userCar.y && newObject.x == userCar.x)
                {
                    livesCount--;
                    hit = true;
                    speed += 50;
                    if (speed > 400)
                    {
                        speed = 400;
                    }
                    if (livesCount <= 0)
                    {
                        PrintResults(8, 10, "GAME OVER!", ConsoleColor.Red);
                        PrintResults(8, 12, "Press [enter] to exit", ConsoleColor.Green);
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                if (newObject.y < Console.WindowHeight)
                {
                    newList.Add(newObject);
                }
            }
            objects = newList;
            Console.Clear();
            if (hit)
            {
                objects.Clear();
                PrintObjects(userCar.x, userCar.y, 'X', ConsoleColor.Red);
            }
            else
            {
                PrintObjects(userCar.x, userCar.y, userCar.c, userCar.color);
            }
            foreach (Object car in objects)
            {
                PrintObjects(car.x, car.y, car.c, car.color);
            }

            //Informacje
            PrintResults(8, 4, "Lives: " + livesCount, ConsoleColor.White);
            PrintResults(8, 5, "Speed: " + speed, ConsoleColor.White);
            PrintResults(8, 6, "Acceleration: " + acceleration, ConsoleColor.White);
            //Tickrate
            Thread.Sleep((int)(800 - speed));
        }
    }
}