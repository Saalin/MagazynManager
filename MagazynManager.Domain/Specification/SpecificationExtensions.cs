using Dapper;

namespace MagazynManager.Domain.Specification
{
    public static class SpecificationExtensions
    {
        public static DynamicParameters GetDynamicParametersCollection<T>(this Specification<T> specification)
        {
            var parameters = new DynamicParameters();
            foreach (var dynamicParamsFunc in specification.GetDynamicParameters())
            {
                dynamicParamsFunc(parameters);
            }

            return parameters;
        }
    }
}