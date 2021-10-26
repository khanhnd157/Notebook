using AutoMapper;

using CodeMaze.Cryptography.Hash;
using CodeMaze.Data;
using CodeMaze.Models;

using MazeCore.MongoDb.Repository;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMaze.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoRepository<UserEntity> repositoryUser;
        private readonly IMapper mapper;

        public UserService(IMongoRepository<UserEntity> repositoryUser, IMapper mapper)
        {
            this.repositoryUser = repositoryUser;
            this.mapper = mapper;
        }

        public List<UserEntity> GetAll()
        {
            var listusers = this.repositoryUser.Select(x => true);
            return listusers?.ToList();
        }

        public async Task<UserModel> RegisterAsync(UserModel user)
        {
            var _checked = await this.repositoryUser.AnyAsync(u => u.Username.ToLower().Equals(user.Username.ToLower()));
            if (_checked) return user;

            _checked = await this.repositoryUser.AnyAsync(u => u.Email.ToLower().Equals(user.Email.ToLower()));
            if (_checked) return user;

            var entity = mapper.Map<UserEntity>(user);
            var userAdded = this.repositoryUser.AddAsync(entity);

            return mapper.Map<UserModel>(userAdded);

        }

        public async Task<UserModel> SignInAsync(string username, string password)
        {
            UserEntity user;

            if (username.Contains("@"))
                user = await this.repositoryUser.GetFirstOrDefaultAsync(user => user.Email.Equals(username));
            else
                user = await this.repositoryUser.GetFirstOrDefaultAsync(user => user.Username.Equals(username));

            var _checked = HashPassword.VerifyHashed(user?.Password, password);
            if (_checked)
            {
                return mapper.Map<UserModel>(user);
            }

            return null;
        }
    }

    public interface IUserService
    {
        List<UserEntity> GetAll();

        Task<UserModel> RegisterAsync(UserModel user);

        Task<UserModel> SignInAsync(string username, string password);
    }
}
