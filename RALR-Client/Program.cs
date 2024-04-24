using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

public class TcpClient
{
    static System.Net.Sockets.TcpClient _client;
    public static RSA _rsa;
    public static string line;


    public static void Main()
    {
        _client = new System.Net.Sockets.TcpClient("127.0.0.1", 8080);
        _rsa = RSA.Create();
        var stream = _client.GetStream();
        var reader = new StreamReader(stream);
        var writer = new StreamWriter(stream);

        writer.WriteLine(_rsa.ToXmlString(false));
        writer.Flush();

        line = reader.ReadLine();
        while (line!=null)
        {
            var encryptedMessage = Convert.FromBase64String(line);
            var decryptedMessage = _rsa.Decrypt(encryptedMessage, RSAEncryptionPadding.Pkcs1);

            Console.WriteLine("Message from server: " + Encoding.UTF8.GetString(decryptedMessage) + "\n" + ChangePolicy(Encoding.UTF8.GetString(decryptedMessage), false));
            
            line = reader.ReadLine();
        }
    }
    public static string ChangePolicy(string path, bool allow)
    {
        string script = @$"
        Import-Module AppLocker

        $fileInformation = Get-AppLockerFileInformation -Path '{path}'

        $Policy = New-AppLockerPolicy -RuleType Publisher -User 'S-1-1-0' -FileInformation $fileInformation

        foreach($RuleCollection in $Policy.RuleCollections)
        {{
            foreach($Rule in $RuleCollection)
            {{
                $Rule.Action = '{(allow ? "Allow" : "Deny")}'
            }}
        }}
        Set-AppLockerPolicy -PolicyObject $Policy -Merge

        ";

        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -ExecutionPolicy unrestricted -Command {script}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        Process process = new Process() { StartInfo = startInfo };
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        // Output will contain the result of the script execution
        return output;
    }
}
