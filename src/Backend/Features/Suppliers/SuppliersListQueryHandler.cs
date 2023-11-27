using MediatR;

namespace Backend.Features.Suppliers;

internal class SuppliersListQueryHandler : IRequestHandler<SuppliersListQuery, List<SuppliersListQueryResponse>>
{
    private readonly BackendContext context;

    public SuppliersListQueryHandler(BackendContext context)
    {
        this.context = context;
    }

    public async Task<List<SuppliersListQueryResponse>> Handle(SuppliersListQuery request, CancellationToken cancellationToken)
    {
        var query = context.Suppliers.AsQueryable();
        if (!string.IsNullOrEmpty(request.Name))
            query = query.Where(q => q.Name.ToLower().Contains(request.Name.ToLower()));

        var data = await query.OrderBy(q => q.Name).ToListAsync(cancellationToken);
        var result = new List<SuppliersListQueryResponse>();

        foreach (var item in data)
            result.Add(new SuppliersListQueryResponse
            {
                Id = item.Id,
                Name = item.Name,
                Address = item.Address,
                Email = item.Email,
                Phone = item.Phone,
            });

        return result;
    }
}