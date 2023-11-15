using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using coreAPI.Services;
using coreAPI.Models;
using coreAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace coreAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        #region Variables
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors
        public UsersController(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }
        #endregion

        /// <summary>
        /// Get User By ID
        /// </summary>
        /// HTTP GET : http://.../api/users/{id}
        /// <param name="id">User ID</param>
        /// <returns>JSON 
        /// {
        ///  "id": 0,
        ///  "login": "string",
        ///  "lastName": "string",
        ///  "firstName": "string",
        ///  "clock": "string",
        ///  "email": "user@example.com",
        ///  "createDate": "2021-09-30T19:02:25.345Z",
        ///  "updateDate": "2021-09-30T19:02:25.345Z",
        ///  "isActive": true,
        ///  "modifiedBy": "string"
        /// }
        /// OR HTTP 400/500 w/ error/validation message(s)
        /// </returns>
        /// 
        [HttpGet("{id}")]
        [Produces("application/json")]

        public async Task<ActionResult<UserModel>> Get(int id)
        {
            if (!_repository.UserExists(id))
            {
                throw new NotFoundException($"Invalid user ID - {id.ToString()} ");
            }

            var result = await _repository.GetUserAsync(id);

            return Ok(_mapper.Map<UserModel>(result));

        }

        /// <summary>
        /// Get List of Users per application (id)
        /// </summary>
        /// HTTP GET : http://.../api/users?appID={appID}
        /// <param name="id">User ID</param>
        /// <returns>JSON :
        /// [
        ///      {
        ///      "id": 0,        
        ///       "lastName": "string",
        ///       "firstName": "string",
        ///       "clock": "string",
        ///       "email": "user@example.com",
        ///       "createDate": "2021-09-30T19:02:25.345Z",
        ///        "updateDate": "2021-09-30T19:02:25.345Z",
        ///        "isActive": true,
        ///        "modifiedBy": "string"
        ///     }
        /// ]
        /// OR HTTP 400/500 w/ error/validation message
        /// </returns>
        [HttpGet("")]
        [Produces("application/json")]

        public async Task<ActionResult<IEnumerable<UserModel>>> GetByApplication([FromQuery] int appID)
        {
            IEnumerable<User> results = null;
                
            if(appID > 0)
            {

                results = await _repository.GetUsersByAppAsync(appID);
            }
            else
            {
                results = await _repository.GetUsersAsync();
            }        

            return  Ok(_mapper.Map<UserModel[]>(results));
        }


        /// <summary>
        /// Update User
        /// </summary>
        /// HTTP PUT: http://.../api/users?appID={appID}
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>Updated user JSON:
        //     {
        ///       "id": 0,        
        ///       "lastName": "string",
        ///       "firstName": "string",
        ///       "clock": "string",
        ///       "email": "user@example.com",
        ///       "createDate": "2021-09-30T19:02:25.345Z",
        ///        "updateDate": "2021-09-30T19:02:25.345Z",
        ///        "isActive": true,
        ///        "modifiedBy": "string"
        ///     }
        ///  OR HTTP 400/500 -  error/validation message(s)
        /// </returns>
        [HttpPut]
        [Route("{id:int}")]
        [Produces("application/json")]

        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserModel model)
        {
            if (id != model.ID)
            {
                throw new BadRequestException("Cant update user - ID mismatch");
            }

            try
            {
                User repoUser = _mapper.Map<Data.User>(model);

                await _repository.SaveUserAsync(repoUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.UserExists(id))
                {
                    throw new BadRequestException($"Invalid user ID - {id.ToString()} ");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Activate/Reactivate user - reset active flag - partial update
        /// </summary>
        /// HTTP PATCH : http://.../api/users/{id}
        /// <param name="id">User Id</param>
        /// <param name="patchDocument">JSON ex. :
        ///  [
        ///   {
        ///      "value": "false",
        ///        "path": "/IsActive",
        ///        "op": "replace"
        ///   }
        ///]
        /// </param>
        /// <returns>HTTP 204 - No Content
        ///  OR HTTP 400/500 w/ error/validation message(s)</returns>
        [HttpPatch]
        [HttpPatch("{id:int}")]
        [Produces("application/json")]

        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<UserModel> patchDocument)
        {
            if (!_repository.UserExists(id))
            {
                throw new BadRequestException($"Invalid user ID - {id.ToString()} ");
            }

            var repoUser = await _repository.GetUserAsync(id);
            var userToPatch = _mapper.Map<UserModel>(repoUser);            
            patchDocument.ApplyTo(userToPatch, ModelState);        
            
            if (!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userToPatch, repoUser);
            await _repository.UpdateUserAsync(repoUser);
            return NoContent();
        }

        public override ActionResult ValidationProblem(
         [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }


        /// <summary>
        /// Add New User
        /// </summary>
        ///  HTTP POST : http://.../api/users
        /// <param name="model">JSON body</param>
        /// <returns>HTTP 200 - success
        ///  OR HTTP 400/500 w/ error/validation message
        ///</returns>
        [HttpPost("")]
        [Produces("application/json")]

        public async Task<IActionResult> AddUser([FromBody] UserModel model)
        {
            try
            {

                User repoUser = _mapper.Map<User>(model);

                await _repository.SaveUserAsync(repoUser);

                return this.Ok();

            }

            catch (DbUpdateConcurrencyException)
            {

                throw new DataValidationException();

            }



        }
        /// <summary>
        /// Authenticate user
        /// </summary>
        /// HTTP POST : http://..../api/users/authenticate
        /// <param name="model">
        /// Body JSON:
        ///  {
        ///    "username" :"string",
        ///    "password" : "string"
        /// }
        /// </param>
        /// <returns>JSON:
        ///    {
        ///       "id": 0,        
        ///       "lastName": "string",
        ///       "firstName": "string",
        ///       "clock": "string",
        ///       "email": "user@example.com",
        ///       "createDate": "2021-09-30T19:02:25.345Z",
        ///        "updateDate": "2021-09-30T19:02:25.345Z",
        ///        "isActive": true,
        ///        "modifiedBy": "string"
        ///     }
        ///  OR HTTP 400/500 -  error/validation message(s)
        /// </returns>
        [HttpPost("authenticate")]
        [Produces("application/json")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {

            var result = await _repository.AuthenticateAsync(model);
            return Ok(result);
        }

      
  




    }
}
