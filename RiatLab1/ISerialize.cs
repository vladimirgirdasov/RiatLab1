namespace RiatLab1
{
    internal interface ISerialize<T>
    {
        T SerializeFormatEnum { get; set; }

        object Serialize(T mod);

        bool Deserialize(T mod, object data);
    }
}
