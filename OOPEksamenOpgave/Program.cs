using System;
using System.Collections.Generic;
using StregsystemCore;

namespace OOPEksamenOpgave {
    internal class Program {
        static void Main(string[] args) {
            Stregsystem stregsystem = new Stregsystem();
            stregsystem.ReadProductsFromFile("products.csv");
            stregsystem.ReadUsersFromFile("users.csv");

            StregsystemCLI stregsystemCLI = new StregsystemCLI(stregsystem);
            stregsystemCLI.Start();
        }
    }
}
