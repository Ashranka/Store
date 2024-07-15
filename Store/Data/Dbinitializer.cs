using Store.Entities;

namespace Store.Data
{
    public static class Dbinitializer
    {
        //SOlo se usara Para crear una seed en la base de datos
        public static void Initialize(StoreContext context)
        {
            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product { Name = "Laptop Pro", Description = "High-performance laptop with 16GB RAM", Price = 120000, PictureUrl = "/images/laptop_pro.png", Type = "Electronics", Brand = "TechBrand", QuantityInStock = 50 },
                new Product { Name = "Smartphone X", Description = "Latest smartphone with OLED display", Price = 90000, PictureUrl = "/images/smartphone_x.png", Type = "Electronics", Brand = "MobileCorp", QuantityInStock = 200 },
                new Product { Name = "Wireless Headphones", Description = "Noise-cancelling wireless headphones", Price = 20000, PictureUrl = "/images/headphones.png", Type = "Accessories", Brand = "SoundTech", QuantityInStock = 150 },
                new Product { Name = "Gaming Console", Description = "Next-gen gaming console", Price = 50000, PictureUrl = "/images/gaming_console.png", Type = "Gaming", Brand = "GameWorld", QuantityInStock = 75 },
                new Product { Name = "4K TV", Description = "Ultra HD 4K television", Price = 60000, PictureUrl = "/images/tv.png", Type = "Electronics", Brand = "ViewMax", QuantityInStock = 30 },
                new Product { Name = "Smartwatch", Description = "Fitness tracking smartwatch", Price = 15000, PictureUrl = "/images/smartwatch.png", Type = "Accessories", Brand = "WristTech", QuantityInStock = 100 },
                new Product { Name = "Bluetooth Speaker", Description = "Portable Bluetooth speaker", Price = 8000, PictureUrl = "/images/speaker.png", Type = "Accessories", Brand = "AudioVibe", QuantityInStock = 120 },
                new Product { Name = "Tablet S", Description = "10-inch tablet with high resolution", Price = 30000, PictureUrl = "/images/tablet.png", Type = "Electronics", Brand = "TechBrand", QuantityInStock = 60 },
                new Product { Name = "Digital Camera", Description = "DSLR camera with 24MP sensor", Price = 45000, PictureUrl = "/images/camera.png", Type = "Photography", Brand = "PhotoPro", QuantityInStock = 40 },
                new Product { Name = "E-Reader", Description = "E-reader with glare-free display", Price = 12000, PictureUrl = "/images/ereader.png", Type = "Electronics", Brand = "ReadEasy", QuantityInStock = 80 },
                new Product { Name = "Home Theater System", Description = "Surround sound home theater system", Price = 70000, PictureUrl = "/images/home_theater.png", Type = "Electronics", Brand = "SoundTech", QuantityInStock = 25 },
                new Product { Name = "Action Camera", Description = "Waterproof action camera with 4K recording", Price = 25000, PictureUrl = "/images/action_camera.png", Type = "Photography", Brand = "AdventureCam", QuantityInStock = 90 },
                new Product { Name = "VR Headset", Description = "Virtual reality headset with motion sensors", Price = 35000, PictureUrl = "/images/vr_headset.png", Type = "Gaming", Brand = "VirtuaWorld", QuantityInStock = 55 },
                new Product { Name = "Electric Scooter", Description = "Foldable electric scooter", Price = 40000, PictureUrl = "/images/electric_scooter.png", Type = "Transportation", Brand = "EcoRide", QuantityInStock = 35 },
                new Product { Name = "Fitness Tracker", Description = "Water-resistant fitness tracker", Price = 10000, PictureUrl = "/images/fitness_tracker.png", Type = "Accessories", Brand = "FitTrack", QuantityInStock = 110 }
            };
            //hacen la misma funcion
            //foreach (var product in products)
            //{
            //    context.Products.Add(product);
            //}
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
