using Microsoft.EntityFrameworkCore.Internal;

namespace Backend.Features.Customers;

internal class CustomersListQueryHandler : IRequestHandler<CustomersListQuery, List<CustomersListQueryResponse>>
{
    private readonly BackendContext context;

    public CustomersListQueryHandler(BackendContext context)
    {
        this.context = context;
    }

    public async Task<List<CustomersListQueryResponse>> Handle(CustomersListQuery request, CancellationToken cancellationToken)
    {
        var query =
            from cust in context.Customers
            join cat in context.CustomerCategories on cust.CustomerCategoryId equals cat.Id into catGroup
            from custCat in catGroup.DefaultIfEmpty()
            where (string.IsNullOrEmpty(request.Name) || cust.Name.ToLower().Contains(request.Name.ToLower()))
            && (string.IsNullOrEmpty(request.Email) || cust.Email.ToLower().Contains(request.Email.ToLower()))
            select new CustomersListQueryResponse
            {
                Id = cust.Id,
                Name = cust.Name,
                Address = cust.Address,
                Email = cust.Email,
                Phone = cust.Phone,
                Iban = cust.Iban,
                Category = new CustomersListQueryResponseCategory { Code = (custCat.Code ?? ""), Description = (custCat.Description ?? "") }
            };

        return await query.ToListAsync(cancellationToken);
    }
}