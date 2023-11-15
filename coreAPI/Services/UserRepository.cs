using coreAPI.Data;
using coreAPI.Models;
using CoreAPI.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace coreAPI.Services
{

    public class UserRepository : IUserRepository
    {
        #region Variables

        private readonly UserDBContext _context;
        private readonly AppSettings _appSettings;

        #endregion

        #region Constructors
        public UserRepository(UserDBContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>JSON:
        /// {
        ///  "id": 0,
        ///  "login": "string",
        ///  "lastName": "string",
        ///  "firstName": "string",
        ///  "clock": "string",
        ///  "email": "user@example.com",
        ///  "createDate": "2021-09-30T17:58:20.688Z",
        ///  "updateDate": "2021-09-30T17:58:20.688Z",
        ///  "isActive": true,
        ///  "modifiedBy": "string"
        ///  }
        /// </returns>
        public async Task<User> GetUserAsync(int id)
        {
            IQueryable<User> query = _context.Users.Where(c => c.UsrId == id);

            return await query.FirstOrDefaultAsync();

        }


        /// <summary>
        /// Get List of active  users
        /// </summary>
        /// <returns>JSON:
        /// [
        /// {
        ///  "id": 0,
        /// "login": "string",
        ///  "lastName": "string",
        ///  "firstName": "string",
        ///  "clock": "string",
        ///  "email": "user@example.com",
        ///  "createDate": "2021-09-30T17:58:20.688Z",
        ///  "updateDate": "2021-09-30T17:58:20.688Z",
        ///  "isActive": true,
        ///  "modifiedBy": "string"
        ///  }
        /// ]
        /// </returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {

            IQueryable<User> query = _context.Users.Where(c => c.UsrActive == true);
            return await query.ToListAsync();
        }


        /// <summary>
        /// Get List of users by application ID
        /// </summary>
        /// <param name="appID">application id</param>
        /// <returns>JSON Array:
        ///[
        ///{
        ///  "id": 0,
        /// "login": "string",
        ///  "lastName": "string",
        ///  "firstName": "string",
        ///  "clock": "string",
        ///  "email": "user@example.com",
        ///  "createDate": "2021-09-30T17:58:20.688Z",
        ///  "updateDate": "2021-09-30T17:58:20.688Z",
        ///  "isActive": true,
        ///  "modifiedBy": "string"
        ///  }
        ///  , ...
        ///  ]
        /// </returns>
        public async Task<IEnumerable<User>> GetUsersByAppAsync(int appID)
        {
            try
            {
                var id = new SqlParameter { ParameterName = "@appID", Value = appID };

                IQueryable<User> query = _context.Users
                                .FromSqlRaw("EXECUTE usp_selUsersByApplication @appID", id);

                return await query.ToListAsync();
            }
            catch (SqlException oSqlException)   //stored procedure ex
            {
                if (oSqlException.Number == Constants.ErrorCode.InvalidApplication)
                {
                    throw new DataValidationException(oSqlException.Message);
                }

                throw;
            }

        }


        /// <summary>
        /// Dupe  check
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>boolean flag</returns>
        public bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UsrId == id);
        }


        /// <summary>
        /// Update  user data
        /// </summary>
        /// <param name="user">
        /// JSON:
        ///  {
        ///  "id": 0,
        /// "login": "string",
        ///  "lastName": "string",
        ///  "firstName": "string",
        ///  "clock": "string",
        ///  "email": "user@example.com",
        ///  "createDate": "2021-09-30T17:58:20.688Z",
        ///  "updateDate": "2021-09-30T17:58:20.688Z",
        ///  "isActive": true,
        ///  "modifiedBy": "string"
        ///  }
        /// </param>
        /// <returns></returns>
        public async Task UpdateUserAsync(User user)
        {
            var oUser = _context.Entry(user);

            oUser.State = EntityState.Modified;
            oUser.Property(x => x.UsrCreatedDate).IsModified = false;

            user.UsrModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

        }


        /// <summary>
        /// Authenticate user
        /// </summary>
        /// Invoke stored procedure 
        /// <param name="model">AuthenticateRequest model - JSON:
        /// {
        ///    "username" :"string",
        ///    "password" : "string",
        ///    "appID" : number
        /// }
        /// </param>
        /// <returns>JSON:
        ///  {
        ///  "id": 0,
        ///  "login": "string",
        ///  "lastName": "string",
        ///  "firstName": "string",
        ///  "clock": "string",
        ///  "email": "user@example.com",
        ///  "createDate": "2021-09-30T17:58:20.688Z",
        ///  "updateDate": "2021-09-30T17:58:20.688Z",
        ///  "isActive": true,
        ///  "modifiedBy": "string"
        ///  }</returns>
        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
        {
            var oUser = new Data.User();      //user entity

            try
            {
                var lstParams = new[] {
                    new SqlParameter("@applicationID", model.appID ),      //@environment.appID     
                    new SqlParameter("@username", model.Username),
                    new SqlParameter("@password",  model.Password)
                };

                using (var oConnection = _context.Database.GetDbConnection())
                {
                    var oCommand = oConnection.CreateCommand();
                    oCommand.Connection = oConnection;
                    oConnection.Open();

                    oCommand.CommandType = CommandType.StoredProcedure;
                    oCommand.CommandText = "[dbo].usp_Authenticate";
                    oCommand.Parameters.AddRange(lstParams.ToArray());


                    using (var dbDataReader = await oCommand.ExecuteReaderAsync())
                    {

                        //read  db record
                        while (dbDataReader.Read())
                        {
                            oUser.UsrId = (int)dbDataReader["usrID"];
                            oUser.UsrLogin = (string)dbDataReader["usrlogin"];
                            oUser.UsrFirstName = (string)dbDataReader["usrFirstName"];
                            oUser.UsrLastName = (string)dbDataReader["usrLastName"];
                            oUser.UsrEmail = (string)dbDataReader["usrEmail"];
                            oUser.UsrClock = (string)dbDataReader["usrClock"];
                            oUser.UsrCreatedBy = (string)dbDataReader["usrCreatedBy"];
                            oUser.UsrCreatedDate = (DateTime)dbDataReader["usrCreatedDate"];
                            oUser.UsrActive = (bool)dbDataReader["usrActive"];
                            oUser.UsrModifiedDate = (DateTime)dbDataReader["usrModifiedDate"];
                            oUser.UsrModifiedBy = (string)dbDataReader["usrModifiedBy"];
                            oUser.UsrStateId = (int)dbDataReader["usrStateID"];
                        }
                    }
                }
            }
            catch (SqlException oSqlException)   //stored procedure ex
            {
                if (oSqlException.Number == Constants.ErrorCode.InvalidAccount ||
                    oSqlException.Number == Constants.ErrorCode.WrongPassword ||
                    oSqlException.Number == Constants.ErrorCode.InvalidUser ||
                    oSqlException.Number == Constants.ErrorCode.NoApplicationPermissions)
                {
                    throw new NotFoundException(oSqlException.Message);
                }

                throw;    //any other exeption
            }

            //authentication successful so generate jwt token
            var token = GenerateJwtToken(oUser, model.appID);

            return (oUser == null) ? null : new AuthenticateResponse(oUser, token);
        }


        private string GenerateJwtToken(User user, int iAppID = 0)
        {
            // generate token that is valid for 2  days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var lstClaims = new List<Claim>();
            lstClaims.Add(new Claim(Constants.Claims.ID, user.UsrId.ToString()));
            if (iAppID > 0)
            {
                lstClaims.Add(new Claim(Constants.Claims.ApplicationID, iAppID.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(lstClaims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(2),   //expires @ 2 days
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        /// <summary>
        /// Save User content
        /// </summary>
        /// <param name="repoUser"></param>
        /// <returns></returns>
        public async Task SaveUserAsync(User repoUser)
            {
                try
                {

                    var usrID = new SqlParameter("@usrID", (repoUser.UsrId == 0) ? System.DBNull.Value : repoUser.UsrId);
                    var usrLogin = new SqlParameter("@usrLogin", repoUser.UsrLogin);
                    var usrLastName = new SqlParameter("@usrLastName", repoUser.UsrLastName);
                    var usrFirstName = new SqlParameter("@usrFirstName", repoUser.UsrFirstName);
                    var usrClock = new SqlParameter("@usrClock", (String.IsNullOrWhiteSpace(repoUser.UsrClock)) ? System.DBNull.Value : repoUser.UsrClock);
                    var usrStateID = new SqlParameter("@usrStateID", repoUser.UsrStateId);
                    var usrEmail = new SqlParameter("@usrEmail", repoUser.UsrEmail);

                    var outID = new SqlParameter();
                    outID.ParameterName = "@outID";
                    outID.SqlDbType = SqlDbType.Int;
                    outID.Direction = ParameterDirection.Output;

                    await _context.Database.ExecuteSqlRawAsync("EXEC usp_execUser @usrID = {0}," +
                        " @usrLogin = {1}," +
                        " @usrLastName = {2}," +
                        " @usrFirstName = {3}," +
                        " @usrClock = {4}," +
                        " @usrEmail ={5}, " +
                        " @usrStateID = {6}, " +
                        " @outID = {7} OUT ",
                        usrID, usrLogin, usrLastName, usrFirstName, usrClock, usrEmail, usrStateID, outID);

                    //return user ID - output parameter
                    repoUser.UsrId = (int)outID.Value;

                }
                catch (SqlException oSqlException)   //stored procedure exeption
                {
                    if (oSqlException.Number == Constants.ErrorCode.InvalidUser)
                    {
                        throw new DataValidationException(oSqlException.Message);
                    }

                    throw;  // any other exception
                }


        }
        #endregion
    }
}
