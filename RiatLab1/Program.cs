namespace RiatLab1
{
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputKeys = new Dictionary<string, Input.SerializeFormat>
            {
                { "Json", Input.SerializeFormat.Json },
                { "Xml", Input.SerializeFormat.Xml }
            };
            var outputKeys = new Dictionary<string, Output.SerializeFormat>
            {
                { "Json", Output.SerializeFormat.Json },
                { "Xml", Output.SerializeFormat.Xml }
            };

            var format = Console.ReadLine();
            var data = Console.ReadLine();

            var input = new Input();
            input.Deserialize(inputKeys[format], data);

            var output = input.TransformToOutput();

            Console.WriteLine(((string)output.Serialize(outputKeys[format])).RemoveEndLines().Replace(" ", ""));
        }
    }
}
