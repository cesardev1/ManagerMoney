using AutoMapper;
using ManagerMoney.Models;

namespace ManagerMoney.Services;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Account, AccountCreateViewModel>();
    }
}