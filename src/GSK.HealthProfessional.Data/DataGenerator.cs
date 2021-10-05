using GSK.HealthProfessional.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSK.HealthProfessional.Data
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ContextInMemory(
                serviceProvider.GetRequiredService<DbContextOptions<ContextInMemory>>()))
            {
                if (context.State.Any())
                {
                    return;   
                }

                                                         
                context.State.Add(new State(){StateId = "AC", Description = "Acre"});
                context.State.Add(new State(){StateId = "AL", Description = "Alagoas"});
                context.State.Add(new State(){StateId = "AM", Description = "Amazonas"});
                context.State.Add(new State(){StateId = "AP", Description = "Amapá"});
                context.State.Add(new State(){StateId = "BH", Description = "Bahia"});
                context.State.Add(new State(){StateId = "CE", Description = "Ceará"});
                context.State.Add(new State(){StateId = "DF", Description = "Distrito Federal"});
                context.State.Add(new State(){StateId = "ES", Description = "Espírito Santo"});
                context.State.Add(new State(){StateId = "GO", Description = "Goiás"});
                context.State.Add(new State(){StateId = "MA", Description = "Maranhão"});
                context.State.Add(new State(){StateId = "MG", Description = "Minas Gerais"});
                context.State.Add(new State(){StateId = "MS", Description = "Mato Grosso do Sul"});
                context.State.Add(new State(){StateId = "MT", Description = "Mato Grosso"});
                context.State.Add(new State(){StateId = "PA", Description = "Pará"});
                context.State.Add(new State(){StateId = "PB", Description = "Paraíba"});
                context.State.Add(new State(){StateId = "PE", Description = "Pernambuco"});
                context.State.Add(new State(){StateId = "PI", Description = "Piauí"});
                context.State.Add(new State(){StateId = "PR", Description = "Paraná"});
                context.State.Add(new State(){StateId = "RJ", Description = "Rio de Janeiro"});
                context.State.Add(new State(){StateId = "RN", Description = "Rio Grande do Norte"});
                context.State.Add(new State(){StateId = "RO", Description = "Rondônia"});
                context.State.Add(new State(){StateId = "RR", Description = "Roraima"});
                context.State.Add(new State(){StateId = "RS", Description = "Rio Grande do Sul"});
                context.State.Add(new State(){StateId = "SC", Description = "Santa Catarina"});
                context.State.Add(new State(){StateId = "SE", Description = "Sergipe"});
                context.State.Add(new State(){StateId = "SP", Description = "São Paulo"});
                context.State.Add(new State(){StateId = "TO", Description = "Tocantins"});

                
                context.SaveChanges();
            }
        }
    }
}
