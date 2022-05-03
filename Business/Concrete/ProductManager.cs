using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class ProductManager : IProductService
{
    private IProductDal _productDal;

    public ProductManager(IProductDal productDal)
    {
        _productDal = productDal;
    }

    public IDataResult<Product> GetById(int productId)
    {
        return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
    }

    public IDataResult<List<Product>> GetList()
    {
        Thread.Sleep(5000);
        return new SuccessDataResult<List<Product>>(_productDal.GetList().ToList());
    }

    public IDataResult<List<Product>> GetListByCategory(int categoryId)
    {
        return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.CategoryId == categoryId).ToList());
    }

    public IResult Add(Product product)
    {
        _productDal.Add(product);
        return new SuccessResult(Messages.ProductAdded);
    }

    private IResult CheckIfProductNameExists(string productName)
    {
        var result = _productDal.GetList(p => p.ProductName == productName).Any();
        if (result)
        {
            return new ErrorResult(Messages.ProductNameAlreadyExists);
        }

        return new SuccessResult();
    }

    
    public IResult Delete(Product product)
    {
        _productDal.Delete(product);
        return new SuccessResult(Messages.ProductDeleted);
    }

    public IResult Update(Product product)
    {
        _productDal.Update(product);
        return new SuccessResult(Messages.ProductUpdated);
    }


    public IResult TransactionalOperation(Product product)
    {
        _productDal.Update(product);
        _productDal.Add(product);
        return new SuccessResult(Messages.ProductUpdated);
    }
}