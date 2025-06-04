namespace Services
{


    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class IgnoreApiResultFilterAttribute : Attribute
    {
    }

}

