import { Pencil, Trash2 } from "lucide-react";

export default function ProductsList({ items = [], isAdmin, onEdit, onDelete }) {
  const API_BASE = "https://localhost:7132";

  // 🧪 DEBUG GLOBAL
  console.log("PRODUCTS LIST ITEMS =>", items);

  if (!items.length) {
    return (
      <div className="text-sm text-[#8a7768] p-6">
        Aucun produit trouvé.
      </div>
    );
  }

  return (
    <div className="p-6 min-h-screen">
      <div className="overflow-x-auto">
        <table className="w-full text-sm text-left border-separate border-spacing-y-3">

          {/* HEADER */}
          <thead>
            <tr className="text-[#8b4f2f]">
              <th>#</th>
              <th>Image</th>
              <th>Nom</th>
              <th>Description</th>
              <th>Prix</th>
              <th>Quantité</th>
              <th>Catégorie</th>
              <th className="text-right">Actions</th>
            </tr>
          </thead>

          {/* BODY */}
          <tbody>
            {items.map((product, index) => {

              // 🧪 DEBUG لكل product
              console.log("PRODUCT =>", product);
              console.log("QUANTITY DEBUG =>", {
                stockQuantity: product.stockQuantity,
                quantity: product.quantity,
                StockQuantity: product.StockQuantity
              });

              return (
                <tr
                  key={product.id}
                  className="border border-[#e7d5c4] rounded-xl shadow-sm "
                >
                  {/* INDEX */}
                  <td className="px-3 py-4 font-medium text-[#8a7768]">
                    {index + 1}
                  </td>

                  {/* IMAGE */}
                  <td className="px-3 py-4">
                    <div className="w-12 h-12 rounded-full overflow-hidden ">
                      {product.imageUrl ? (
                        <img
                          src={`${API_BASE}${product.imageUrl}`}
                          alt={product.name ?? product.Name}
                          className="w-full h-full object-cover"
                        />
                      ) : (
                        <div className="w-full h-full flex items-center justify-center text-xs text-[#8a7768]">
                          -
                        </div>
                      )}
                    </div>
                  </td>

                  {/* NAME */}
                  <td className="px-3 py-4 font-medium text-[#8b4f2f]">
                    {product.name ?? product.Name}
                  </td>

                  {/* DESCRIPTION */}
                  <td className="px-3 py-4 text-[#8a7768]">
                    {product.description ?? product.Description ?? "-"}
                  </td>

                  {/* PRICE */}
                  <td className="px-3 py-4 text-[#8b4f2f] font-semibold">
                    {product.price ?? product.Price} €
                  </td>

                  {/* QUANTITY 🔥 */}
                  <td className="px-3 py-4 text-[#8a7768] font-semibold">
                    {(product.stockQuantity ??
                      product.quantity ??
                      product.StockQuantity ??
                      0)}{" "}
                    {product.unit ?? product.Unit}
                  </td>

                  {/* CATEGORY */}
                  <td className="px-3 py-4 text-[#8a7768]">
                    {product.categoryName ?? product.CategoryName ?? "-"}
                  </td>

                  {/* ACTIONS */}
                  <td className="px-3 py-4 text-right">
                    {isAdmin && (
                      <div className="flex justify-end gap-2">

                        <button
                          onClick={() => onEdit(product)}
                          className="px-3 py-1 rounded-md bg-[#f8a5a5] text-white text-xs hover:bg-[#e97f7f]"
                        >
                          Modifier
                        </button>

                        <button
                          onClick={() => onDelete(product.id)}
                          className="p-2 rounded-md bg-[#f3e2cf] text-[#b28a63] hover:bg-[#ead7c1]"
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>

                      </div>
                    )}
                  </td>
                </tr>
                
              );
            })}
          </tbody>

        </table>
      </div>
    </div>
  );
}