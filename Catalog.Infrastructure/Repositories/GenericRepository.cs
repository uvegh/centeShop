using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Library.Interface;
using Shared.Library.Model.Pagination;

namespace Catalog.Infrastructure.Repositories
{
    public  class GenericRepository<T>:IGenericRepository<T> where T:class
    {
        private readonly CatalogDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IMapper _mapper;

        public GenericRepository(CatalogDbContext context, DbSet<T> dbSet,IMapper mapper)
        {
            _dbSet = context.Set<T>();
            _context = context;

            _mapper = mapper;
        }

        public async Task<TRes?> CreateAsync<TSource, TRes>(TSource entity)
        {
            if (entity != null)
            {
                var obj = _mapper.Map<T>(entity);

                await _dbSet.AddAsync(obj);
                await _context.SaveChangesAsync();
                var res = _mapper.Map<TRes>(obj);
                if (res != null)
                {
                    return res;
                }
                return default;
            }
            return default;   
            
           
        }

        public  async Task<bool> DeleteAsync(int id)
        {
            if (id != null)
            {
             var obj=   await _dbSet.FindAsync(id);
                if (obj!=null)
                {
                    _context.Remove(obj);
                    await _context.SaveChangesAsync();

                }


            }
            return false;
          
        }

        public async Task<bool> Exists(int id)
        {

         var obj=   await _dbSet.FindAsync(id);
            if (obj != null)
            {
                return true;
            }
            return false;
        }

        public async Task<PagedResult<TRes>> GetAllAsync<TRes>(QueryParameters queryParameters)
        {
            var totalCount = await _dbSet.CountAsync();
            var totalItems = await _dbSet.AsNoTracking().Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize).ProjectTo<TRes>(_mapper.ConfigurationProvider).ToListAsync();
        

            return new PagedResult<TRes>
            {
                CurrentPage = queryParameters.PageNumber,
                TotalCount = totalCount,
                RecordNumber = totalItems.Count(),
                Items = totalItems
            };
            
        }

        public Task<T?> GetByIdAsync<TRes>(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TRes> UpdateAsync<TSource, TRes>(int id, TSource entity)
            where TSource : class
            where TRes : class
        {
            throw new NotImplementedException();
        }
    }
}
