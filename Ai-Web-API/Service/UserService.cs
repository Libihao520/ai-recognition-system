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
        var users = _context.users.Where(u => u.Name == userName && u.Password == passWord).FirstOrDefault();
        if (users != null)
        {
            return _mapper.Map<UserRes>(users);
        }

        return new UserRes();
    }

    public async Task<string> add(UserAdd userAdd)
    {
        users user = new users()
        {
            Name = userAdd.username,
            Password = userAdd.Password,
            CreateDate = DateTime.Now,
            CreateUserId = 0,
            IsDeleted = 0
        };
        _context.users.Add(user);
        _context.SaveChanges();
        return "注册成功！";
    }
}