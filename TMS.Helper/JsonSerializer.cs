using Newtonsoft.Json;
using System;

namespace TMS.Helper
{
    //用于对象与JSon的转换
    public static class JsonSerializer
    {
       
        public static string ToJSON(this object o)
        {
            if (o == null)           
            {
                return null;
            }
            return JsonConvert.SerializeObject(o);         }

        public static T FromJSON<T>(this string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception ex)
            {
                 return default(T);
            }
        }
    }
}
