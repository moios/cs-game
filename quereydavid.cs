
using System;

Console.WriteLine("=== DATOS DE UNA PERSONA ===");

Console.Write("Ingrese su nombre: ");
string nombre = Console.ReadLine() ?? "";

Console.Write("Ingrese su apellido: ");
string apellido = Console.ReadLine() ?? "";

Console.Write("Ingrese su edad: ");
int edad = int.Parse(Console.ReadLine() ?? "0");

Console.Write("Ingrese su ciudad: ");
string ciudad = Console.ReadLine() ?? "";

Console.WriteLine();
Console.WriteLine("=== DATOS INGRESADOS ===");
Console.WriteLine($"Nombre: {nombre}");
Console.WriteLine($"Apellido: {apellido}");
Console.WriteLine($"Edad: {edad} años");
Console.WriteLine($"Ciudad: {ciudad}");
