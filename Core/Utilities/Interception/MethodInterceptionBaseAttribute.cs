using Castle.DynamicProxy;

namespace Core.Utilities.Interception;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public abstract class MethodInterceptionBaseAttribute:Attribute,IInterceptor
{
    public int Priority { get; set; }

    public virtual void Intercept(IInvocation invocation)
    {
        
    }
}