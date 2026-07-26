using Jobee.Workplace.Jobs.Contracts.Offers.Shared;
using Jobee.Workplace.Jobs.Contracts.Shared;
using Jobee.Workplace.Jobs.Domain.Offers;
using Jobee.Workplace.Jobs.Domain.Shared;

namespace Jobee.Workplace.Jobs.Application.Offers;

public static class Extensions
{
    public static WorkType ToWorkType(this WorkTypeModel workType) => workType switch
    {
        WorkTypeModel.Hybrid => WorkType.Hybrid,
        WorkTypeModel.Office => WorkType.Office,
        WorkTypeModel.Remote => WorkType.Remote,
        _ => throw new ArgumentOutOfRangeException(nameof(workType), workType, null)
    };

    public static WorkTypeModel ToWorkTypeModel(this WorkType workType) => workType switch
    {
        WorkType.Hybrid => WorkTypeModel.Hybrid,
        WorkType.Office => WorkTypeModel.Office,
        WorkType.Remote => WorkTypeModel.Remote,
        _ => throw new ArgumentOutOfRangeException(nameof(workType), workType, null)
    };

    public static ContractType ToContractType(this ContractTypeModel contractType) => contractType switch
    {
        ContractTypeModel.Employment => ContractType.Employment,
        ContractTypeModel.B2B => ContractType.B2B,
        _ => throw new ArgumentOutOfRangeException(nameof(contractType), contractType, null)
    };

    public static ContractTypeModel ToContractTypeModel(this ContractType contractType) => contractType switch
    {
        ContractType.Employment => ContractTypeModel.Employment,
        ContractType.B2B => ContractTypeModel.B2B,
        _ => throw new ArgumentOutOfRangeException(nameof(contractType), contractType, null)
    };

    public static Address ToEntity(this AddressModel model) => new()
    {
        City = model.City,
        Street = model.Street,
        PostalCode = model.PostalCode,
        Country = model.Country,
        FirstLine = model.FirstLine,
        SecondLine = model.SecondLine
    };

    public static AddressModel ToAddressModel(this Address address) => new()
    {
        City = address.City,
        Street = address.Street,
        PostalCode = address.PostalCode,
        Country = address.Country,
        FirstLine = address.FirstLine,
        SecondLine = address.SecondLine
    };

    public static KnowledgeLevel ToKnowledgeLevel(this KnowledgeLevelModel level) => level switch
    {
        KnowledgeLevelModel.NiceToHave => KnowledgeLevel.NiceToHave,
        KnowledgeLevelModel.Basic => KnowledgeLevel.Basic,
        KnowledgeLevelModel.Intermediate => KnowledgeLevel.Intermediate,
        KnowledgeLevelModel.Advanced => KnowledgeLevel.Advanced,
        KnowledgeLevelModel.Expert => KnowledgeLevel.Expert,
        _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
    };

    public static KnowledgeLevelModel ToKnowledgeLevelModel(this KnowledgeLevel level) => level switch
    {
        KnowledgeLevel.NiceToHave => KnowledgeLevelModel.NiceToHave,
        KnowledgeLevel.Basic => KnowledgeLevelModel.Basic,
        KnowledgeLevel.Intermediate => KnowledgeLevelModel.Intermediate,
        KnowledgeLevel.Advanced => KnowledgeLevelModel.Advanced,
        KnowledgeLevel.Expert => KnowledgeLevelModel.Expert,
        _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
    };

    public static Requirement ToRequirement(this OfferRequirementModel model, Guid offerId) =>
        new(offerId, model.Name, model.Level.ToKnowledgeLevel());

    public static OfferRequirementModel ToRequirementModel(this Requirement requirement) => new()
    {
        Name = requirement.Name,
        Level = requirement.Level.ToKnowledgeLevelModel()
    };

    public static Location ToLocation(this OfferLocationModel model, Guid offerId) =>
        new(offerId, model.Name, model.Address?.ToEntity());

    public static OfferLocationModel ToLocationModel(this Location location) => new()
    {
        Name = location.Name,
        Address = location.Address?.ToAddressModel()
    };
}