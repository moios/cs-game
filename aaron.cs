using System;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Estado del Jugador
        int jugadorHp = 100;
        int jugadorMaxHp = 100;
        int pociones = 3;
        int oroTotal = 50;

        // Progresión y Combate
        int jugadorNivel = 1;
        int jugadorExp = 0;
        int expNecesaria = 50;
        int danioBaseEspada = 15;

        int habitacion = 1;
        int totalHabitaciones = 5;
        bool juegoTerminadoPorUsuario = false;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine(" ⚔️ AVENTURA EN LA MAZMORRA (RPG + TIENDA C#) ⚔️");
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.WriteLine("\nSobrevive a las 5 salas y derrota al jefe final.");
        Console.WriteLine("💡 Tip: Puedes presionar [ESC] o escribir 'Q' en cualquier combate para rendirte.");
        Console.WriteLine("\nPresiona cualquier tecla para comenzar...");
        Console.ReadKey();

        Random rnd = new Random();

        // Bucle principal de la mazmorra
        while (habitacion <= totalHabitaciones && jugadorHp > 0)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"--- 🏰 HABITACIÓN {habitacion} de {totalHabitaciones} ---");
            Console.ResetColor();
            Console.WriteLine($"⭐ Nivel: {jugadorNivel} (EXP: {jugadorExp}/{expNecesaria})");
            Console.WriteLine($"❤️ HP: {jugadorHp}/{jugadorMaxHp}  |  🧪 Pociones: {pociones}  |  💰 Oro: {oroTotal}  |  🗡️ Daño: {danioBaseEspada}");
            Console.WriteLine("--------------------------------------------------\n");

            bool esJefe = (habitacion == totalHabitaciones);
            string nombreEnemigo = esJefe ? "🐉 Dragón Guardián" : ObtenerEnemigoAleatorio(rnd);

            int enemigoHp = esJefe ? 120 : rnd.Next(30, 50) + (jugadorNivel * 5);
            int enemigoMaxHp = enemigoHp;
            int enemigoAtaqueMax = esJefe ? 24 : 15;

            Console.WriteLine($"¡Un {nombreEnemigo} salvaje ha aparecido! (HP: {enemigoHp})\n");

            // Combate por turnos contra el enemigo actual
            while (enemigoHp > 0 && jugadorHp > 0)
            {
                Console.WriteLine("¿Qué deseas hacer?");
                Console.WriteLine("1. Atacar con espada 🗡️");
                Console.WriteLine("2. Usar poción de vida 🧪 (Recupera 40 HP)");
                Console.WriteLine("[ESC / Q] Rendirse / Salir del juego");
                Console.Write("\nElige una opción: ");

                // Comprobamos si presionó ESC directamente en el teclado
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
                    if (tecla.Key == ConsoleKey.Escape)
                    {
                        juegoTerminadoPorUsuario = true;
                        break;
                    }
                }

                string opcion = Console.ReadLine()?.Trim().ToUpper() ?? "";
                Console.WriteLine();

                // Si escribe 'Q' también se sale
                if (opcion == "Q")
                {
                    juegoTerminadoPorUsuario = true;
                    break;
                }

                if (opcion == "1")
                {
                    int danioJugador = rnd.Next(danioBaseEspada - 5, danioBaseEspada + 5) + (jugadorNivel * 2);
                    enemigoHp -= danioJugador;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"¡Atacas al {nombreEnemigo} y le infliges {danioJugador} de daño!");
                    Console.ResetColor();
                }
                else if (opcion == "2")
                {
                    if (pociones > 0)
                    {
                        pociones--;
                        int curacion = 40;
                        jugadorHp = Math.Min(jugadorMaxHp, jugadorHp + curacion);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Bebes una poción y recuperas {curacion} HP. (Pociones restantes: {pociones})");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("¡No te quedan pociones! Pierdes tu turno.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine("Opción inválida. Pierdes tu turno por vacilar.");
                }

                // Si el jugador decidió salir, rompemos el bucle de combate
                if (juegoTerminadoPorUsuario) break;

                // Turno del enemigo (si sigue vivo)
                if (enemigoHp > 0)
                {
                    int danioEnemigo = rnd.Next(5, enemigoAtaqueMax);
                    jugadorHp -= danioEnemigo;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"El {nombreEnemigo} contraataca y te causa {danioEnemigo} de daño.");
                    Console.ResetColor();
                }

                Console.WriteLine("\n--------------------------------------------------");
                Console.WriteLine($"Tú: {Math.Max(0, jugadorHp)}/{jugadorMaxHp} HP  |  {nombreEnemigo}: {Math.Max(0, enemigoHp)}/{enemigoMaxHp} HP");
                Console.WriteLine("--------------------------------------------------\n");
            }

            // Si el jugador se rindió, salimos de todo el juego
            if (juegoTerminadoPorUsuario)
            {
                break;
            }

            // Comprobar si el jugador murió
            if (jugadorHp <= 0)
            {
                break;
            }

            // Victoria sobre el enemigo de la sala
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n🎉 ¡Has derrotado al {nombreEnemigo}!");
            Console.ResetColor();

            int expGanada = esJefe ? 100 : rnd.Next(25, 45);
            int oroGanado = esJefe ? 150 : rnd.Next(15, 35);

            jugadorExp += expGanada;
            oroTotal += oroGanado;

            Console.WriteLine($"✨ ¡Has ganado {expGanada} puntos de experiencia!");
            Console.WriteLine($"💰 ¡Encontraste {oroGanado} de oro en los restos!");

            if (!esJefe && rnd.Next(100) < 40)
            {
                pociones++;
                Console.WriteLine("🧪 ¡Encontraste una poción extra tirada en el suelo!");
            }

            while (jugadorExp >= expNecesaria)
            {
                jugadorExp -= expNecesaria;
                jugadorNivel++;
                expNecesaria += 25;
                jugadorMaxHp += 25;
                jugadorHp = jugadorMaxHp;

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n==================================================");
                Console.WriteLine($"   🌟 ¡SUBISTE AL NIVEL {jugadorNivel}! 🌟");
                Console.WriteLine($"   ¡HP máximo aumentado y salud restaurada al 100%!");
                Console.WriteLine("==================================================");
                Console.ResetColor();
            }

            // Tienda entre habitaciones
            if (habitacion < totalHabitaciones)
            {
                bool enTienda = true;
                while (enTienda)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("==================================================");
                    Console.WriteLine("               🛒 TIENDA DEL MERCADER             ");
                    Console.WriteLine("==================================================");
                    Console.ResetColor();
                    Console.WriteLine($"💰 Tu Oro: {oroTotal} monedas  |  ❤️ HP: {jugadorHp}/{jugadorMaxHp}  |  🧪 Pociones: {pociones}  |  🗡️ Daño: {danioBaseEspada}");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("1. Comprar Poción de Vida (+40 HP) - 💰 15 oro");
                    Console.WriteLine("2. Mejorar Espada (+5 de daño base) - 💰 30 oro");
                    Console.WriteLine("3. Poción de Curación Total (Restaura todo el HP) - 💰 25 oro");
                    Console.WriteLine("4. Continuar a la siguiente habitación 🚪");
                    Console.Write("\nElige una opción (1-4): ");

                    string opcionTienda = Console.ReadLine()?.Trim() ?? "";

                    if (opcionTienda == "1")
                    {
                        if (oroTotal >= 15)
                        {
                            oroTotal -= 15;
                            pociones++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n¡Has comprado una poción!");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n¡No tienes suficiente oro!");
                        }
                        Console.ResetColor();
                        Console.WriteLine("Presiona una tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if (opcionTienda == "2")
                    {
                        if (oroTotal >= 30)
                        {
                            oroTotal -= 30;
                            danioBaseEspada += 5;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n¡Has mejorado tu espada! Ahora haces más daño.");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n¡No tienes suficiente oro!");
                        }
                        Console.ResetColor();
                        Console.WriteLine("Presiona una tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if (opcionTienda == "3")
                    {
                        if (oroTotal >= 25)
                        {
                            if (jugadorHp < jugadorMaxHp)
                            {
                                oroTotal -= 25;
                                jugadorHp = jugadorMaxHp;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\n¡Has sanado por completo tus heridas!");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\n¡Ya tienes la salud al máximo!");
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n¡No tienes suficiente oro!");
                        }
                        Console.ResetColor();
                        Console.WriteLine("Presiona una tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if (opcionTienda == "4")
                    {
                        enTienda = false;
                    }
                    else
                    {
                        Console.WriteLine("\nOpción inválida.");
                        Console.ReadKey();
                    }
                }
            }

            habitacion++;
        }

        // Fin del juego
        Console.Clear();
        if (juegoTerminadoPorUsuario)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("==================================================");
            Console.WriteLine("          🏳️ HAS ABANDONADO LA PARTIDA 🏳️");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine("Decidiste huir de la mazmorra antes de terminar.");
        }
        else if (jugadorHp > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================================================");
            Console.WriteLine("             🏆 ¡HAS GANADO EL JUEGO! 🏆          ");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine($"Alcanzaste el Nivel {jugadorNivel}.");
            Console.WriteLine($"Derrotaste al Dragón y escapaste con {oroTotal} monedas sobrantes 💰.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("==================================================");
            Console.WriteLine("                 💀 HAS MUERTO 💀                 ");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine($"Llegaste hasta el Nivel {jugadorNivel}. ¡Inténtalo de nuevo!");
        }

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }

    static string ObtenerEnemigoAleatorio(Random rnd)
    {
        string[] enemigos = { "Goblin Astuto", "Esqueleto Guerrero", "Orco Berserker", "Llama Viviente" };
        return enemigos[rnd.Next(enemigos.Length)];
    }
}