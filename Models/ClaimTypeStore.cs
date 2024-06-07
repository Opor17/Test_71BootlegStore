namespace _71BootlegStore.Models
{
    public class ClaimTypeStore
    {
        public string ClaimValue { get; set; }
        public List<string> RoleClaimsAuthorize { get; set; }

        public List<bool> BoolsClaimsAuthorize { get; set; }

        public List<string> RoleClaimsRole { get; set; }

        public List<bool> BoolsClaimsRole { get; set; }

        public List<string> RoleClaimsProductManagement { get; set; }

        public List<bool> BoolsClaimsProductManagement { get; set; }
        
        public List<string> RoleClaimsNews { get; set; }

        public List<bool> BoolsClaimsNews { get; set; }
        
        public List<string> RoleClaimsOrder { get; set; }

        public List<bool> BoolsClaimsOrder { get; set; }
        
        public List<string> RoleClaimsShipping { get; set; }

        public List<bool> BoolsClaimsShipping { get; set; }
        
        public List<string> RoleClaimsPurchaseHistory { get; set; }

        public List<bool> BoolsClaimsPurchaseHistory { get; set; }
        
        public List<string> RoleClaimsShippingUser { get; set; }

        public List<bool> BoolsClaimsShippingUser { get; set; }
        
        public List<string> RoleClaimsReport { get; set; }

        public List<bool> BoolsClaimsReport { get; set; }

        public ClaimTypeStore()
        {
            RoleClaimsAuthorize = new List<string>()
            {
                PolicyTypeEnum.List_Authorize.PolicyTypeToString(),
                PolicyTypeEnum.View_Detail_Authorize.PolicyTypeToString(),
                PolicyTypeEnum.Create_Authorize.PolicyTypeToString(),
                PolicyTypeEnum.Edit_Authorize.PolicyTypeToString(),
                PolicyTypeEnum.Delete_Authorize.PolicyTypeToString()
            };

            RoleClaimsRole = new List<string>()
            {
                PolicyTypeEnum.List_Role.PolicyTypeToString(),
                PolicyTypeEnum.View_Detail_Role.PolicyTypeToString(),
                PolicyTypeEnum.Create_Role.PolicyTypeToString(),
                PolicyTypeEnum.Edit_Role.PolicyTypeToString(),
                PolicyTypeEnum.Delete_Role.PolicyTypeToString()
            };

            RoleClaimsProductManagement = new List<string>()
            {
                PolicyTypeEnum.List_ProductManagement.PolicyTypeToString(),
                PolicyTypeEnum.View_Detail_ProductManagement.PolicyTypeToString(),
                PolicyTypeEnum.Create_ProductManagement.PolicyTypeToString(),
                PolicyTypeEnum.Edit_ProductManagement.PolicyTypeToString(),
                PolicyTypeEnum.Delete_ProductManagement.PolicyTypeToString()
            };
            
            RoleClaimsNews = new List<string>()
            {
                PolicyTypeEnum.List_News.PolicyTypeToString(),
                PolicyTypeEnum.View_Detail_News.PolicyTypeToString(),
                PolicyTypeEnum.Create_News.PolicyTypeToString(),
                PolicyTypeEnum.Edit_News.PolicyTypeToString(),
                PolicyTypeEnum.Delete_News.PolicyTypeToString()
            };

            RoleClaimsOrder = new List<string>()
            {
                PolicyTypeEnum.List_Order.PolicyTypeToString(),
                PolicyTypeEnum.View_Detail_Order.PolicyTypeToString(),
                PolicyTypeEnum.Create_Order.PolicyTypeToString(),
                PolicyTypeEnum.Edit_Order.PolicyTypeToString(),
                PolicyTypeEnum.Delete_Order.PolicyTypeToString()
            };

            RoleClaimsShipping = new List<string>()
            {
                PolicyTypeEnum.List_Shipping.PolicyTypeToString(),
                PolicyTypeEnum.View_Detail_Shipping.PolicyTypeToString(),
                PolicyTypeEnum.Create_Shipping.PolicyTypeToString(),
                PolicyTypeEnum.Edit_Shipping.PolicyTypeToString(),
                PolicyTypeEnum.Delete_Shipping.PolicyTypeToString()
            };

            RoleClaimsPurchaseHistory = new List<string>()
            {
                PolicyTypeEnum.List_PurchaseHistory.PolicyTypeToString(),
                PolicyTypeEnum.View_Detail_PurchaseHistory.PolicyTypeToString(),
                PolicyTypeEnum.Create_PurchaseHistory.PolicyTypeToString(),
                PolicyTypeEnum.Edit_PurchaseHistory.PolicyTypeToString(),
                PolicyTypeEnum.Delete_PurchaseHistory.PolicyTypeToString()
            };

            RoleClaimsShippingUser = new List<string>()
            {
                PolicyTypeEnum.List_ShippingUser.PolicyTypeToString(),
                PolicyTypeEnum.View_Detail_ShippingUser.PolicyTypeToString(),
                PolicyTypeEnum.Create_ShippingUser.PolicyTypeToString(),
                PolicyTypeEnum.Edit_ShippingUser.PolicyTypeToString(),
                PolicyTypeEnum.Delete_ShippingUser.PolicyTypeToString()
            };

            RoleClaimsReport = new List<string>()
            {
                PolicyTypeEnum.List_Report.PolicyTypeToString(),
                PolicyTypeEnum.View_Detail_Report.PolicyTypeToString(),
                PolicyTypeEnum.Create_Report.PolicyTypeToString(),
                PolicyTypeEnum.Edit_Report.PolicyTypeToString(),
                PolicyTypeEnum.Delete_Report.PolicyTypeToString()
            };
        }
    }
}
