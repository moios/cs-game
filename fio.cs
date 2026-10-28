using System;
using System.Collections.Generic;

class Territorio
{
    public string Nombre { get; set; }
    public string Dueno { get; set; } // "Jugador", "IA" o "Neutral"
    public int Tropas { get; set; }
    public List<int> Vecinos { get; set; }

    public Territorio(string nombre, string dueno, int tropas)
    {
        Nombre = nombre;
        Dueno = dueno;
        Tropas = tropas;
        Vecinos = new List<int>();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Random rand = new Random();

        bool jugarNuevamente = true;
        while (jugarNuevamente)
        {
            // Inicializar el mapa (6 territorios)
            Territorio[] mapa = new Territorio[]
            {
                new Territorio("1. Capital (Norte)", "Jugador", 5),
                new Territorio("2. Colinas (Oeste)", "Jugador", 3),
                new Territorio("3. Bosque (Centro)", "Neutral", 2),
                new Territorio("4. Montaña (Este)", "IA", 4),
                new Territorio("5. Desierto (Sur)", "IA", 5),
                new Territorio("6. Fortaleza (Suroeste)", "IA", 3)
            };

            // Conexiones (Vecindad)
            mapa[0].Vecinos = new List<int> { 1, 2, 3 };
            mapa[1].Vecinos = new List<int> { 0, 2, 5 };
            mapa[2].Vecinos = new List<int> { 0, 1, 3, 4 };
            mapa[3].Vecinos = new List<int> { 0, 2, 4 };
            mapa[4].Vecinos = new List<int> { 2, 3, 5 };
            mapa[5].Vecinos = new List<int> { 1, 4 };

            int ronda = 1;
            bool partidaActiva = true;

            while (partidaActiva)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine($"         ⚔️ ESTRATEGIA: RONDA {ronda} ⚔️");
                Console.WriteLine("========================================");

                // Mostrar Mapa
                Console.WriteLine("ESTADO DEL MAPA:");
                foreach (var t in mapa)
                {
                    string icono = t.Dueno == "Jugador" ? "🔵 [TÚ]" : (t.Dueno == "IA" ? "🔴 [IA]" : "⚪ [NEUTRAL]");
                    Console.WriteLine($"{t.Nombre} | Dueño: {icono} | Tropas: 🛡️ {t.Tropas}");
                }
                Console.WriteLine("----------------------------------------");

                // Verificar condiciones de victoria/derrota
                int territoriosJugador = 0;
                int territoriosIA = 0;
                foreach (var t in mapa)
                {
                    if (t.Dueno == "Jugador") territoriosJugador++;
                    if (t.Dueno == "IA") territoriosIA++;
                }

                if (territoriosIA == 0)
                {
                    Console.WriteLine("\n🏆 ¡VICTORIA! Has aniquilado a todas las fuerzas enemigas.");
                    break;
                }
                if (territoriosJugador == 0)
                {
                    Console.WriteLine("\n💀 ¡DERROTA! Tu imperio ha caído.");
                    break;
                }

                // ==================== FASE 1: REFUERZOS ====================
                int refuerzos = Math.Max(2, territoriosJugador + 1);
                Console.WriteLine($"\n[Fase 1: Refuerzos] Tienes {refuerzos} tropas nuevas para desplegar.");

                bool refuerzoValido = false;
                while (!refuerzoValido)
                {
                    Console.Write("¿En qué territorio tuyo deseas desplegarlas? (1-6): ");
                    if (int.TryParse(Console.ReadLine(), out int tIdx) && tIdx >= 1 && tIdx <= 6)
                    {
                        if (mapa[tIdx - 1].Dueno == "Jugador")
                        {
                            mapa[tIdx - 1].Tropas += refuerzos;
                            Console.WriteLine($"✅ ¡Desplegaste {refuerzos} tropas en {mapa[tIdx - 1].Nombre}!");
                            refuerzoValido = true;
                        }
                        else
                        {
                            Console.WriteLine("⚠️ No puedes reforzar un territorio que no te pertenece.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Opción inválida.");
                    }
                }

                // ==================== FASE 2: ATAQUE ====================
                Console.WriteLine("\n[Fase 2: Ataque]");
                Console.Write("¿Deseas realizar un ataque en este turno? (s/n): ");
                string quiereAtacar = Console.ReadLine()?.Trim().ToLower();

                if (quiereAtacar == "s" || quiereAtacar == "si" || quiereAtacar == "sí")
                {
                    Console.Write("Elige tu territorio atacante (1-6): ");
                    if (int.TryParse(Console.ReadLine(), out int origenIdx) && origenIdx >= 1 && origenIdx <= 6)
                    {
                        origenIdx--;
                        if (mapa[origenIdx].Dueno == "Jugador" && mapa[origenIdx].Tropas > 1)
                        {
                            Console.Write("Elige el territorio enemigo/neutral a atacar (vecino): ");
                            if (int.TryParse(Console.ReadLine(), out int destinoIdx) && destinoIdx >= 1 && destinoIdx <= 6)
                            {
                                destinoIdx--;
                                if (mapa[origenIdx].Vecinos.Contains(destinoIdx) && mapa[destinoIdx].Dueno != "Jugador")
                                {
                                    // Simulación de combate por dados
                                    int dadoAtacante = rand.Next(1, 7);
                                    int dadoDefensor = rand.Next(1, 7);

                                    Console.WriteLine($"\n🎲 ¡COMBATE! Atacante sacó [{dadoAtacante}] vs Defensor [{dadoDefensor}]");

                                    if (dadoAtacante >= dadoDefensor)
                                    {
                                        Console.WriteLine("🔥 ¡Ataque exitoso! El defensor pierde una tropa.");
                                        mapa[destinoIdx].Tropas--;
                                        if (mapa[destinoIdx].Tropas <= 0)
                                        {
                                            Console.WriteLine($"🎉 ¡Has conquistado {mapa[destinoIdx].Nombre}!");
                                            mapa[destinoIdx].Dueno = "Jugador";
                                            mapa[destinoIdx].Tropas = 1; // Deja 1 tropa de guardia
                                            mapa[origenIdx].Tropas--;   // La mueve del origen
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("🛡️ ¡Defensa exitosa! El atacante pierde una tropa.");
                                        mapa[origenIdx].Tropas--;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("⚠️ Territorio no válido, no es vecino o es tuyo.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("⚠️ Territorio inválido o necesitas al menos 2 tropas para atacar.");
                        }
                    }
                }

                // ==================== FASE 3: TURNO DE LA IA ====================
                Console.WriteLine("\n[Turno de la IA pensando... Presiona ENTER para continuar]");
                Console.ReadLine();

                // IA simple: busca un territorio de la IA con más de 2 tropas y ataca un vecino del jugador si puede
                foreach (var tIA in mapa)
                {
                    if (tIA.Dueno == "IA" && tIA.Tropas > 2)
                    {
                        foreach (int vIdx in tIA.Vecinos)
                        {
                            if (mapa[vIdx].Dueno == "Jugador")
                            {
                                Console.WriteLine($"🔴 ¡La IA ataca tu territorio {mapa[vIdx].Nombre} desde {tIA.Nombre}!");
                                int dadoIA = rand.Next(1, 7);
                                int dadoJugador = rand.Next(1, 7);
                                Console.WriteLine($"🎲 Dados -> IA: [{dadoIA}] | Tú: [{dadoJugador}]");

                                if (dadoIA >= dadoJugador)
                                {
                                    mapa[vIdx].Tropas--;
                                    Console.WriteLine("⚠️ ¡La IA hirió tus defensas en ese territorio!");
                                    if (mapa[vIdx].Tropas <= 0)
                                    {
                                        mapa[vIdx].Dueno = "IA";
                                        mapa[vIdx].Tropas = 1;
                                        Console.WriteLine($"💔 ¡Has perdido el control de {mapa[vIdx].Nombre}!");
                                    }
                                }
                                else
                                {
                                    tIA.Tropas--;
                                    Console.WriteLine("🛡️ ¡Repeliste con éxito el ataque de la IA!");
                                }
                                break; // Un ataque por turno de IA
                            }
                        }
                        break;
                    }
                    else if (tIA.Dueno == "IA")
                    {
                        // Si la IA está débil, se refuerza
                        tIA.Tropas += 1;
                    }
                }

                ronda++;
                Console.WriteLine("\nPresiona ENTER para ir a la siguiente ronda...");
                Console.ReadLine();
            }

            // Fin de partida
            Console.Write("\n¿Quieres jugar otra partida de estrategia? (s/n): ");
            string respuesta = Console.ReadLine()?.Trim().ToLower();
            if (respuesta != "s" && respuesta != "si" && respuesta != "sí")
            {
                jugarNuevamente = false;
                Console.WriteLine("\n¡Gracias por jugar! ¡Comandante retirado! 🎖️");
            }
        }
    }
}