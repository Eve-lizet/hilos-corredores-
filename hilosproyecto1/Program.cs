using System;
using System.Collections.Generic;
using System.Threading;

class Restaurante
{
    static Queue<string> pedidos = new Queue<string>();
    static object lockObject = new object();
    static bool cerrado = false;

    static void Main()
    {
        // Crear 1 hilo para cliente y 1 para cocinero
        Thread cliente = new Thread(GenerarPedidos);
        Thread cliente2 = new Thread(GenerarPedidos);
        Thread cliente3 = new Thread(GenerarPedidos);
        Thread cocinero = new Thread(PrepararPedidos);
        Thread cocinero2 = new Thread(PrepararPedidos);

        cliente.Start();
        cliente2.Start();
        cliente3.Start();
        cocinero.Start();
        cocinero2.Start();

        // Esperar a que termine el cliente
        cliente.Join();
        cliente2.Join();
        cliente3.Join();


        // Avisar que el restaurante cerró
        lock (lockObject)
        {
            cerrado = true;
        }

        // Esperar a que termine el cocinero
        cocinero.Join();
        cocinero2.Join();

        Console.WriteLine("Restaurante cerrado");
    }

    static void GenerarPedidos()
    {
        string[] menu = { "Pizza", "Hamburguesa", "Ensalada" };

        for (int i = 1; i <= 3; i++) // Solo 3 pedidos
        {
            string pedido = menu[new Random().Next(menu.Length)];

            lock (lockObject)
            {
                pedidos.Enqueue(pedido);
                Console.WriteLine($"Pedido recibido: {pedido}");
            }

            Thread.Sleep(1000); // Espera 1 segundo entre pedidos
        }
    }

    static void PrepararPedidos()
    {
        while (true)
        {
            string pedido = null;

            lock (lockObject)
            {
                if (pedidos.Count > 0)
                {
                    pedido = pedidos.Dequeue();
                }
                else if (cerrado)
                {
                    break; // Salir si no hay pedidos y el restaurante cerró
                }
            }

            if (pedido != null)
            {
                Console.WriteLine($"Preparando: {pedido}");
                Thread.Sleep(2000); // Tiempo de preparación
                Console.WriteLine($"Pedido listo: {pedido}");
            }
            else
            {
                Thread.Sleep(500); // Espera breve si no hay pedidos
            }
        }
    }
}