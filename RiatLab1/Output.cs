namespace RiatLab1
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [Serializable]
    public class Output : ISerialize<Output.SerializeFormat>
    {
        public enum SerializeFormat
        {
            Json,
            Xml
        }

        public decimal SumResult { get; set; }
        public int MulResult { get; set; }
        public decimal[] SortedInputs { get; set; }

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
                    var serializer = new XmlSerializer(typeof(Output));
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
                            var buf = JsonConvert.DeserializeObject<Output>((string)data);

                            SumResult = buf.SumResult;
                            MulResult = buf.MulResult;
                            SortedInputs = buf.SortedInputs;
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
                                var serializer = new XmlSerializer(typeof(Output));
                                var buf = (Output)serializer.Deserialize(fs);

                                SumResult = buf.SumResult;
                                MulResult = buf.MulResult;
                                SortedInputs = buf.SortedInputs;
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
