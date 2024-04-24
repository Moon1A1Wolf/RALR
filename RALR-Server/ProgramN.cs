using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;


public class TcpServer {

    public static void fMain(string[] args)
    {
        Start();
    }
    public static TcpListener _server;
    public static RSA _rsa;
    public static string message;


    public static void Start()
    {
        _server = new TcpListener(IPAddress.Any, 8080);
        _rsa = RSA.Create();
        _server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            var client = _server.AcceptTcpClient();
            var stream = client.GetStream();

            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);

            var clientPublicKeyXml = reader.ReadLine();
            var rsaClient = RSA.Create();
            rsaClient.FromXmlString(clientPublicKeyXml);
            while (true)
            {
                Console.WriteLine("GG");
                message = Console.ReadLine();
                var encryptedMessage = rsaClient.Encrypt(Encoding.UTF8.GetBytes(message), RSAEncryptionPadding.Pkcs1);
                writer.WriteLine(Convert.ToBase64String(encryptedMessage));
                writer.Flush();
            }
        }
    }
}
