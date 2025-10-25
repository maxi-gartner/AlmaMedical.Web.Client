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
            UpdateAppointment,    // Alias de EditAppointment (para compatibilidad)
            CancelAppointment,
            ConfirmAppointment,
            CompleteAppointment,
            DeleteAppointment,
            ViewAllProfessionalsSchedule, // Ver agenda de todos

            // 🆕 Tareas/Pre-citas
            ViewTasks,
            CreateTask,
            EditTask,
            DeleteTask,
            CompleteTask,
            AssignTask,

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
                Permission.UpdateAppointment,
                Permission.CancelAppointment,
                Permission.ConfirmAppointment,
                Permission.CompleteAppointment,
                Permission.DeleteAppointment,
                Permission.ViewAllProfessionalsSchedule,
                // Tareas
                Permission.ViewTasks,
                Permission.CreateTask,
                Permission.EditTask,
                Permission.DeleteTask,
                Permission.CompleteTask,
                Permission.AssignTask,
                // Ventas
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
                Permission.UpdateAppointment,
                Permission.CancelAppointment,
                Permission.ConfirmAppointment,
                Permission.CompleteAppointment,
                Permission.DeleteAppointment,
                Permission.ViewAllProfessionalsSchedule,
                // Tareas
                Permission.ViewTasks,
                Permission.CreateTask,
                Permission.EditTask,
                Permission.DeleteTask,
                Permission.CompleteTask,
                Permission.AssignTask,
                // Ventas
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
                Permission.UpdateAppointment,
                Permission.CancelAppointment,
                Permission.ConfirmAppointment,
                Permission.ViewAllProfessionalsSchedule,
                // Tareas - Recepcionistas pueden ver y completar tareas asignadas
                Permission.ViewTasks,
                Permission.CompleteTask,
                // Ventas
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

            // Si busca UpdateAppointment pero el rol tiene EditAppointment, también es válido (alias)
            if (permission == Permission.UpdateAppointment && _rolePermissions[userRole].Contains(Permission.EditAppointment))
                return true;

            // Si busca EditAppointment pero el rol tiene UpdateAppointment, también es válido (alias)
            if (permission == Permission.EditAppointment && _rolePermissions[userRole].Contains(Permission.UpdateAppointment))
                return true;

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
                Permission.ViewClientHistory => "Ver historial de clientes",
                Permission.ViewProducts => "Ver productos",
                Permission.CreateProduct => "Crear productos",
                Permission.EditProduct => "Editar productos",
                Permission.DeleteProduct => "Eliminar productos",
                Permission.ManageStock => "Gestionar stock",
                Permission.ManagePrices => "Gestionar precios",
                Permission.ViewAppointments => "Ver turnos",
                Permission.CreateAppointment => "Crear turnos",
                Permission.EditAppointment => "Editar turnos",
                Permission.UpdateAppointment => "Actualizar turnos",
                Permission.CancelAppointment => "Cancelar turnos",
                Permission.ConfirmAppointment => "Confirmar turnos",
                Permission.CompleteAppointment => "Completar turnos",
                Permission.DeleteAppointment => "Eliminar turnos",
                Permission.ViewAllProfessionalsSchedule => "Ver agenda de todos los profesionales",
                Permission.ViewTasks => "Ver tareas",
                Permission.CreateTask => "Crear tareas",
                Permission.EditTask => "Editar tareas",
                Permission.DeleteTask => "Eliminar tareas",
                Permission.CompleteTask => "Completar tareas",
                Permission.AssignTask => "Asignar tareas",
                Permission.ViewSales => "Ver ventas",
                Permission.CreateSale => "Crear ventas",
                Permission.EditSale => "Editar ventas",
                Permission.CancelSale => "Cancelar ventas",
                Permission.ApplyDiscounts => "Aplicar descuentos",
                Permission.ViewSalesReport => "Ver reporte de ventas",
                Permission.ViewExpenses => "Ver gastos",
                Permission.CreateExpense => "Registrar gastos",
                Permission.EditExpense => "Editar gastos",
                Permission.DeleteExpense => "Eliminar gastos",
                Permission.ViewExpenseReport => "Ver reporte de gastos",
                Permission.ManageUsers => "Gestionar usuarios",
                Permission.ManageSettings => "Gestionar configuración",
                Permission.ManageIntegrations => "Gestionar integraciones",
                Permission.ManagePlans => "Gestionar planes",
                Permission.SendNotifications => "Enviar notificaciones",
                Permission.ViewInternalMessages => "Ver mensajes internos",
                _ => permission.ToString()
            };
        }
    }
}