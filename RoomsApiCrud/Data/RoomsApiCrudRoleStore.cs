using System.Data.SqlClient;

using Dapper;

using Microsoft.AspNetCore.Identity;

using RoomsApiCrud.Models;

namespace RoomsApiCrud.Data
{
    public class RoomsApiCrudRoleStore : IRoleStore<RoomsApiCrudRole>
    {
        private readonly string _connectionString;

        public RoomsApiCrudRoleStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RoomsApiCrudConn");
        }

        public async Task<IdentityResult> CreateAsync(RoomsApiCrudRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                role.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [roomsApiCrudRole] ([name], [normalizedName])
                    VALUES (@{nameof(RoomsApiCrudRole.Name)}, @{nameof(RoomsApiCrudRole.NormalizedName)});
                    SELECT CAST(SCOPE_IDENTITY() as int)", role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(RoomsApiCrudRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE [roomsApiCrudRole] SET
                    [name] = @{nameof(RoomsApiCrudRole.Name)},
                    [normalizedName] = @{nameof(RoomsApiCrudRole.NormalizedName)}
                    WHERE [id] = @{nameof(RoomsApiCrudRole.Id)}", role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(RoomsApiCrudRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [roomsApiCrudRole] WHERE [id] = @{nameof(RoomsApiCrudRole.Id)}", role);
            }

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(RoomsApiCrudRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(RoomsApiCrudRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(RoomsApiCrudRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(RoomsApiCrudRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(RoomsApiCrudRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<RoomsApiCrudRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<RoomsApiCrudRole>($@"SELECT * FROM [roomsApiCrudRole]
                    WHERE [id] = @{nameof(roleId)}", new { roleId });
        }

        public async Task<RoomsApiCrudRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<RoomsApiCrudRole>($@"SELECT * FROM [roomsApiCrudRole]
                    WHERE [normalizedName] = @{nameof(normalizedRoleName)}", new { normalizedRoleName });
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}