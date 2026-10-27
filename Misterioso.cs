using System;

namespace JuegoMisterioso
{
    class Misterioso
    {
        static void Main()
        {
            Console.Title = "El Número Misterioso";
            bool jugarDeNuevo = true;
            Random rnd = new Random();

            while (jugarDeNuevo)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== EL NÚMERO MISTERIOSO ===");
                Console.ResetColor();
                Console.WriteLine("Pensé un número entre 1 y 100. Tenés 7 intentos.\n");

                int secreto = rnd.Next(1, 101);
                int intentos = 7;
                bool gano = false;

                for (int i = 1; i <= intentos; i++)
                {
                    Console.Write($"Intento {i}/{intentos}: ");
                    if (!int.TryParse(Console.ReadLine(), out int n) || n < 1 || n > 100)
                    {
                        Console.WriteLine("Ingresá un número válido entre 1 y 100.");
                        i--;
                        continue;
                    }

                    if (n == secreto)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"¡Correcto! Lo descubriste en {i} intento(s). Puntaje: {(intentos - i + 1) * 100}");
                        Console.ResetColor();
                        gano = true;
                        break;
                    }

                    int dif = Math.Abs(n - secreto);
                    string pista = n < secreto ? "más ALTO" : "más BAJO";
                    string temp = dif <= 5 ? "¡Muy caliente!" : dif <= 15 ? "Tibio" : "Frío";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  El número es {pista}. {temp}");
                    Console.ResetColor();
                }

                if (!gano)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nSe acabaron los intentos. El número era {secreto}.");
                    Console.ResetColor();
                }

                Console.Write("\n¿Jugar de nuevo? (s/n): ");
                jugarDeNuevo = Console.ReadLine()?.Trim().ToLower() == "s";
            }

            Console.WriteLine("¡Gracias por jugar!");
        }
    }
}
