using System.Data.SqlClient;

using Dapper;

using Microsoft.AspNetCore.Identity;

using RoomsApiCrud.Models;

namespace RoomsApiCrud.Data
{
    public class UserStore : IUserStore<RoomsApiCrudUser>, IUserEmailStore<RoomsApiCrudUser>, IUserPhoneNumberStore<RoomsApiCrudUser>,
        IUserTwoFactorStore<RoomsApiCrudUser>, IUserPasswordStore<RoomsApiCrudUser>, IUserRoleStore<RoomsApiCrudUser>
    {
        private readonly string _connectionString;

        public UserStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RoomsApiCrudConn");
        }

        public async Task<IdentityResult> CreateAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                user.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [roomsApiCrudUser] ([userName], [normalizedUserName], [email],
                    [normalizedEmail], [emailConfirmed], [passwordHash], [phoneNumber], [phoneNumberConfirmed], [twoFactorEnabled])
                    VALUES (@{nameof(RoomsApiCrudUser.UserName)}, @{nameof(RoomsApiCrudUser.NormalizedUserName)}, @{nameof(RoomsApiCrudUser.Email)},
                    @{nameof(RoomsApiCrudUser.NormalizedEmail)}, @{nameof(RoomsApiCrudUser.EmailConfirmed)}, @{nameof(RoomsApiCrudUser.PasswordHash)},
                    @{nameof(RoomsApiCrudUser.PhoneNumber)}, @{nameof(RoomsApiCrudUser.PhoneNumberConfirmed)}, @{nameof(RoomsApiCrudUser.TwoFactorEnabled)});
                    SELECT CAST(SCOPE_IDENTITY() as int)", user);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [roomsApiCrudUser] WHERE [id] = @{nameof(RoomsApiCrudUser.Id)}", user);
            }

            return IdentityResult.Success;
        }

        public async Task<RoomsApiCrudUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<RoomsApiCrudUser>($@"SELECT * FROM [roomsApiCrudUser]
                    WHERE [id] = @{nameof(userId)}", new { userId });
        }

        public async Task<RoomsApiCrudUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<RoomsApiCrudUser>($@"SELECT * FROM [roomsApiCrudUser]
                    WHERE [normalizedUserName] = @{nameof(normalizedUserName)}", new { normalizedUserName });
        }

        public Task<string> GetNormalizedUserNameAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(RoomsApiCrudUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(RoomsApiCrudUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE [roomsApiCrudUser] SET
                    [userName] = @{nameof(RoomsApiCrudUser.UserName)},
                    [normalizedUserName] = @{nameof(RoomsApiCrudUser.NormalizedUserName)},
                    [email] = @{nameof(RoomsApiCrudUser.Email)},
                    [normalizedEmail] = @{nameof(RoomsApiCrudUser.NormalizedEmail)},
                    [emailConfirmed] = @{nameof(RoomsApiCrudUser.EmailConfirmed)},
                    [passwordHash] = @{nameof(RoomsApiCrudUser.PasswordHash)},
                    [phoneNumber] = @{nameof(RoomsApiCrudUser.PhoneNumber)},
                    [phoneNumberConfirmed] = @{nameof(RoomsApiCrudUser.PhoneNumberConfirmed)},
                    [twoFactorEnabled] = @{nameof(RoomsApiCrudUser.TwoFactorEnabled)}
                    WHERE [id] = @{nameof(RoomsApiCrudUser.Id)}", user);
            }

            return IdentityResult.Success;
        }

        public Task SetEmailAsync(RoomsApiCrudUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(RoomsApiCrudUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<RoomsApiCrudUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<RoomsApiCrudUser>($@"SELECT * FROM [roomsApiCrudUser]
                    WHERE [normalizedEmail] = @{nameof(normalizedEmail)}", new { normalizedEmail });
        }

        public Task<string> GetNormalizedEmailAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(RoomsApiCrudUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(RoomsApiCrudUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(RoomsApiCrudUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(RoomsApiCrudUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetPasswordHashAsync(RoomsApiCrudUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task AddToRoleAsync(RoomsApiCrudUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var normalizedName = roleName.ToUpper();
            var roleId = await connection.ExecuteScalarAsync<int?>($"SELECT [id] FROM [applicationRole] WHERE [normalizedName] = @{nameof(normalizedName)}", new { normalizedName });
            if (!roleId.HasValue)
                roleId = await connection.ExecuteAsync($"INSERT INTO [applicationRole]([name], [normalizedName]) VALUES(@{nameof(roleName)}, @{nameof(normalizedName)})",
                    new { roleName, normalizedName });

            await connection.ExecuteAsync($"IF NOT EXISTS(SELECT 1 FROM [roomsApiCrudUserRole] WHERE [userId] = @userId AND [roleId] = @{nameof(roleId)}) " +
                $"INSERT INTO [roomsApiCrudUserRole]([userId], [roleId]) VALUES(@userId, @{nameof(roleId)})",
                new { userId = user.Id, roleId });
        }

        public async Task RemoveFromRoleAsync(RoomsApiCrudUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var roleId = await connection.ExecuteScalarAsync<int?>("SELECT [Id] FROM [ApplicationRole] WHERE [NormalizedName] = @normalizedName", new { normalizedName = roleName.ToUpper() });
            if (!roleId.HasValue)
                await connection.ExecuteAsync($"DELETE FROM [RoomsApiCrudUserRole] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}", new { userId = user.Id, roleId });
        }

        public async Task<IList<string>> GetRolesAsync(RoomsApiCrudUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var queryResults = await connection.QueryAsync<string>("SELECT r.[Name] FROM [ApplicationRole] r INNER JOIN [RoomsApiCrudUserRole] ur ON ur.[RoleId] = r.Id " +
                "WHERE ur.UserId = @userId", new { userId = user.Id });

            return queryResults.ToList();
        }

        public async Task<bool> IsInRoleAsync(RoomsApiCrudUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            var roleId = await connection.ExecuteScalarAsync<int?>("SELECT [Id] FROM [ApplicationRole] WHERE [NormalizedName] = @normalizedName", new { normalizedName = roleName.ToUpper() });
            if (roleId == default(int)) return false;
            var matchingRoles = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM [RoomsApiCrudUserRole] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}",
                new { userId = user.Id, roleId });

            return matchingRoles > 0;
        }

        public async Task<IList<RoomsApiCrudUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            var queryResults = await connection.QueryAsync<RoomsApiCrudUser>("SELECT u.* FROM [RoomsApiCrudUser] u " +
                "INNER JOIN [RoomsApiCrudUserRole] ur ON ur.[UserId] = u.[Id] INNER JOIN [ApplicationRole] r ON r.[Id] = ur.[RoleId] WHERE r.[NormalizedName] = @normalizedName",
                new { normalizedName = roleName.ToUpper() });

            return queryResults.ToList();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}