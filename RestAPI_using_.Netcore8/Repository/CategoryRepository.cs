﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI_using_.Netcore8.Data;
using RestAPI_using_.Netcore8.Models;
using RestAPI_using_.Netcore8.Repository.IRepository;
namespace RestAPI_using_.Netcore8.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private string CategoryCacheKey = "CategoryCacheKey";
        private int CacheExpirationTime = 3600;
        public CategoryRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<bool> CategoryExistsAsync(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name);
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            return await Save();
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            category.CreatedDate = DateTime.Now;
            _context.Update(category);
            return await Save();
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            return await Save();
        }

        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            if (_cache.TryGetValue(CategoryCacheKey, out ICollection<Category> categoriesCached))
                return categoriesCached;

            var categoriesFromDb = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(CategoryCacheKey, categoriesFromDb, cacheEntryOptions);
            return categoriesFromDb;
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            if (_cache.TryGetValue(CategoryCacheKey, out ICollection<Category> categoriesCached))
            {
                var category = categoriesCached.FirstOrDefault(c => c.Id == id);
                if (category != null)
                    return category;
            }

            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<bool> Save()
        {
            var result = _context.SaveChanges() >= 0 ? true : false;


            if (result)
            {
                ClearCategoryCache();
            }

            return result;
        }


        public void ClearCategoryCache()
        {
            _cache.Remove(CategoryCacheKey);
        }
    }
}
