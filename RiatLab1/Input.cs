namespace RiatLab1
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [Serializable]
    public class Input : ISerialize<Input.SerializeFormat>
    {
        public enum SerializeFormat
        {
            Json,
            Xml
        }

        public int K { get; set; }
        public decimal[] Sums { get; set; }
        public int[] Muls { get; set; }
        
        public Output TransformToOutput()
        {
            return new Output()
            {
                SumResult = Sums.Sum() * K,
                MulResult = Muls.Aggregate(1, (x, y) => x * y),
                SortedInputs = Sums.Concat(Array.ConvertAll(Muls, x => (decimal)x)).OrderBy(x => x).ToArray()
            };
        }

        #region ISerialize realization

        [XmlIgnore]
        [JsonIgnore]
        public SerializeFormat SerializeFormatEnum { get; set; }

        public object Serialize(SerializeFormat mod)
        {
            switch (mod)
            {
                case SerializeFormat.Json:
                {
                    return JsonConvert.SerializeObject(this);
                }
                case SerializeFormat.Xml:
                {
                    var serializer = new XmlSerializer(typeof(Input));
                    var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    var settings = new XmlWriterSettings
                    {
                        Indent = true,
                        OmitXmlDeclaration = true
                    };

                    using (var stream = new StringWriter())
                    {
                        using (var writer = XmlWriter.Create(stream, settings))
                        {
                            serializer.Serialize(writer, this, emptyNamepsaces);
                            return stream.ToString();
                        }
                    }
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(mod), mod, null);
            }
        }

        public bool Deserialize(SerializeFormat mod, object data)
        {
            switch (mod)
            {
                case SerializeFormat.Json:
                    {
                        if (!(data is string))
                        {
                            return false;
                        }

                        var ans = true;

                        try
                        {
                            var buf = JsonConvert.DeserializeObject<Input>((string)data);

                            K = buf.K;
                            Sums = buf.Sums;
                            Muls = buf.Muls;
                        }
                        catch (Exception)
                        {
                            ans = false;
                        }
                        return ans;
                    }

                case SerializeFormat.Xml:
                    {
                        if (!(data is string))
                            return false;

                        var ans = true;

                        try
                        {
                            using (var fs = new StringReader((string)data))
                            {
                                var serializer = new XmlSerializer(typeof(Input));
                                var buf = (Input)serializer.Deserialize(fs);

                                K = buf.K;
                                Muls = buf.Muls;
                                Sums = buf.Sums;
                            }
                        }
                        catch (Exception)
                        {
                            ans = false;
                        }

                        return ans;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(mod), mod, null);
            }
        }

        #endregion

    }
}
