using AutoMapper;
using coreAPI.Data;
using coreAPI.Models;

namespace coreAPI
{
    /// <summary>
    /// Data mapping between 
    /// ContentType Entity - Data/ContentType.cs  
    /// AND
    /// ContentType Model - /Models/ContentTypeModel.cs 
    /// </summary>
    public class ContentTypeProfile :Profile
    {

        public ContentTypeProfile()
        {

            var configuration = new MapperConfiguration(cfg => {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<ContentType, ContentTypeModel>();
            });

            this.CreateMap<ContentType, ContentTypeModel>()
               .ForMember(m => m.ID, o => o.MapFrom(c => c.CntId))
               .ForMember(m => m.Name, o => o.MapFrom(c => c.CntName))
               .ReverseMap();
        }
    }
}
