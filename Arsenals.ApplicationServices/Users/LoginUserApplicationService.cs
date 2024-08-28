using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Text.Json.Serialization;
using Arsenals.Domains.Users;
using Arsenals.Domains.Users.Exceptions;

namespace Arsenals.ApplicationServices.Users;

public class LoginUserApplicationService
{
    private readonly IUserRepository _repository;

    public LoginUserApplicationService(IUserRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));

        _repository = repository;
    }

    /// <summary>
    /// ログイン処理
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="UserNotFoundException"></exception>
    /// <exception cref="InvalidCredentialException"></exception>
    public async Task ExecuteAsync(LoginRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        UserId userId = new UserId(request.UserId);
        Password password = new Password(request.Password);

        User? user = await _repository.FetchAsync(userId);
        if (user == null) { throw new UserNotFoundException(userId); }

        bool checkPassword = await _repository.CheckPasswordAsync(userId, password);
        if (checkPassword == false)
        {
            throw new InvalidCredentialException();
        }
    }
}

public class LoginRequestDto
{
    [Required]
    [JsonPropertyName("userId")]
    public required string UserId { get; set; }

    [Required]
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}

public class LoginResponseDto
{
    public string? Token { get; set; }
}