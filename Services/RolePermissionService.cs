namespace AlmaMedical.Web.Client.Services
{
    public class RolePermissionService
    {
        // Permisos disponibles en el sistema
        public enum Permission
        {
            // Dashboard
            ViewDashboard,
            ViewReports,
            ViewFinancialMetrics,

            // Clientes
            ViewClients,
            CreateClient,
            EditClient,
            DeleteClient,
            ViewClientHistory,

            // Productos
            ViewProducts,
            CreateProduct,
            EditProduct,
            DeleteProduct,
            ManageStock,
            ManagePrices,

            // Turnos
            ViewAppointments,
            CreateAppointment,
            EditAppointment,
            CancelAppointment,
            ConfirmAppointment,
            CompleteAppointment,
            ViewAllProfessionalsSchedule, // Ver agenda de todos

            // Ventas
            ViewSales,
            CreateSale,
            EditSale,
            CancelSale,
            ApplyDiscounts,
            ViewSalesReport,

            // Gastos
            ViewExpenses,
            CreateExpense,
            EditExpense,
            DeleteExpense,
            ViewExpenseReport,

            // Configuraciones
            ManageUsers,
            ManageSettings,
            ManageIntegrations,
            ManagePlans,

            // Notificaciones
            SendNotifications,
            ViewInternalMessages
        }

        // Definición de permisos por rol
        private readonly Dictionary<string, List<Permission>> _rolePermissions = new()
        {
            // Super Admin - acceso total
            ["SuperAdmin"] = Enum.GetValues<Permission>().ToList(),

            // Dueño del negocio - casi todo excepto gestión de planes
            ["TenantAdmin"] = new List<Permission>
            {
                Permission.ViewDashboard,
                Permission.ViewReports,
                Permission.ViewFinancialMetrics,
                Permission.ViewClients,
                Permission.CreateClient,
                Permission.EditClient,
                Permission.DeleteClient,
                Permission.ViewClientHistory,
                Permission.ViewProducts,
                Permission.CreateProduct,
                Permission.EditProduct,
                Permission.DeleteProduct,
                Permission.ManageStock,
                Permission.ManagePrices,
                Permission.ViewAppointments,
                Permission.CreateAppointment,
                Permission.EditAppointment,
                Permission.CancelAppointment,
                Permission.ConfirmAppointment,
                Permission.CompleteAppointment,
                Permission.ViewAllProfessionalsSchedule,
                Permission.ViewSales,
                Permission.CreateSale,
                Permission.EditSale,
                Permission.CancelSale,
                Permission.ApplyDiscounts,
                Permission.ViewSalesReport,
                Permission.ViewExpenses,
                Permission.CreateExpense,
                Permission.EditExpense,
                Permission.DeleteExpense,
                Permission.ViewExpenseReport,
                Permission.ManageUsers,
                Permission.ManageSettings,
                Permission.ManageIntegrations,
                Permission.SendNotifications,
                Permission.ViewInternalMessages
            },

            // Profesional - todo menos gestión de usuarios y configuraciones críticas
            ["Professional"] = new List<Permission>
            {
                Permission.ViewDashboard,
                Permission.ViewReports,
                Permission.ViewFinancialMetrics,
                Permission.ViewClients,
                Permission.CreateClient,
                Permission.EditClient,
                Permission.ViewClientHistory,
                Permission.ViewProducts,
                Permission.CreateProduct,
                Permission.EditProduct,
                Permission.ManageStock,
                Permission.ManagePrices,
                Permission.ViewAppointments,
                Permission.CreateAppointment,
                Permission.EditAppointment,
                Permission.CancelAppointment,
                Permission.ConfirmAppointment,
                Permission.CompleteAppointment,
                Permission.ViewAllProfessionalsSchedule,
                Permission.ViewSales,
                Permission.CreateSale,
                Permission.EditSale,
                Permission.ApplyDiscounts,
                Permission.ViewSalesReport,
                Permission.ViewExpenses,
                Permission.CreateExpense,
                Permission.EditExpense,
                Permission.ViewExpenseReport,
                Permission.SendNotifications,
                Permission.ViewInternalMessages
            },

            // Recepcionista/Secretaria - gestión operativa sin acceso financiero profundo
            ["Receptionist"] = new List<Permission>
            {
                Permission.ViewDashboard,
                Permission.ViewClients,
                Permission.CreateClient,
                Permission.EditClient,
                Permission.ViewClientHistory,
                Permission.ViewProducts,
                Permission.ViewAppointments,
                Permission.CreateAppointment,
                Permission.EditAppointment,
                Permission.CancelAppointment,
                Permission.ConfirmAppointment,
                Permission.ViewAllProfessionalsSchedule,
                Permission.ViewSales,
                Permission.CreateSale,
                Permission.SendNotifications,
                Permission.ViewInternalMessages
            },

            // Cliente final - solo ver su info
            ["Customer"] = new List<Permission>
            {
                Permission.ViewAppointments, // Solo SUS turnos
                Permission.CreateAppointment, // Reservar turno
            }
        };

        // Verifica si un rol tiene un permiso específico
        public bool HasPermission(string userRole, Permission permission)
        {
            if (!_rolePermissions.ContainsKey(userRole))
                return false;

            return _rolePermissions[userRole].Contains(permission);
        }

        // Verifica si tiene al menos uno de varios permisos
        public bool HasAnyPermission(string userRole, params Permission[] permissions)
        {
            return permissions.Any(p => HasPermission(userRole, p));
        }

        // Verifica si tiene todos los permisos especificados
        public bool HasAllPermissions(string userRole, params Permission[] permissions)
        {
            return permissions.All(p => HasPermission(userRole, p));
        }

        // Obtiene todos los permisos de un rol
        public List<Permission> GetRolePermissions(string userRole)
        {
            return _rolePermissions.ContainsKey(userRole)
                ? _rolePermissions[userRole]
                : new List<Permission>();
        }

        // Obtiene descripción legible del permiso
        public string GetPermissionDescription(Permission permission)
        {
            return permission switch
            {
                Permission.ViewDashboard => "Ver panel principal",
                Permission.ViewReports => "Ver reportes",
                Permission.ViewFinancialMetrics => "Ver métricas financieras",
                Permission.ViewClients => "Ver clientes",
                Permission.CreateClient => "Crear clientes",
                Permission.EditClient => "Editar clientes",
                Permission.DeleteClient => "Eliminar clientes",
                Permission.ViewProducts => "Ver productos",
                Permission.CreateProduct => "Crear productos",
                Permission.ManagePrices => "Gestionar precios",
                Permission.ViewAppointments => "Ver turnos",
                Permission.CreateAppointment => "Crear turnos",
                Permission.ViewSales => "Ver ventas",
                Permission.CreateSale => "Crear ventas",
                Permission.ApplyDiscounts => "Aplicar descuentos",
                Permission.ViewExpenses => "Ver gastos",
                Permission.CreateExpense => "Registrar gastos",
                Permission.ManageUsers => "Gestionar usuarios",
                Permission.ManageSettings => "Gestionar configuración",
                _ => permission.ToString()
            };
        }
    }
}