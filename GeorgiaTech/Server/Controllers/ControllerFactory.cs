using System;
using Server.Models;

namespace Server.Controllers
{
    public static class ControllerFactory
    {
        public static IAddressController CreateAddressController(GTLContext context)
        {
            return new AddressController();
        }
        
        public static IAuthorController CreateAuthorController(GTLContext context)
        {
            return new AuthorController(context);
        }

        /*
        public static ILoanController CreateLoanController(GTLContext context)
        {
            return new LoanController();
        }
        */
            
        public static IMaterialController CreateMaterialController(GTLContext context)
        {
            return new MaterialController();
        }

        public static IMemberController CreateMemberController(GTLContext context)
        {
            return new MemberController();
        }

        /*
        public static IVolumeController CreateVolumeController(GTLContext context)
        {
            return new VolumeController();
        }
        */
    }
}