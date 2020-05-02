using BexsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BexsAPI.Repository
{
    public class Repository : DbContext
    {

        public Repository(DbContextOptions<Repository> options) : base(options) { }

        public DbSet<Pergunta> PerguntaDB { get; set; }

        public DbSet<Resposta> RespotasDB { get; set; }

      
    }
}
