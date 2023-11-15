using System.ComponentModel.DataAnnotations;

namespace coreAPI.Models
{
    public class ContentTypeModel
    {

        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
