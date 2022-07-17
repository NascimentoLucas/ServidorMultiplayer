using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace Server
{
    class Program
    {
        //função que inicia o programa
        static void Main(string[] args)
        {
            int porta = 5000;
            //Inicia a classe que gerencia os jogadores
            ControleJogo controle = new ControleJogo();

            try
            {
                IPAddress enderecoServidor = IPAddress.Parse("127.0.0.1");
                //"Inicia" o servidor, aqui o programa espera na porta escolhida 
                //novas conexões
                TcpListener listener = new TcpListener(enderecoServidor, porta);
                listener.Start();

                while (controle.Rodando)
                {
                    Console.WriteLine("Aguardando conexões");
                    //Nessa linha o programa para até que uma nova conexão ocorra
                    TcpClient cliente = listener.AcceptTcpClient();
                    controle.AdicionaNovoJogador(cliente);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine($"Erro de rede: {se}");
            }
        }
    }
}
