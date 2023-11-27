namespace Backend.Features.Suppliers
{
    public class SuppliersListQuery : IRequest<List<SuppliersListQueryResponse>>
    {
        public string? Name { get; set; }
    }
}
