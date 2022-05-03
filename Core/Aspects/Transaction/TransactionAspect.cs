using System.Formats.Asn1;
using System.Transactions;
using Castle.DynamicProxy;
using Core.Utilities.Interception;

namespace Core.Aspects.Transaction;

public class TransactionScopeAspect : MethodInterception
{
    public override void Intercept(IInvocation invocation)
    {
        using (TransactionScope transactionScope = new TransactionScope())
        {
            try
            {
                invocation.Proceed();
                transactionScope.Complete();
            }

            catch (Exception e)
            {
                transactionScope.Dispose();
                throw;
            }
        }
    }
}

