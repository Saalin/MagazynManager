using System;
using System.Collections.Generic;

namespace MagazynManager.Infrastructure.InputModel.Authentication
{
    public class SetPermissionsModel
    {
        public Guid UserId { get; set; }
        public List<ClaimModel> Claims { get; set; }
    }
}