using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Reflection
{
    public class CustomSerializer<T> where T : F, new()
    {
        public static string Serialize(T Tobj)
        {
            var resultStr = new StringBuilder();
            var fields = Tobj.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).ToList();
            foreach (var field in fields)
            { resultStr.Append(field.GetValue(Tobj).ToString() + ";"); }

            return resultStr.ToString();
        }

        public static T Deserialize(string csv)
        {
            var result = new T();
            var resultFields = result.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var scvValuesList = csv.Split(";").Where(o => o != string.Empty).ToArray();
            for (int i = 0; i < scvValuesList.Length; i++)
            {
                if (resultFields.Length <= i)
                    break;

                resultFields[i].SetValue(result, Convert.ChangeType( scvValuesList[i], resultFields[i].FieldType));

            }
            return result;
        }
    }
}
