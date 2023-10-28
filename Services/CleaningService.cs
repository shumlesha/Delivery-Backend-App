using webNET_Hits_backend_aspnet_project_1.Models;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public class CleaningService: IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CleaningService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CleanOldTokens, null, TimeSpan.FromSeconds(10), TimeSpan.FromHours(3));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
    private async void CleanOldTokens(object state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            var nonActualTokens = _context.BannedTokens.Where(token => token.AdditionDate <= DateTime.UtcNow.AddHours(-3)).ToList();
        
            if (nonActualTokens.Count > 0)
            {
                _context.BannedTokens.RemoveRange(nonActualTokens);
                await _context.SaveChangesAsync();
            }
        }
        
    }
}