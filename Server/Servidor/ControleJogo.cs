using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class ControleJogo
{
    const int MAXIMO = 2;

    public Jogador[] jogadores;

    public bool Rodando { get; private set; } = true;

    public ControleJogo()
    {
        jogadores = new Jogador[MAXIMO];
    }

    public void DesconectarCliente(TcpClient cliente)
    {
        NetworkStream stream = cliente.GetStream();
        StreamWriter writer = new StreamWriter(stream);
        Console.WriteLine("Servidor lotado");
        writer.WriteLine("Servidor lotado");
        writer.Flush();
        writer.Close();
    }

    public void DesconectarCliente(int id, TcpClient cliente)
    {
        NetworkStream stream = cliente.GetStream();
        StreamWriter writer = new StreamWriter(stream);
        Console.WriteLine("Servidor lotado");
        writer.WriteLine("Servidor lotado");
        writer.Flush();
        writer.Close();
        jogadores[id] = null;
    }


    public void AdicionaNovoJogador(TcpClient cliente)
    {
        for (int i = 0; i < jogadores.Length; i++)
        {
            if (jogadores[i] == null)
            {
                int id = i;
                //Aqui cria-se um novo jogador com a conexão feito pelo cliente (TcpClient)
                //Esse nova jogador/objeto é quem vai receber a mensagem do cliente.
                //Cada jogador no servidor tem um objeto para si.
                jogadores[id] = new Jogador(this, id, cliente);
                Console.WriteLine($"Novo jogador conectado com o id {id}");
                return;
            }
        }
        DesconectarCliente(cliente);
    }
}
