using Newtonsoft.Json;

namespace ScriptingSample.Lib
{
    public class TestClass
    {
        public string ToJson(object obj)
            => JsonConvert.SerializeObject(obj);
    }
}
