namespace BazarCarioca.WebAPI.Repositories
{
    public interface IUnitOfWork
    {
        IShopkeeperRepository ShopkeeperRepository { get; }
        IStoreRepository StoreRepository { get; }
        IServiceRepository ServiceRepository { get; }
        IProductTypeRepository ProductTypeRepository { get; }
        IProductRepository ProductRepository { get; }

        Task CommitAsync();
    }
}
