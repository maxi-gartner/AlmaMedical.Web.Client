using Microsoft.AspNetCore.Components;
using AlmaMedical.Web.Client.Services;

namespace AlmaMedical.Web.Client.Base
{
    public class AuthorizedComponentBase : ComponentBase, IDisposable
    {
        [Inject]
        protected CurrentUserService CurrentUser { get; set; } = default!;

        [Inject]
        protected RolePermissionService PermissionService { get; set; } = default!;

        protected override void OnInitialized()
        {
            // Suscribirse al evento de cambio de rol
            CurrentUser.OnRoleChanged += OnUserRoleChanged;
            base.OnInitialized();
        }

        // Este método se ejecuta cuando cambia el rol
        private void OnUserRoleChanged()
        {
            InvokeAsync(() =>
            {
                StateHasChanged(); // Actualizar la UI
            });
        }

        // Método helper para verificar permisos (opcional, para simplificar)
        protected bool HasPermission(RolePermissionService.Permission permission)
        {
            return PermissionService.HasPermission(CurrentUser.UserRole, permission);
        }

        // Método helper para verificar múltiples permisos
        protected bool HasAnyPermission(params RolePermissionService.Permission[] permissions)
        {
            return PermissionService.HasAnyPermission(CurrentUser.UserRole, permissions);
        }

        // Limpieza cuando se destruye el componente
        public void Dispose()
        {
            CurrentUser.OnRoleChanged -= OnUserRoleChanged;
            GC.SuppressFinalize(this);
        }
    }
}