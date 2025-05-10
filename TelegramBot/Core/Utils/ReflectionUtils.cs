namespace TelegramBot.Core.Utils
{
    static class ReflectionUtils
    {
        public static IEnumerable<Type> GetAllAssignableTypes(Type interfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => interfaceType.IsAssignableFrom(type)
                               && type.IsClass
                               && !type.IsAbstract);
        }

    }
}
