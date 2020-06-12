﻿using Microsoft.EntityFrameworkCore;
using server.Data;
using server.enums;
using server.Helper;
using server.Helper.product;
using server.Interfaces;
using server.Models;
using server.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopDbContext _context;
        public ProductService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request)
        {
            // query
            var query = _context.products.AsQueryable();
            //filter
            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => FormatVietnamese.convertToUnSign(x.name.ToLower())
                .Contains(FormatVietnamese.convertToUnSign(request.keyword.ToLower())));
            }
            if (request.categoryId.HasValue && request.categoryId.Value > 0)
            {
                query = query.Where(x => x.categoryId == request.categoryId.Value);
            }
            //pagination
            int totalRow = await query.CountAsync();
            var data = await query.Include(eva => eva.Evaluations.Where(e => e.status == EvaluationStatus.Confirm))
                .Include(img => img.Images.Where(i => i.status == ActionStatus.Display))
                .OrderBy(x => x.categoryId).Skip((request.pageIndex - 1) * request.pageSize)
                .Take(request.pageSize).Select(rs => new ProductViewModel
                {
                    id = rs.id,
                    name = rs.name,
                    price = rs.price,
                    importPrice = rs.importPrice,
                    sale = rs.sale,
                    categoryId = rs.categoryId,
                    category = rs.category,
                    color = rs.color,
                    size = rs.size,
                    description = rs.description,
                    Evaluations = rs.Evaluations,
                    Images = rs.Images,
                    rating = Convert.ToInt32(rs.Evaluations.Average(ave => ave.rating)),
                    provider = rs.provider,
                    providerId = rs.providerId,
                    status = rs.status

                }).ToListAsync();
            //
            var pagedResult = new PagedResult<ProductViewModel>
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pagedResult;
        }

        public async Task<List<ProductViewModel>> GetAllProduct(int itemCount = 8)
        {
            var data = await _context.products.Where(x => x.status == ActionStatus.Display)
                .Select(rs => new ProductViewModel
                {
                    id = rs.id,
                    name = rs.name,
                    price = rs.price,
                    importPrice = rs.importPrice,
                    sale = rs.sale,
                    categoryId = rs.categoryId,
                    category = rs.category,
                    color = rs.color,
                    size = rs.size,
                    amount = rs.amount,
                    viewCount = rs.viewCount,
                    description = rs.description,
                    Evaluations = rs.Evaluations,
                    Images = rs.Images.Where(p => p.status == ActionStatus.Display).ToList(),
                    rating = Convert.ToInt32(rs.Evaluations.Average(ave => ave.rating)),
                    provider = rs.provider,
                    providerId = rs.providerId,
                    status = rs.status

                }).Take(itemCount).ToListAsync();
            return data;
        }

        public async Task<ProductViewModel> getProductById(int productId)
        {
            var temp = await _context.products.Include(eva => eva.Evaluations)
                .Include(img => img.Images)
                .Select(ele => new Product()
                {
                    id = ele.id,
                    name = ele.name,
                    category = ele.category,
                    categoryId = ele.categoryId,
                    color = ele.color,
                    description = ele.description,
                    Evaluations = ele.Evaluations.Where(e => e.status == EvaluationStatus.Confirm).ToList(),
                    Images = ele.Images.Where(i => i.status == ActionStatus.Display).ToList(),
                    importPrice = ele.importPrice,
                    price = ele.price,
                    provider = ele.provider,
                    providerId = ele.providerId,
                    //
                    rating = Convert.ToInt32(ele.Evaluations.Where(e => e.status == EvaluationStatus.Confirm).Average(ave => ave.rating)),
                    sale = ele.sale,
                    size = ele.size,
                    status = ele.status,
                    amount = ele.amount,
                    viewCount = ele.viewCount,
                })
                .FirstOrDefaultAsync(x => x.id == productId);
            temp.viewCount = temp.viewCount + 1;
            _context.Entry(temp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //
            var product = await _context.products.Include(eva => eva.Evaluations)
                .Include(img => img.Images)
                .Select(ele => new ProductViewModel()
                {
                    id = ele.id,
                    name = ele.name,
                    category = ele.category,
                    categoryId = ele.categoryId,
                    color = ele.color,
                    description = ele.description,
                    Evaluations = ele.Evaluations.Where(e => e.status == EvaluationStatus.Confirm).ToList(),
                    Images = ele.Images.Where(i => i.status == ActionStatus.Display).ToList(),
                    importPrice = ele.importPrice,
                    price = ele.price,
                    provider = ele.provider,
                    providerId = ele.providerId,
                    rating = ele.rating,
                    sale = ele.sale,
                    size = ele.size,
                    status = ele.status,
                    amount = ele.amount,
                    viewCount = ele.viewCount,
                })
                .FirstOrDefaultAsync(x => x.id == productId);
            
            
            return product;
        }

        public async Task<List<ProductViewModel>> GetTopViewCountProduct(bool all = false)
        {
            if(all == false)
            {
                var data = await _context.products.Where(x => x.status == ActionStatus.Display)
                    .OrderByDescending(x => x.viewCount).Take(5)
                .Select(rs => new ProductViewModel
                {
                    id = rs.id,
                    name = rs.name,
                    price = rs.price,
                    importPrice = rs.importPrice,
                    sale = rs.sale,
                    categoryId = rs.categoryId,
                    category = rs.category,
                    color = rs.color,
                    size = rs.size,
                    amount = rs.amount,
                    viewCount = rs.viewCount,
                    description = rs.description,
                    Evaluations = rs.Evaluations,
                    Images = rs.Images.Where(p => p.status == ActionStatus.Display).ToList(),
                    rating = Convert.ToInt32(rs.Evaluations.Average(ave => ave.rating)),
                    provider = rs.provider,
                    providerId = rs.providerId,
                    status = rs.status

                }).ToListAsync();
                return data;
            }
            else
            {
                var data = await _context.products.Where(x => x.status == ActionStatus.Display)
                    .OrderByDescending(x => x.viewCount).Take(16)
                .Select(rs => new ProductViewModel
                {
                    id = rs.id,
                    name = rs.name,
                    price = rs.price,
                    importPrice = rs.importPrice,
                    sale = rs.sale,
                    categoryId = rs.categoryId,
                    category = rs.category,
                    color = rs.color,
                    size = rs.size,
                    amount = rs.amount,
                    viewCount = rs.viewCount,
                    description = rs.description,
                    Evaluations = rs.Evaluations,
                    Images = rs.Images.Where(p => p.status == ActionStatus.Display).ToList(),
                    rating = Convert.ToInt32(rs.Evaluations.Average(ave => ave.rating)),
                    provider = rs.provider,
                    providerId = rs.providerId,
                    status = rs.status
                }).ToListAsync();
                return data;
            }
        }
    }
}
