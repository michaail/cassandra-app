﻿using System;
using cassandra_app.cassandra;

namespace cassandra_app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Backend be = new Backend();
        }
    }
}