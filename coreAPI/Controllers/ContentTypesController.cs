using AutoMapper;
using coreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using coreAPI.Data;
using coreAPI.Models;


namespace coreAPI.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class ContentTypesController : ControllerBase
    {

        #region Variables
        private readonly IContentTypeRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        #endregion

        #region Constructors
        public ContentTypesController(IContentTypeRepository repository, IMapper mapper,
                             LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }
        #endregion

        #region methods




        /// <summary>
        /// Get Content Types 
        /// HTTP GET  http://.../api/contenttypes
        /// </summary>
        /// <returns>
        /// ContentTypeModel JSON array :
        ///  [
        ///     {
        //          "id": 0,
        //           "name": "string"
        //      }
        // ]
        /// OR HTTP 500 - error message
        /// </returns> 
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ContentTypeModel>>> Get()
        {
            var lstContentTypes = await _repository.GetContentTypesAsync();

            IEnumerable<ContentTypeModel> results = _mapper.Map<IEnumerable<ContentType>, IEnumerable<ContentTypeModel>>((lstContentTypes));

            return this.Ok(results);
       }


        /// <summary>
        ///Content type By ID
        ///HTTP GET: http://.../api/contenttypes/{id}
        /// </summary>
        /// <param name="id">Content Type ID</param>
        /// <returns> JSON
        ///     {
        //          "id": 0,
        //           "name": "string"
        //      }
        /// OR HTTP 400/500 - error/validation messages
        /// </returns>
        [HttpGet("{id:int}")]
        [Produces("application/json")]

        public async Task<ActionResult<ContentTypeModel>> Get(int id)
        {
         
            var result = await _repository.GetContentTypeAsync(id);

            if (result == null)
            {
                throw new NotFoundException($"Invalid content type ID - {id.ToString()} ");
            }

            return Ok(_mapper.Map<ContentTypeModel>(result));

        }

        /// <summary>
        /// Delete content type by ID
        /// HTTP DELETE : http://.../api/contenttype/{id}
        /// </summary>
        /// <param name="id">Content type id</param>
        /// <returns>HTTP 200 - No JSON result 
        /// OR HTTP 400/500 - error/validation message(s)</returns>
        [Produces("application/json")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteContentType(int id)
        {
            var delContentType = await _repository.GetContentTypeAsync(id);

            if (delContentType == null)
            {
                throw new BadRequestException($"Invalid content type: ID - {id.ToString()} ");
            }

            await _repository.DeleteContentTypeAsync(id);

            return Ok();
          
        }

        /// <summary>
        /// Add new content type record
        /// </summary>
        /// HTTP POST : http://.../api/contenttypes
        /// <param name="model">Body - raw JSON </param>
        /// <returns>HTTP  201  - JSON:
        ///  {
        ///          "id":  int - next identity value
        ///           "name": "string"
        ///     }
        /// OR HTTP 400/500 w/ error/validation message
        /// </returns>
        [HttpPost]
        [Produces("application/json")]

        public async Task<IActionResult> Post([FromBody] ContentTypeModel model)
        {
 

            if (_repository.ContentTypeExists(model.Name))
            {
                throw new DataValidationException("Existing content type");
            }

            var repoContentType = _mapper.Map<ContentType>(model);

            await _repository.AddContentTypeAsync(repoContentType);

            var newContentType = _mapper.Map<ContentTypeModel>(repoContentType);

            //Route  - GET - newly created ContentType
            var url = _linkGenerator.GetPathByAction(HttpContext, "Get", values: new { id = newContentType.ID });

            return Created(url, _mapper.Map<ContentTypeModel>(newContentType));   //http status - 201
        }


        #endregion
    }
}
