using Jobee.Workplace.Jobs.Contracts.Companies.Queries;
using Jobee.Workplace.Jobs.Contracts.Companies.Shared;
using Jobee.Workplace.Jobs.Contracts.Shared;
using Jobee.Workplace.Jobs.Domain.Companies;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Jobs.Infrastructure.Companies.QueryHandlers;

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, CompanyModel>
{
    private readonly DbContext _dbContext;

    public GetCompanyQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CompanyModel> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        var company = await _dbContext.Set<Company>()
            .Where(c => c.Id == request.CompanyId)
            .Select(c => new CompanyModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Address = new AddressModel
                {
                    City = c.Address.City,
                    Street = c.Address.Street,
                    PostalCode = c.Address.PostalCode,
                    Country = c.Address.Country,
                    FirstLine = c.Address.FirstLine,
                    SecondLine = c.Address.SecondLine
                }
            })
            .FirstAsync(cancellationToken);

        return company;
    }
}

