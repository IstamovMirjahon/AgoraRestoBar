using System.ComponentModel.DataAnnotations;

namespace Agora.Domain.Abstractions
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
