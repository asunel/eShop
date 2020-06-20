using eShop.Common.DTO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
namespace eShop.DataAccess.Models
{
    public class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();

                if (!context.Products.Any())
                {
                    context.AddRange
                    (
                        new Product
                        {
                            Name = "Dumbbell",
                            Price = 2000,
                            Description = "Rust free iron made",
                            Image = "Images/products/dumbbell.jpg",
                        },
                        new Product
                        {
                            Name = "Mat",
                            Price = 200,
                            Description = "Beautiful carpet with attractive colors",
                            Image = "Images/products/mat.jpg",
                        },
                        new Product
                        {
                            Name = "Realme",
                            Price = 20000,
                            Description = "Capture life with the camera having high resolution",
                            Image = "Images/products/realme.jpeg",
                        },
                        new Product
                        {
                            Name = "Shoes ",
                            Price = 1000,
                            Description = "Be at comfort with quality shoes.",
                            Image = "Images/products/shoes.jpg",
                        },
                        new Product
                        {
                            Name = "Speaker",
                            Price = 3000,
                            Description = "Sound with high quality",
                            Image = "Images/products/speaker.jpeg",
                        },
                        new Product
                        {
                            Name = "Washing machine",
                            Price = 15000,
                            Description = "Clean clothes with latest technology machine",
                            Image = "Images/products/washing-machine.jpg",
                        }
                    );
                }

                context.SaveChanges();
            }
        }

    }
}
