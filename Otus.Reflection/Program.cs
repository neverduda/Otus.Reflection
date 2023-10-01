using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Otus.Reflection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomTest(1000);
            JsonTest(1000);

            Console.ReadLine();
        }


        private static void JsonTest(int iterationCount)
        {
            Console.WriteLine();
            Console.WriteLine($"механизм = JsonSerializer");

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateFieldContractResolver()
            };

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var serializeResult = string.Empty;
            var f = new F().Get();

            for (int i = 0; i <= iterationCount; i++)
            {
                serializeResult = JsonConvert.SerializeObject(f, typeof(F), settings).ToString();
                // Console.WriteLine($"No:{i}; Value: {serializeResult}");
            }
            stopwatch.Stop();
            long serializeTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Время на сериализацию = {serializeTime} мс");

            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i <= iterationCount; i++)
            {
                var deserializeResult = JsonConvert.DeserializeObject(serializeResult, typeof(F), settings);
            }
            stopwatch.Stop();
            long deserializeTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Время на десериализацию = {deserializeTime} мс");
        }

        private static void CustomTest(int iterationCount)
        {
            Console.WriteLine();
            Console.WriteLine($"механизм = CustomSerializer");
            var f = new F().Get();
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var serializeResult = string.Empty;
            for (int i = 0; i <= iterationCount; i++)
            {
                serializeResult = CustomSerializer<F>.Serialize(f);
                // Console.WriteLine($"No:{i}; Value: {serializeResult}");
            }
            stopwatch.Stop();
            long serializeTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Время на сериализацию = {serializeTime} мс");

            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i <= iterationCount; i++)
            {
                var deserializeResult = CustomSerializer<F>.Deserialize(serializeResult);
            }
            stopwatch.Stop();
            long deserializeTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Время на дисериализацию = {deserializeTime} мс");
        }

        class PrivateFieldContractResolver : DefaultContractResolver
        {
            protected override List<MemberInfo> GetSerializableMembers(Type objectType)
            {
                var members = base.GetSerializableMembers(objectType);
                members.AddRange(objectType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic));
                return members;
            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                return base.CreateProperties(type, MemberSerialization.Fields);
            }
        }
    }
}