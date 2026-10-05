using Jobee.Workplace.Jobs.RestApi.Categories;
using Jobee.Workplace.Jobs.RestApi.Companies;
using Jobee.Workplace.Jobs.RestApi.Offers;

namespace Jobee.Workplace.Jobs.RestApi;

public static class JobsEndpoints
{
    public static void MapJobsEndpoints(this WebApplication app)
    {
        app.MapCompanyEndpoints();
        app.MapCategoryEndpoints();
        app.MapOfferEndpoints();
    }
}
