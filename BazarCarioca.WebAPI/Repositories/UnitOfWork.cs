using AutoMapper;
using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Services;

namespace BazarCarioca.WebAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext DataBase;
        public IWebService WebService;
        public IMapper Mapper;

        public UnitOfWork(AppDbContext _DataBase, IWebService _WebService, IMapper _Mapper)
        {
            DataBase = _DataBase;
            WebService = _WebService;
            Mapper = _Mapper;
        }

        private IShopkeeperRepository ShopkeeperRepo;

        private IStoreRepository StoreRepo;

        private IServiceRepository ServiceRepo;

        private IProductTypeRepository ProductTypeRepo;

        private IProductRepository ProductRepo;

        public IShopkeeperRepository ShopkeeperRepository
        {
            get
            {
                return ShopkeeperRepo = ShopkeeperRepo ?? new ShopkeeperRepository(DataBase);
            }
        }

        public IStoreRepository StoreRepository
        {
            get
            {
                return StoreRepo = StoreRepo ?? new StoreRepository(DataBase, WebService, Mapper);
            }
        }

        public IServiceRepository ServiceRepository
        {
            get
            {
                return ServiceRepo = ServiceRepo ?? new ServiceRepository(DataBase);
            }
        }

        public IProductTypeRepository ProductTypeRepository
        {
            get
            {
                return ProductTypeRepo = ProductTypeRepo ?? new ProductTypeRepository(DataBase);
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return ProductRepo = ProductRepo ?? new ProductRepository(DataBase, WebService, Mapper);
            }
        }

        public async Task CommitAsync()
        {
            await DataBase.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await DataBase.DisposeAsync();
        }
    }
}
