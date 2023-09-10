using AutoMapper;
using EFCoreMigrations;
using Interface;
using Model.Dto.User;
using Model.Entitys;
using Model.Other;

namespace Service;

public class UserService : IUserService
{
    private readonly IMapper _mapper;

    private MyDbContext _context;

    public UserService(IMapper mapper, MyDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public UserRes GetUser(string userName, string passWord)
    {
        var users = _context.Users.Where(u => u.Name == userName && u.Password == passWord).FirstOrDefault();
        if (users != null)
        {
            return _mapper.Map<UserRes>(users);
        }

        return new UserRes();
    }

    public async Task<string> add(UserAdd userAdd)
    {
        var users = _context.Users.Where(u => u.Name == userAdd.username).FirstOrDefault();
        if (users == null)
        {
            Users user = new Users()
            {
                Name = userAdd.username,
                Password = userAdd.Password,
                CreateDate = DateTime.Now,
                CreateUserId = 0,
                IsDeleted = 0
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return "注册成功";
        }
        else
        {
            return "用户已存在";
        }
    }
}