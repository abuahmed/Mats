using System.ComponentModel.DataAnnotations.Schema;


namespace MATS.Core.Models.Interfaces
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}
