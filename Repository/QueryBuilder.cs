using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Dapper;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;

namespace _71BootlegStore.Repository
{
    public class QueryBuilder : IQueryBuilder
    {
        private readonly string connectionString;
        private readonly ApplicationDbContext _context;
        public QueryBuilder(
            IConfiguration configuration,
            ApplicationDbContext Context
        )
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = Context;
        }

        public List<UserViewModel> GetAuthUserList()
        {
            var sql = @"SELECT a.*,c.Id AS RoleId,c.Name AS RoleName FROM AspNetUsers a 
                        LEFT JOIN AspNetUserRoles b ON a.Id=b.UserId
                        LEFT JOIN AspNetRoles c ON b.RoleId=c.Id";

            var authUsers = new List<UserViewModel>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                authUsers = connection.Query<UserViewModel>(sql).ToList();
            }

            return authUsers;
        }

        public Role GetUserRole(string userId)
        {
            var sql = @"SELECT a.* FROM AspNetRoles a LEFT JOIN AspNetUserRoles b ON a.Id=b.RoleId WHERE b.UserId=@userId";

            var role = new Role();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                role = connection.QueryFirstOrDefault<Role>(sql, new { userId });
            }

            if (role != null)
            {
                return role;
            }
            return new Role();
        }

        public List<OrdersUserViewModel> GetOrderUserList()
        {
            var sql = @"SELECT a.*, b.UserName, b.FirstName FROM Orders a LEFT JOIN AspNetUsers b ON a.UserId = b.Id";

            var orders = new List<OrdersUserViewModel>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                orders = connection.Query<OrdersUserViewModel>(sql).ToList();
            }

            return orders;
        }

        public List<OrdersUserViewModel> GetOrderUserListWhereId(string userId)
        {
            var sql = @"SELECT a.*, b.UserName, b.FirstName, b.Address FROM Orders a LEFT JOIN AspNetUsers b ON a.UserId = b.Id WHERE b.Id = @userId";

            var orders = new List<OrdersUserViewModel>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                orders = connection.Query<OrdersUserViewModel>(sql, new { userId }).ToList();
            }

            return orders;
        }

        public List<ProductManagement> GetProductWhereName(string Name)
        {
            var sql = @"SELECT * FROM ProductManagement WHERE Name = @Name";

            var product = new List<ProductManagement>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                product = connection.Query<ProductManagement>(sql, new { Name }).ToList();
            }

            return product;
        }

        public List<OrdersUserViewModel> GetOrderUserListWhereIdWhereShippingStatus(string userId)
        {
            var sql = @"SELECT a.*, b.UserName, b.FirstName, b.Address FROM Orders a LEFT JOIN AspNetUsers b ON a.UserId = b.Id WHERE b.Id = @userId AND ShippingStatus != '' ";

            var orders = new List<OrdersUserViewModel>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                orders = connection.Query<OrdersUserViewModel>(sql, new { userId }).ToList();
            }

            return orders;
        }

        public List<OrdersUserViewModel> GetOrderReport(ReportVM reportVM)
        {
            var query = "";
            var dateTime = "";
            dateTime = $@"CreatedAt BETWEEN '{reportVM.FromDate.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{reportVM.ToDate.ToString("yyyy-MM-dd HH:mm:ss")}'";
            query = $@"SELECT a.*, b.UserName, b.FirstName, b.Address FROM Orders a LEFT JOIN AspNetUsers b ON a.UserId = b.Id WHERE ShippingStatus != '' AND a.Status = 'Successed' AND {dateTime}";

            var orders = new List<OrdersUserViewModel>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                orders = connection.Query<OrdersUserViewModel>(query).ToList();
            }

            return orders;
        }

        public List<ProductManagement> GetSizeList()
        {
            var sql = @"SELECT DISTINCT Size  FROM ProductManagement  ORDER BY Size";

            var size = new List<ProductManagement>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                size = connection.Query<ProductManagement>(sql).ToList();
            }

            return size;
        }

        public List<OrdersUserViewModel> GetCartDetailListWhereUserId(string userId)
        {
            var sql = @"SELECT a.*, b.UserName, b.FirstName, b.Address, c.ShippingPrice FROM CartDetail a LEFT JOIN AspNetUsers b ON a.UserId = b.Id LEFT JOIN Shipping c ON a.Shipping = c.Name WHERE b.Id = @userId";

            var orders = new List<OrdersUserViewModel>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                orders = connection.Query<OrdersUserViewModel>(sql, new { userId }).ToList();
            }

            return orders;
        }

        public List<CartDetail> GetCartDetailList(string userId)
        {
            var sql = @"SELECT * FROM CartDetail WHERE UserId = @userId";

            var cartDetailLsit = new List<CartDetail>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                cartDetailLsit = connection.Query<CartDetail>(sql, new { userId }).ToList();
            }

            return cartDetailLsit;
        }

        public List<CartDetail> SaveCartDetailList(CartDetail cartDetail)
        { 
            var sql = $@"UPDATE CartDetail SET SlipImage = '{cartDetail.SlipImage}'";

            var orders = new List<CartDetail>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                orders = connection.Query<CartDetail>(sql).ToList();
            }

            return orders;

        }

        public List<BestSealler> GetOrderBestSeller(ReportVM reportVM)
        {
            var query = "";
            var dateTime = "";
            dateTime = $@"o.CreatedAt BETWEEN '{reportVM.FromDate.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{reportVM.ToDate.ToString("yyyy-MM-dd HH:mm:ss")}'";
            query = $@"SELECT TOP 1 o.Name AS Name, o.Image AS Image, SUM(o.Quantity) AS total FROM Orders AS o INNER JOIN ProductManagement AS p ON o.ProductId = p.Id WHERE ShippingStatus != '' AND o.Status = 'Successed' AND {dateTime} GROUP BY o.ProductId, o.Name, o.Image ORDER BY SUM(o.Quantity) DESC";

            var orders = new List<BestSealler>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                orders = connection.Query<BestSealler>(query).ToList();
            }

            return orders;
        }

        public List<WorstsellerList> GetOrderWorstsellerList(ReportVM reportVM)
        {
            var query = "";
            var dateTime = "";
            dateTime = $@"o.CreatedAt BETWEEN '{reportVM.FromDate.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{reportVM.ToDate.ToString("yyyy-MM-dd HH:mm:ss")}'";
            query = $@"SELECT TOP 1 o.Name AS Name, o.Image, SUM(o.Quantity) AS total FROM Orders AS o INNER JOIN ProductManagement AS p ON o.ProductId = p.Id WHERE ShippingStatus != '' AND o.Status = 'Successed' AND {dateTime} GROUP BY o.ProductId, o.Name, o.Image ORDER BY SUM(o.Quantity)";

            var orders = new List<WorstsellerList>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                orders = connection.Query<WorstsellerList>(query).ToList();
            }

            return orders;
        }
    }
}
