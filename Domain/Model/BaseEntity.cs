using Domain.Interface;

namespace Domain.Model
{
    public class BaseEntity: IEntity
    {

        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
    