using Contracts.Repository;
using Entities;
using Service.Contracts;
using Shared;

namespace Service;
internal sealed class CountryService(ICountryRepository _repository) : ICountryService
{
    public async Task<ApiResponse> Add(Country country)
    {
        if (await _repository.Exists(country))
            return ApiResponse.FailResponse($"Country {country.CountryName} already exists");

        await _repository.Add(country);
        return ApiResponse.SuccessResponse($"Country {country.CountryName} added");
    }

    public async Task<ApiResponse> Delete(Country country)
    {
        if (!await _repository.Exists(country))
            return ApiResponse.FailResponse($"Country {country.CountryName} does not exists");

        await _repository.Delete(country);
        return ApiResponse.SuccessResponse($"Country {country.CountryName} deleted");
    }

    public async Task<ApiResponse> Get(int id)
    {
        var country = await _repository.Get(id);

        if (country is null)
            return ApiResponse.FailResponse($"Country with ID:{id} not found");

        return ApiResponse.SuccessResponse(country);
    }

    public async Task<ApiResponse> GetAll()
    {
        var countryList = await _repository.GetAll();
        return ApiResponse.SuccessResponse(countryList);
    }

    public async Task<ApiResponse> Update(Country country)
    {
        if (!await _repository.Exists(country))
            return ApiResponse.FailResponse($"Country {country.CountryName} does not exists");

        await _repository.Update(country);
        return ApiResponse.SuccessResponse($"Country {country.CountryName} updated");
    }
}
