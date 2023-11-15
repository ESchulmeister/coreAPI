using coreAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coreAPI.Services
{
   
     /// <summary>
    /// Content type endpoints 
    /// </summary>
     public  interface IContentTypeRepository
    {

       Task<ContentType> GetContentTypeAsync(int id);

        Task<IEnumerable<ContentType>> GetContentTypesAsync();

        Task DeleteContentTypeAsync(int id);

        Task AddContentTypeAsync(ContentType entContenttype);

        bool ContentTypeExists(string name);


    }
}
