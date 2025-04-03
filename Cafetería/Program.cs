using System;
using System.Threading;

class Cafeteria
{
    private static int caja = 0; // Dinero en caja (recurso compartido)
    private static object bloqueo = new object(); // Para sincronización

    static void Main()
    {
        Console.WriteLine("¡Cafetería abierta!");

        // Simulación: 3 clientes hacen pedidos en paralelo
        Thread[] clientes = new Thread[3];
        for (int i = 0; i < 3; i++)
        {
            int numCliente = i + 1;
            clientes[i] = new Thread(() => HacerPedido(numCliente));
            clientes[i].Start();
        }

        // Barista trabajando en segundo plano
        Thread barista = new Thread(PrepararCafe);
        barista.Start();

        // Esperar a que todos los clientes terminen
        foreach (Thread cliente in clientes)
        {
            cliente.Join();
        }

        barista.Join(); // Esperar al barista

        Console.WriteLine($"\nCierre de caja: ${caja}");
        Console.WriteLine("¡Cafetería cerrada!");
    }

    static void HacerPedido(int numCliente)
    {
        Console.WriteLine($"Cliente {numCliente}: Haciendo pedido...");
        Thread.Sleep(2000); // Tiempo para decidir

        lock (bloqueo) // Bloquea la caja para evitar condiciones de carrera
        {
            caja += 5; // Cada café cuesta $5
            Console.WriteLine($"Cliente {numCliente}: Pagó $5. Caja: ${caja}");
        }
    }

    static void PrepararCafe()
    {
        for (int i = 1; i <= 3; i++)
        {
            Console.WriteLine($"Barista: Preparando café {i}...");
            Thread.Sleep(3000); // Tiempo de preparación
            Console.WriteLine($"Barista: Café {i} listo.");
        }
    }
}