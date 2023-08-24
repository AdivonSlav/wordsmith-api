using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.MessageObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.DataAccess.Services;

public class UserService : WriteService<UserDto, User, SearchObject, UserInsertRequest, UserUpdateRequest>, IUserService
{
    private readonly IMessageProducer _messageProducer;

    public UserService(DatabaseContext databaseContext, IMapper mapper, IMessageProducer messageProducer)
        : base(databaseContext, mapper)
    {
        _messageProducer = messageProducer;
    }

    protected override async Task BeforeInsert(User entity, UserInsertRequest insert)
    {
        if (await AlreadyExists(insert))
        {
            throw new AppException("User with the provided username or email already exists");
        }
        
        entity.RegistrationDate = DateTime.UtcNow;

        if (insert.ProfileImage?.EncodedImage == null) return;

        var savePath = Path.Combine("images", "users",
            $"{entity.Username}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.{insert.ProfileImage.Format}");
        var result = ImageHelper.SaveFromBase64(insert.ProfileImage.EncodedImage, insert.ProfileImage.Format, savePath);

        var newImageEntity = new Image()
        {
            Path = result.Path,
            Format = result.Format,
            Size = result.Size
        };

        await Context.Images.AddAsync(newImageEntity);
        entity.ProfileImage = newImageEntity;
    }

    protected override Task AfterInsert(User entity, UserInsertRequest insert)
    {
        _messageProducer.SendMessage("user_insertion", new RegisterUserMessage()
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email,
            Password = insert.Password
        });

        return Task.CompletedTask;
    }

    private async Task<bool> AlreadyExists(UserInsertRequest insert)
    {
        return await Context.Users.AnyAsync(user => user.Username == insert.Username || user.Email == insert.Email);
    }
}