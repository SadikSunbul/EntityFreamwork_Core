




using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

class denemeContext:DbContext

{
    StreamWriter _log = new StreamWriter("denem.txt", append: true);
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.LogTo( message =>  _log.WriteLine(message),LogLevel.Information)
            .EnableDetailedErrors();

    }


    public override void Dispose()
    {
        base.Dispose();
        _log.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _log.DisposeAsync(); 
    }

}