using Newtonsoft.Json;

namespace OnlineStore.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetJson<T>(this ISession session, string key)
        {
            string? sessionData = session.GetString(key);

            if (sessionData != null)
            {
                return JsonConvert.DeserializeObject<T>(sessionData);
            }
            else
            {
                return default(T);
            }
        }
    }
}
