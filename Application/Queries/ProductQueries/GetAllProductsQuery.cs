using Application.Services.Contracts.Repositories;
using Application.Services.Contracts.Services.Base;
using Application.Services.Models.Base;
using Application.Services.Models.ProductModels;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.ProductQueries
{
    public record GetAllProductsQuery(SearchBaseModel SearchModel) : IRequest<PaginatedList<ProductForView>> { }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedList<ProductForView>>
    {
        private readonly IMapper _mapper;
        private readonly ILocalizationMessage _localization;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(
            IMapper mapper,
            ILocalizationMessage localization,
            IProductRepository repository,
            ILogger<GetAllProductsQueryHandler> logger)
        {
            _mapper = mapper;
            _localization = localization;
            _productRepository = repository;
            _logger = logger;
        }

        public async Task<PaginatedList<ProductForView>> Handle(GetAllProductsQuery productDto, CancellationToken cancellationToken)
        {
            try
            {
                var products = _productRepository.GetAllProducts(productDto.SearchModel, cancellationToken);
                if (products == null)
                {
                    _logger.LogError("Products list is null.");
                    throw new NullReferenceException("Products list is null.");
                }

                var paginatedCategories = await PaginatedList<Product>.CreateAsync(
                    products,
                    productDto.SearchModel.PageIndex,
                    productDto.SearchModel.PageSize,
                    cancellationToken);

                if (paginatedCategories == null)
                {
                    _logger.LogError("Paginated list is null.");
                    throw new NullReferenceException("Paginated list is null.");
                }

                var items = new PaginatedList<ProductForView>(
                    _mapper.Map<List<ProductForView>>(paginatedCategories.Items),
                    productDto.SearchModel.PageIndex,
                    productDto.SearchModel.PageSize,
                    paginatedCategories.TotalCount);

                if (items == null)
                {
                    _logger.LogError("Items mapping is null.");
                    throw new NullReferenceException("Items mapping is null.");
                }

                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling GetAllProductsQuery.");
                throw;
            }
        }
    }
}
