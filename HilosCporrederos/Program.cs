using System;
using System.Threading;
class Program
{
    static void Main()
    {
        Console.WriteLine("¡Carrera de hilos!");

        // Crear dos corredores
        int numCorredores = 5;
        Thread[] corredores = new Thread[numCorredores];


        corredorA.Start("Corredor A");
        corredorB.Start("Corredor B");
        corredorC.Start("Corredor C");
        corredorD.Start("Corredor D");
        corredorE.Start("Corredor E");

        corredorA.Join();
        corredorB.Join();
        corredorC.Join();
        corredorD.Join();
        corredorE.Join();


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