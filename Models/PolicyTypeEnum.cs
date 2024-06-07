namespace _71BootlegStore.Models
{
    public enum PolicyTypeEnum
    {
        #region Master Data

        #region Authorize

        List_Authorize,
        View_Detail_Authorize,
        Create_Authorize,
        Edit_Authorize,
        Delete_Authorize,

        #endregion

        #region Role

        List_Role,
        View_Detail_Role,
        Create_Role,
        Edit_Role,
        Delete_Role,

        #endregion

        #region ProductManagement

        List_ProductManagement,
        View_Detail_ProductManagement,
        Create_ProductManagement,
        Edit_ProductManagement,
        Delete_ProductManagement,

        #endregion

        #region News

        List_News,
        View_Detail_News,
        Create_News,
        Edit_News,
        Delete_News,

        #endregion

        #region Order

        List_Order,
        View_Detail_Order,
        Create_Order,
        Edit_Order,
        Delete_Order,

        #endregion

        #region Shipping

        List_Shipping,
        View_Detail_Shipping,
        Create_Shipping,
        Edit_Shipping,
        Delete_Shipping,

        #endregion

        #endregion

        #region PurchaseHistory

        List_PurchaseHistory,
        View_Detail_PurchaseHistory,
        Create_PurchaseHistory,
        Edit_PurchaseHistory,
        Delete_PurchaseHistory,

        #endregion

        #region ShippingUser

        List_ShippingUser,
        View_Detail_ShippingUser,
        Create_ShippingUser,
        Edit_ShippingUser,
        Delete_ShippingUser,

        #endregion

        #region Report

        List_Report,
        View_Detail_Report,
        Create_Report,
        Edit_Report,
        Delete_Report,

        #endregion
    }

    public enum PolicyValueEnum
    {
        PC
    }
    public static class PolicyEnumToString
    {
        public static string PolicyTypeToString(this PolicyTypeEnum input)
        {
            return input.ToString().Replace("_", " ");
        }
    }
}
