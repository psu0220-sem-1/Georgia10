using Server.Models;

namespace Server.Controllers
{
    public interface IAddressController: IController<Address>
    {
        /// <summary>
        /// Create summary
        /// </summary>
        /// <param name="street"></param>
        /// <param name="additionalInfo"></param>
        /// <param name="zip">Zip parameter needs to exist within the database, otherwise this operation will fail</param>
        /// <returns></returns>
        public Address Create(string street, string additionalInfo, int zip);
    }
}