using System;
using System.Collections.Generic;

namespace JuegoMisterioso
{
    class Misterioso
    {
        static void Main()
        {
            Console.Title = "El Número Misterioso - Edición Mejorada";
            bool jugarDeNuevo = true;
            Random rnd = new Random();

            // Estadísticas de la sesión
            int partidasJugadas = 0;
            int partidasGanadas = 0;
            int rachaActual = 0;
            int puntajeTotal = 0;

            while (jugarDeNuevo)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("======================================");
                Console.WriteLine("      === EL NÚMERO MISTERIOSO ==     ");
                Console.WriteLine("======================================");
                Console.ResetColor();

                // Menú de Dificultad
                Console.WriteLine("\nSeleccioná la dificultad:");
                Console.WriteLine("1. Fácil   (1 a 50  - 10 intentos)");
                Console.WriteLine("2. Normal  (1 a 100 - 7 intentos)");
                Console.WriteLine("3. Difícil (1 a 200 - 5 intentos)");
                Console.Write("\nOpción (1-3): ");

                int maxRango = 100;
                int intentos = 7;

                string opcionDif = Console.ReadLine();
                switch (opcionDif)
                {
                    case "1":
                        maxRango = 50;
                        intentos = 10;
                        break;
                    case "3":
                        maxRango = 200;
                        intentos = 5;
                        break;
                    default: // Opción 2 o por defecto
                        maxRango = 100;
                        intentos = 7;
                        break;
                }

                int secreto = rnd.Next(1, maxRango + 1);
                bool gano = false;
                List<int> intentosAnteriores = new List<int>();
                int ultimoDif = -1;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"¡Pensé un número entre 1 y {maxRango}! Tenés {intentos} intentos.\n");
                Console.ResetColor();

                for (int i = 1; i <= intentos; i++)
                {
                    Console.Write($"Intento [{i}/{intentos}] - Ingresá un número: ");

                    if (!int.TryParse(Console.ReadLine(), out int n) || n < 1 || n > maxRango)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"  [!] Ingresá un número válido entre 1 y {maxRango}.");
                        Console.ResetColor();
                        i--; // No gastar intento por error de tipeo
                        continue;
                    }

                    // Verificar si ya repitió el número
                    if (intentosAnteriores.Contains(n))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("  [!] ¡Ya probaste ese número! Intentá con uno diferente.");
                        Console.ResetColor();
                        i--;
                        continue;
                    }

                    intentosAnteriores.Add(n);

                    if (n == secreto)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        int puntosObtenidos = (intentos - i + 1) * 100 * (maxRango == 200 ? 3 : maxRango == 100 ? 2 : 1);
                        Console.WriteLine($"\n¡Felicidades! ¡Correcto! Lo descubriste en {i} intento(s).");
                        Console.WriteLine($"Puntaje obtenido: {puntosObtenidos} puntos.");
                        Console.ResetColor();

                        gano = true;
                        partidasGanadas++;
                        rachaActual++;
                        puntajeTotal += puntosObtenidos;
                        break;
                    }

                    // Pistas dinámicas
                    int dif = Math.Abs(n - secreto);
                    string pista = n < secreto ? "más ALTO 📈" : "más BAJO 📉";
                    string temp = dif <= (maxRango * 0.05) ? "¡Muy caliente! 🔥" : dif <= (maxRango * 0.15) ? "Tibio 🌤️" : "Frío ❄️";

                    // Comparar si se está acercando respecto al intento anterior
                    string dinamicaDistancia = "";
                    if (i > 1)
                    {
                        if (dif < ultimoDif)
                            dinamicaDistancia = "(¡Te estás acercando! 🎯)";
                        else if (dif > ultimoDif)
                            dinamicaDistancia = "(Te estás alejando ⚠️)";
                        else
                            dinamicaDistancia = "(Estás a la misma distancia)";
                    }

                    ultimoDif = dif;

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"  -> El número es {pista}. {temp} {dinamicaDistancia}");
                    Console.WriteLine($"  -> Historial de intentos: [ {string.Join(", ", intentosAnteriores)} ]\n");
                    Console.ResetColor();
                }

                partidasJugadas++;

                if (!gano)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n¡Se acabaron los intentos! El número secreto era: {secreto}");
                    Console.ResetColor();
                    rachaActual = 0; // Se rompe la racha
                }

                // Mostrar estadísticas de la sesión
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n--------------------------------------");
                Console.WriteLine("          ESTADÍSTICAS DE JUEGO       ");
                Console.WriteLine("--------------------------------------");
                Console.ResetColor();
                Console.WriteLine($" Partidas jugadas: {partidasJugadas}");
                Console.WriteLine($" Partidas ganadas: {partidasGanadas}");
                Console.WriteLine($" Racha actual    : {rachaActual}");
                Console.WriteLine($" Puntaje total   : {puntajeTotal} pts");
                Console.WriteLine("--------------------------------------");

                Console.Write("\n¿Querés jugar de nuevo? (s/n): ");
                jugarDeNuevo = Console.ReadLine()?.Trim().ToLower() == "s";
            }

            Console.Clear();
            Console.WriteLine("¡Gracias por jugar! Volvé pronto.");
        }
    }
}