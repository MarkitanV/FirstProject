using System.ComponentModel.DataAnnotations;
using FirstProject.Domain;
using FirstProject.Domain.Entities;
using FirstProject.Service;
namespace FirstProject.Domain.Entities
{
    public class ServiceItem: EntityBase
    {
        [Required(ErrorMessage ="Заполните название услуги")]
        [Display(Name ="название услуги")]
        public override string Title { get; set; }

        [Display(Name ="краткое описание услуги")]
        public override string Subtitle { get; set; }

        [Display(Name ="Полное описание услуги")]
        public override string Text { get; set; }
    }
}
