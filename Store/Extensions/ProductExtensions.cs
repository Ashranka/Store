using Store.Entities;

namespace Store.Extensions
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> Sort(this IQueryable<Product> query, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Name); 

            // Aplica el ordenamiento según el valor de orderBy
            query = orderBy switch
            {
                "price" => query.OrderBy(p => p.Price),                // Ordena por precio ascendente
                "priceDesc" => query.OrderByDescending(p => p.Price),  // Ordena por precio descendente
                _ => query.OrderBy(p => p.Name)                        // Ordena por nombre (default)
            };

            return query;

        }


        public static IQueryable<Product> Search(this IQueryable<Product> query, string searchTem)
        {
            if (string.IsNullOrWhiteSpace(searchTem)) return query;

            var loweCaseSearchTerm = searchTem.Trim().ToLower();

            //return query.Where(p => p.Name.Trim().ToLower() == lowweCaseSearchTerm); Busca la coincidencia por el nombre exacto
            return query.Where(p => p.Name.ToLower().Contains(loweCaseSearchTerm)); // Revisa que dentro de las sub cadenas tenga el parametro que queremos
        }

        public static IQueryable<Product> Filter(this IQueryable<Product> query, string brands, string type)
        {
            var brandList = new List<string>();
            var typeList = new List<string>();

            if (!string.IsNullOrEmpty(brands))
            {

                brandList.AddRange(brands.ToLower().Split(",").ToList());
            }

            if (!string.IsNullOrEmpty(type))
            {

                typeList.AddRange(type.ToLower().Split(",").ToList());
            }

            query = query.Where(p => brandList.Count == 0 || brandList.Contains(p.Brand.ToLower()));
            query = query.Where(p => typeList.Count == 0 || typeList.Contains(p.Type.ToLower()));

            return query;
        }

    }
}
