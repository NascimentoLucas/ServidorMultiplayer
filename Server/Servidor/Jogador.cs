using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;


public class Jogador
{
    private ControleJogo ControleJogo;
    private TcpClient cliente;

    private StreamReader reader = null;
    private StreamWriter writer = null;

    private Thread thread;

    public string loginUser;
    public string dados;

    public int id { get; private set; } = -1;

    public Jogador(ControleJogo controleJogo, int id, TcpClient cliente)
    {
        this.id = id;
        this.ControleJogo = controleJogo;
        this.cliente = cliente;


        NetworkStream stream = this.cliente.GetStream();
        //Objeto responsável em ler as mensagens
        reader = new StreamReader(stream);
        //Objeto responsável em enviar mensagens
        writer = new StreamWriter(stream);

        //Começa uma thread para esse jogador,
        //ou seja, o código executado aqui não trava o programa.
        thread = new Thread(Run);
        thread.Start();
    }

    public void EnviaMenssagem(string dados)
    {
        //Console.WriteLine($"{dados}");
        writer.WriteLine(dados);
        writer.Flush();
    }

    public void Run()
    {
        Console.WriteLine($"Start id:{id}.listener");
        //Envia uma mensagem com o valor da variável "id",Ex.:
        //jogador:0
        EnviaMenssagem($"jogador:{id}");
        //Espera uma mensagem chegar do cliente,
        //Essa linha pararia o programa,
        //porém como está em uma thread separada não trava
        string dados = reader.ReadLine();
        while (dados != null)
        {
            try
            {
                //Console.WriteLine($"Recebido {id} | Mensagem: {dados}");

                //Toda menssagem recebida é enviada a todos os jogadores no servidor
                //Menos ao jogador que enviou a mensagem.
                for (int i = 0; i < ControleJogo.jogadores.Length; i++)
                {
                    if (i != id && ControleJogo.jogadores[i] != null)
                        ControleJogo.jogadores[i].EnviaMenssagem(dados);
                }

                //mesmo processo a cima
                dados = reader.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro de rede: {e}");
                dados = null;
            }
        }
        cliente.Close();
    }
}
