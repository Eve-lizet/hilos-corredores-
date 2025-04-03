using System;
using System.Collections.Generic;
using System.Threading;

class Restaurante
{
    static Queue<string> pedidos = new Queue<string>(); // Cola de pedidos
    static object lockObject = new object(); // Para sincronización
    static SemaphoreSlim semaforoCocineros = new SemaphoreSlim(2); // Máximo 2 cocineros trabajando
    static Random random = new Random();

    static void Main()
    {
        Thread[] clientes = new Thread[5];
        Thread[] cocineros = new Thread[3];
        Thread cajero = new Thread(RegistrarPedidos);

        // Iniciar clientes (generan pedidos)
        for (int i = 0; i < clientes.Length; i++)
        {
            clientes[i] = new Thread(GenerarPedido);
            clientes[i].Start($"Cliente {i + 1}");
        }

        // Iniciar cocineros (procesan pedidos)
        for (int i = 0; i < cocineros.Length; i++)
        {
            cocineros[i] = new Thread(PrepararPedido);
            cocineros[i].Start($"Cocinero {i + 1}");
        }

        // Iniciar cajero (registra pedidos completados)
        cajero.Start();

        // Esperar a que terminen los clientes y cocineros
        foreach (Thread cliente in clientes) cliente.Join();
        foreach (Thread cocinero in cocineros) cocinero.Join();

        // Finalizar cajero
        lock (lockObject)
        {
            pedidos.Enqueue("FIN"); // Señal para que el cajero termine
        }
        cajero.Join();

        Console.WriteLine("El restaurante ha cerrado.");
    }

    // Cliente: Genera un pedido y lo agrega a la cola
    static void GenerarPedido(object cliente)
    {
        string[] menu = { "Pizza", "Hamburguesa", "Sushi", "Pasta", "Ensalada" };
        string pedido = menu[random.Next(menu.Length)];

        lock (lockObject)
        {
            pedidos.Enqueue($"{cliente} ordenó {pedido}");
            Console.WriteLine($"{cliente} hizo un pedido de {pedido}");
        }
        Thread.Sleep(random.Next(500, 1500)); // Simula tiempo entre pedidos
    }

    // Cocinero: Procesa pedidos de la cola
    static void PrepararPedido(object cocinero)
    {
        while (true)
        {
            string pedido;

            lock (lockObject)
            {
                if (pedidos.Count == 0) return; // No hay pedidos, el cocinero termina
                pedido = pedidos.Dequeue();
            }

            semaforoCocineros.Wait(); // Controla el número de cocineros activos
            Console.WriteLine($"{cocinero} está preparando {pedido}...");
            Thread.Sleep(random.Next(1000, 3000)); // Simula el tiempo de preparación
            Console.WriteLine($"{cocinero} ha terminado {pedido}");
            semaforoCocineros.Release();
        }
    }

    // Cajero: Registra pedidos completados
    static void RegistrarPedidos()
    {
        while (true)
        {
            lock (lockObject)
            {
                if (pedidos.Count > 0)
                {
                    string pedido = pedidos.Peek();
                    if (pedido == "FIN") break; // Señal de fin
                }
            }
            Thread.Sleep(1000); // Simula tiempo de registro
        }
        Console.WriteLine("Cajero ha terminado de registrar los pedidos.");
    }
}
