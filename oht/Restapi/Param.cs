namespace oht.Restapi
{
    enum ParamType
    {        
        Object = 1,
        FileContent,
        FilePath
    }

    /// <summary>
    /// Used internally to pass extra parameters when doing REST API call
    /// Can be of several types: object (anything passed as is), file content - form upload is used, file path - file content is pulled and then form uploaded.
    /// </summary>
    internal class Param
    {
        public Param()
        {
            Type = ParamType.Object;
        }

        public Param(string name, object value)
        {
            Type = ParamType.Object;
            Name = name;
            Value = value;
        }

        public Param(string name, object value, ParamType type)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public ParamType Type { get; set; }
        public string Name { get; set; }
        public  object Value { get; set; }
    }
}
