/// <summary>
/// Enum class for mapping the types in the DB to the types used in the software. 
/// </summary>
namespace Api.Models
{
    public partial class MemberType
    {
        //You are also called TypeID - are you the Enum then, or what?
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
