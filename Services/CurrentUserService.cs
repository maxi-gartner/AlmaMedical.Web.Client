namespace AlmaMedical.Web.Client.Services
{
    public class CurrentUserService
    {
        // Evento que se dispara cuando cambia el rol
        public event Action? OnRoleChanged;

        public string UserId { get; set; } = "user-123";
        public string Email { get; set; } = "admin@clinica.com";
        public string FirstName { get; set; } = "Dr. Juan";
        public string LastName { get; set; } = "Pérez";

        private string _userRole = "Professional";
        public string UserRole
        {
            get => _userRole;
            private set
            {
                if (_userRole != value)
                {
                    _userRole = value;
                    OnRoleChanged?.Invoke(); // Notificar cambio
                }
            }
        }

        public Guid? TenantId { get; set; } = Guid.NewGuid();

        public string FullName => $"{FirstName} {LastName}";

        // Método para cambiar de rol
        public void SetRole(string role)
        {
            UserRole = role;
        }
    }
}