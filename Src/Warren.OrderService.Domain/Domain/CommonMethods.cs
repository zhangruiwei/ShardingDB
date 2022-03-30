using Microsoft.Extensions.Options;
using Warren.OrderService.Common.Configuration;

namespace Warren.OrderService.Domain.Domain
{
    public class CommonMethods : Interface.ICommonMethods
    {
        private readonly DatabaseConfiguration _databaseConfiguration;
        public CommonMethods(IOptionsSnapshot<DatabaseConfiguration> options)
        {
            _databaseConfiguration = options.Value;
        }

    }
}
