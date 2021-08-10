using System.Collections.Generic;

namespace Marketplace.Domain.Models.permissions
{
    public static class permission
    {
        public static List<dto.Item> GetPermissions()
        {
            var _retorno = new List<dto.Item>();
            _retorno.AddRange(Language.permissions);
            _retorno.AddRange(Topic.permissions);
            _retorno.AddRange(User.permissions);
            _retorno.AddRange(GroupPermission.permissions);
            _retorno.AddRange(Provider.permissions);
            _retorno.AddRange(Customer.permissions);
            _retorno.AddRange(Reports.permissions);
            _retorno.AddRange(Bank.permissions);
            return _retorno;
        }

        public static class Bank
        {
            public const string View = "bank.view";
            public const string Create = "bank.create";
            public const string Edit = "bank.edit";
            public const string Delete = "bank.delete";

            public static List<dto.Item> permissions
            {
                get
                {
                    return new List<dto.Item>()
                    {
                        new dto.Item() {label = "Visualizar Bancos", value = View },
                        new dto.Item() {label = "Criar Bancos", value = Create },
                        new dto.Item() {label = "Editar Bancos", value = Edit },
                        new dto.Item() {label = "Excluir Bancos", value = Delete }
                    };
                }
            }
        }

        public static class Reports
        {
            public const string ViewAppointment = "reports.appointment.view";
            public const string ViewDashboard = "reports.dashboard.view";

            public static List<dto.Item> permissions
            {
                get
                {
                    return new List<dto.Item>()
                    {
                        new dto.Item() {label = "Visualizar Agendamentos", value = ViewAppointment },
                        new dto.Item() {label = "Visualizar Dashboard", value = ViewDashboard }
                    };
                }
            }
        }

        public static class Customer
        {
            public const string View = "customer.view";
            public const string Create = "customer.create";
            public const string Edit = "customer.edit";
            public const string Delete = "customer.delete";

            public static List<dto.Item> permissions
            {
                get
                {
                    return new List<dto.Item>()
                    {
                        new dto.Item() {label = "Visualizar Clientes", value = View },
                        new dto.Item() {label = "Criar Clientes", value = Create },
                        new dto.Item() {label = "Editar Clientes", value = Edit },
                        new dto.Item() {label = "Excluir Clientes", value = Delete }
                    };
                }
            }
        }

        public static class Provider
        {
            public const string View = "provider.view";
            public const string Create = "provider.create";
            public const string Edit = "provider.edit";
            public const string Delete = "provider.delete";

            public static List<dto.Item> permissions
            {
                get
                {
                    return new List<dto.Item>()
                    {
                        new dto.Item() {label = "Visualizar Psicólogas", value = View },
                        new dto.Item() {label = "Criar Psicólogas", value = Create },
                        new dto.Item() {label = "Editar Psicólogas", value = Edit },
                        new dto.Item() {label = "Excluir Psicólogas", value = Delete }
                    };
                }
            }
        }

        public static class GroupPermission
        {
            public const string View = "group.permission.view";
            public const string Create = "group.permission.create";
            public const string Edit = "group.permission.edit";
            public const string Delete = "group.permission.delete";

            public static List<dto.Item> permissions
            {
                get
                {
                    return new List<dto.Item>()
                    {
                        new dto.Item() {label = "Visualizar Grupo Permissão", value = View },
                        new dto.Item() {label = "Criar Grupo Permissão", value = Create },
                        new dto.Item() {label = "Editar Grupo Permissão", value = Edit },
                        new dto.Item() {label = "Excluir Grupo Permissão", value = Delete }
                    };
                }
            }
        }

        public static class User
        {
            public const string View = "user.view";
            public const string Create = "user.create";
            public const string Edit = "user.edit";
            public const string Delete = "user.delete";

            public static List<dto.Item> permissions
            {
                get
                {
                    return new List<dto.Item>()
                    {
                        new dto.Item() {label = "Visualizar Usuários", value = View },
                        new dto.Item() {label = "Criar Usuários", value = Create },
                        new dto.Item() {label = "Editar Usuários", value = Edit },
                        new dto.Item() {label = "Excluir Usuários", value = Delete }
                    };
                }
            }
        }

        public static class Topic
        {
            public const string View = "topic.view";
            public const string Create = "topic.create";
            public const string Edit = "topic.edit";
            public const string Delete = "topic.delete";

            public static List<dto.Item> permissions
            {
                get
                {
                    return new List<dto.Item>()
                    {
                        new dto.Item() {label = "Visualizar Topicos", value = View },
                        new dto.Item() {label = "Criar Topicos", value = Create },
                        new dto.Item() {label = "Editar Topicos", value = Edit },
                        new dto.Item() {label = "Excluir Topicos", value = Delete }
                    };
                }
            }
        }

        public static class Language
        {
            public const string View = "language.view";
            public const string Create = "language.create";
            public const string Edit = "language.edit";
            public const string Delete = "language.delete";

            public static List<dto.Item> permissions
            {
                get
                {
                    return new List<dto.Item>()
                    {
                        new dto.Item() {label = "Visualizar Linguagens", value = View },
                        new dto.Item() {label = "Criar Linguagens", value = Create },
                        new dto.Item() {label = "Editar Linguagens", value = Edit },
                        new dto.Item() {label = "Excluir Linguagens", value = Delete }
                    };
                }
            }
        }
    }
}
