using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.Business.Types;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Enums;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class UserManager : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtector _dataProtector;

        public UserManager(IRepository<UserEntity> userRepository, IDataProtectionProvider dataProtectionProvider)
        {
            _userRepository = userRepository;
            _dataProtector = dataProtectionProvider.CreateProtector("security");
        }

        public ServiceMessage AddUser(UserAddDto userAddDto)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == userAddDto.Email.ToLower()).ToList();

            if(hasMail.Any())
            {
                return new ServiceMessage()
                {
                    IsSucceed = false,
                    Message = "Bu eposta adresi zaten alınmış."
                };
                
            }

            var entity = new UserEntity()
            {
                Email = userAddDto.Email,
                FirstName = userAddDto.FirstName,
                LastName = userAddDto.LastName,
                Password = _dataProtector.Protect(userAddDto.Password),
                UserType = UserTypeEnum.User
            };

            _userRepository.Add(entity);

            return new ServiceMessage()
            {
                IsSucceed = true,
                Message = "Kayıt başarılı."
            };


        }

        public UserInfoDto LoginUser(UserLoginDto userLoginDto)
        {
            var userEntity = _userRepository.Get(x => x.Email ==  userLoginDto.Email);

            if(userEntity is null)
            {
                return null;
            }

            var rawPass = _dataProtector.Unprotect(userEntity.Password);

            if(rawPass == userLoginDto.Password)
            {
                return new UserInfoDto()
                {
                    Id = userEntity.Id,
                    Email = userEntity.Email,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    UserType = userEntity.UserType

                };
            }
            else
            {
                return null;
            }


        }
    }
}
