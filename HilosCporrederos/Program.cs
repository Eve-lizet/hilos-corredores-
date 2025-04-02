using System;
using System.Threading;
class Program
{
    static void Main()
    {
        Console.WriteLine("¡Carrera de hilos!");

        // Crear dos corredores
        Thread corredorA = new Thread(Correr);
        Thread corredorB = new Thread(Correr);
        Thread corredorC = new Thread(Correr);
        Thread corredorD = new Thread(Correr);
        Thread corredorE = new Thread(Correr);


        corredorA.Start("Corredor A");
        corredorB.Start("Corredor B");

        corredorA.Join();
        corredorB.Join();

        Console.WriteLine("¡Carrera terminada!");
    }

    static void Correr(object nombre)
    {
        Random rnd = new Random();
        for (int pasos = 1; pasos <= 10; pasos++)
        {
            Console.WriteLine($"{nombre} avanzó a la posición: {pasos}");
            Thread.Sleep(rnd.Next(100, 500)); // Velocidad aleatoria
        }
        Console.WriteLine($"🏁 {nombre} terminó la carrera!");
    }
}