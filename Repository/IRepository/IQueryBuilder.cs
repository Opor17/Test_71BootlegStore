using _71BootlegStore.Models;
using _71BootlegStore.ViewModels;

namespace _71BootlegStore.Repository.IRepository
{
    public interface IQueryBuilder
    {
        List<UserViewModel> GetAuthUserList();

        Role GetUserRole(string userId);

        List<OrdersUserViewModel> GetOrderUserList();

        List<OrdersUserViewModel> GetOrderUserListWhereId(string userId);

        List<ProductManagement> GetProductWhereName(string Name);

        List<OrdersUserViewModel> GetOrderUserListWhereIdWhereShippingStatus(string userId);

        public List<OrdersUserViewModel> GetOrderReport(ReportVM reportVM);

        List<ProductManagement> GetSizeList();

        List<OrdersUserViewModel> GetCartDetailListWhereUserId(string userId);

        List<CartDetail> GetCartDetailList(string userId);

        List<CartDetail> SaveCartDetailList(CartDetail cartDetail);

        List<BestSealler> GetOrderBestSeller(ReportVM reportVM);

        List<WorstsellerList> GetOrderWorstsellerList(ReportVM reportVM);
    }
}
