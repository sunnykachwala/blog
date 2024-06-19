namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    public interface IDefault
    {
        [MaxLength(256)]
        public string SystemUser { get; set; }

        [MaxLength(256)]
        public string AppName { get; set; }
    }
}
